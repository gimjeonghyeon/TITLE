using System;
using System.Threading.Tasks;
using Firebase;
using UniRx;
using UnityEngine;

namespace Playground.Title
{
    public class InitializePresenter : MonoBehaviour
    {
        #region Private

        private readonly InitializeModel _model = new ();

        private CompositeDisposable _disposable;

        #endregion

        /// <summary>
        /// 초기화 단계 실행
        /// </summary>
        /// <returns></returns>
        public IObservable<Unit> Execute()
        {
            return Observable.Create<Unit>(observer =>
            {
                _disposable = new();
                
                _model.UpdateStep(InitializeStep.FirebaseInitialize);
                
                _model.Step
                    .Where(step => step == InitializeStep.FirebaseInitialize)
                    .Do(_ => Debug.Log("FirebaseInitialize"))
                    .SelectMany(_ => InitializeFirebase().ToObservable())
                    .Subscribe(_ => {})
                    .AddTo(this);
                
                _model.Step
                    .Where(step => step == InitializeStep.Complete)
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
        /// 파이어베이스 초기화
        /// </summary>
        private async Task InitializeFirebase()
        {
            await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var result = task.Result;
                if (result == DependencyStatus.Available) 
                {
                    _model.UpdateStep(InitializeStep.Complete);
                } 
                else 
                {
                    Debug.Log($"Could not resolve all Firebase dependencies: {result}");
                }
            });
        }
    }
}