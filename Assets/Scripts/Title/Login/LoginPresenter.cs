using System;
using Cysharp.Threading.Tasks;
using Playground.Firebase;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
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
                            _view.SetButtonActive(false);
                            _view.SetUserInfo(_userInfoManager.DisplayName, _userInfoManager.Email);
                            ImageLoad(_userInfoManager.PhotoUrl, sprite => _view.SetUserPhoto(sprite)).Forget();
                            
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

        /// <summary>
        /// url을 통해 이미지 로드 TODO: 추후 별도 스크립트로 분리
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onComplete"></param>
        private async UniTask ImageLoad(Uri url, Action<Sprite> onComplete)
        {
            var www = UnityWebRequestTexture.GetTexture(url);

            await www.SendWebRequest().AsObservable();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());

                onComplete?.Invoke(sprite);
            }
        }
    }
}
