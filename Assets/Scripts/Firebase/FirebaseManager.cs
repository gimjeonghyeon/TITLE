using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Google;
using UniRx;

namespace Playground.Firebase
{
    public class FirebaseManager
    {
        #region Private
        
        private readonly SignInModel _signInModel = new();
        
        private FirebaseApp _app;
        private FirebaseAuth _auth;
        private GoogleSignInConfiguration _configuration;
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
                _configuration = new()
                {
                    WebClientId = _signInModel.WebClientId, 
                    RequestIdToken = true
                };
                
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
        public IObservable<FirebaseUser> GoogleSignInAsObservable()
        {
            return Observable.Create<FirebaseUser>(observer =>
            {
                _disposable = new();
                
                _signInModel.UpdateSignInStep(SignInStep.GOOGLE_SIGN_IN);

                _signInModel.SignInStep
                    .Where(step => step == SignInStep.GOOGLE_SIGN_IN)
                    .Subscribe(_ =>
                    {
                        GoogleSignIn.Configuration = _configuration;
                        GoogleSignIn.Configuration.UseGameSignIn = false;
                        GoogleSignIn.Configuration.RequestIdToken = true;

                        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                string message = null;

                                if (task.Exception != null)
                                {
                                    using IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator();

                                    if (enumerator.MoveNext())
                                    {
                                        var error = (GoogleSignIn.SignInException)enumerator.Current;

                                        if (error != null)
                                        {
                                            message += $"Get Error: {error.Status} {error.Message}\n";
                                        }
                                    }
                                    else
                                    {
                                        message += $"Got Unexpected Exception: {task.Exception}";
                                    }
                                }

                                observer.OnError(new Exception(message ?? "Sign In Error"));
                            }
                            else if (task.IsCanceled)
                            {
                                observer.OnError(new Exception("Sign In Cancel"));
                            }
                            else if (task.IsCompleted)
                            {
                                _signInModel.GoogleIdToken = task.Result.IdToken;
                                _signInModel.UpdateSignInStep(SignInStep.GOOGLE_SIGN_IN_WITH_CREDENTIAL);
                            }
                        });
                    })
                    .AddTo(_disposable);
                

                _signInModel.SignInStep
                    .Where(step => step == SignInStep.GOOGLE_SIGN_IN_WITH_CREDENTIAL)
                    .Subscribe(_ =>
                    {
                        var credential = GoogleAuthProvider.GetCredential(_signInModel.GoogleIdToken, null);

                        _auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                        {
                            if (task.IsCanceled || task.IsFaulted)
                            {
                                observer.OnError(task.Exception?? new Exception());
                            }
                            else if (task.IsCompleted)
                            {
                                observer.OnNext(task.Result);
                            }
                        });
                    })
                    .AddTo(_disposable);

                _signInModel.SignInStep
                    .Where(step => step == SignInStep.COMPLETE)
                    .First()
                    .Subscribe(_ =>
                    {
                        observer.OnCompleted();
                    });

                return Disposable.Create(() => _disposable?.Dispose());
            }).ObserveOnMainThread();
        }
    }
}