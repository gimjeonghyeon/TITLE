using System;
using Playground.Firebase;
using UniRx;
using UnityEngine;
using Zenject;

namespace Playground.Title
{
    public class InitializePresenter : MonoBehaviour
    {
        #region Private

        [Inject] private readonly FirebaseManager _firebaseManager;
        
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
                Debug.Log($"_firebaseManager = {_firebaseManager}");
                
                _disposable = new();
                
                _model.UpdateStep(InitializeStep.FirebaseInitialize);

                _model.Step
                    .Where(step => step == InitializeStep.FirebaseInitialize)
                    .Do(_ => Debug.Log("FirebaseInitialize"))
                    .SelectMany(_ => _firebaseManager.CheckAndFixDependenciesAsObservable())
                    .Subscribe
                    (
                        onNext: _ =>
                        {
                            _model.UpdateStep(InitializeStep.Complete);
                        },
                        onError: exception =>
                        {
                            Debug.Log(exception.Message);
                        }
                    )
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
            }).ObserveOnMainThread();
        }
    }
}