using System;
using UniRx;
using UnityEngine;

namespace Playground.Title
{
    public class TitlePresenter : MonoBehaviour
    {
        #region Private

        [SerializeField] private InitializePresenter _initializePresenter;
        [SerializeField] private LoginPresenter _loginPresenter;
        
        private readonly TitleModel _model = new();
        
        private TitleView _view;
        
        #endregion

        private void Awake() => _view = GetComponent<TitleView>();

        private void Start()
        {
            _model.UpdateStep(TitleStep.Initialize);

            _model.Step
                .Subscribe(step => _view.UpdateProgressText(step.ToString()))
                .AddTo(this);
            
            _model.Step
                .Where(step => step == TitleStep.Initialize)
                .Do(_ => Debug.Log("Initialize"))
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .SelectMany(_ => _initializePresenter.Execute())
                .Subscribe(_ => _model.UpdateStep(TitleStep.Login))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Login)
                .Do(_ => Debug.Log("Login"))
                .SelectMany(_ => _loginPresenter.Execute())
                .Subscribe(_ => _model.UpdateStep(TitleStep.Update))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Update)
                .Do(_ => Debug.Log("Update"))
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => _model.UpdateStep(TitleStep.Download))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Download)
                .Do(_ => Debug.Log("Download"))
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => _model.UpdateStep(TitleStep.Complete))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Complete)
                .Do(_ => Debug.Log("Complete"))
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => { /* 씬 전환 */ })
                .AddTo(this);
        }
    }
}
