using System;
using Playground.Firebase;
using UniRx;
using UnityEngine;
using Zenject;

namespace Playground.Title
{
    public class LoginPresenter : MonoBehaviour
    {
        #region Private

        [Inject] private readonly ApplicationManager _applicationManager;
        [Inject] private readonly FirebaseManager _firebaseManager;
        [Inject] private readonly UserInfoManager _userInfoManager;

        private readonly LoginModel _model = new ();
        
        private LoginView _view;
        private CompositeDisposable _disposable;

        #endregion

        private void Awake() => _view = GetComponent<LoginView>();

        /// <summary>
        /// 로그인 단계 실행
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> Execute()
        {
            return Observable.Create<Unit>(observer =>
            {
                _disposable = new();

                _view.SetGoogleLoginButtonText(_model.GoogleLoginButtonText);
                _view.SetButtonActive(true);

                _view.OnGoogleLoginButtonClick()
                    .Do(_ => Debug.Log("OnGoogleLoginButtonClick"))
                    .Subscribe(_ => _model.UpdateStep(LoginStep.SignIn))
                    .AddTo(this);

                _model.Step
                    .Where(step => step == LoginStep.SignIn)
                    .Do(_ => Debug.Log("SignIn"))
                    .SelectMany(_ => _firebaseManager.GoogleSignInAsObservable())
                    .Subscribe
                    (
                        onNext: result =>
                        {
                            _userInfoManager.SetUserInfo(result);
                            _model.UpdateStep(LoginStep.Complete);
                        },
                        onError: exception =>
                        {
                            Debug.Log(exception.Message);
                            _applicationManager.Restart();
                        }
                    )
                    .AddTo(this);
            
                _model.Step
                    .Where(step => step == LoginStep.Complete)
                    .Do(_ => Debug.Log("Complete"))
                    .First()
                    .Subscribe(_ =>
                    {
                        observer.OnNext(new Unit());
                        observer.OnCompleted();
                    });

                return Disposable.Create(() => _disposable?.Dispose());
            }).ObserveOnMainThread();
        }
    }
}
