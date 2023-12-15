using System;
using Firebase;
using Firebase.Auth;
using UniRx;

namespace Playground.Firebase
{
    public class FirebaseManager
    {
        #region Private

        private FirebaseApp _app;
        private FirebaseAuth _auth;
        private CompositeDisposable _disposable;

        #endregion

        /// <summary>
        /// 파이어베이스 초기화
        /// 파이어베이스에 필요한 종속성이 있는지 확인하고 누락된 종속성이 있을 경우 수정하려고 시도
        /// </summary>
        public IObservable<Unit> CheckAndFixDependenciesAsObservable()
        {
            return Observable.Create<Unit>(observer =>
            {
                _disposable = new();
                
                _app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    var result = task.Result;
                    if (result == DependencyStatus.Available) 
                    {
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                    } 
                    else 
                    {
                        observer.OnError(new Exception($"Could not resolve all Firebase dependencies: {result}"));
                    }
                });
                
                return Disposable.Create(() => _disposable?.Dispose());
            }).ObserveOnMainThread();
        }

        /// <summary>
        /// 구글 로그인
        /// </summary>
        public IObservable<FirebaseUser> GoogleSignInAsObservable(string googleIdToken, string googleAccessToken)
        {
            return Observable.Create<FirebaseUser>(observer =>
            {
                _disposable = new();
                
                var credential = GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
                
                _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        observer.OnError(task.Exception?? new Exception());
                    }
                    else if (task.IsCompleted)
                    {
                        observer.OnNext(task.Result);
                        observer.OnCompleted();
                    }
                });

                return Disposable.Create(() => _disposable?.Dispose());
            }).ObserveOnMainThread();
        }
    }
}