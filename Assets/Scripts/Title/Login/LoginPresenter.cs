using System;
using System.Threading.Tasks;
using Firebase.Auth;
using UniRx;
using UnityEngine;
using Zenject;

namespace Playground.Title
{
    public class LoginPresenter : MonoBehaviour
    {
        #region Private

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
                    .SelectMany(_ => SignIn().ToObservable())
                    .Subscribe(_ => {})
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
            });
        }

        /// <summary>
        /// 구글 로그인 진행
        /// </summary>
        private async Task SignIn()
        {
            var credential = GoogleAuthProvider.GetCredential(_model.GoogleIdToken, null);
            
            await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    // TODO: 로그인중 취소에 대한 처리
                    
                    _model.UpdateStep(LoginStep.None);
                }
                else if (task.IsFaulted)
                {
                    // TODO: 로그인중 실패에 대한 처리

                    if (task.Exception != null)
                    {
                        Debug.Log($"Sing In Error : {task.Exception.Message}");
                    }
                    
                    _model.UpdateStep(LoginStep.None);
                }
                else if (task.IsCompleted)
                {
                    _userInfoManager.SetUserInfo(task.Result);
                    _model.UpdateStep(LoginStep.Complete);
                }
            });
        }
    }

}
