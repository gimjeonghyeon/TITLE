using System;
using UniRx;
using UnityEngine;

namespace Title
{
    public class TitlePresenter : MonoBehaviour
    {
        #region Private
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
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => _model.UpdateStep(TitleStep.Login))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Login)
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => _model.UpdateStep(TitleStep.Update))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Update)
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => _model.UpdateStep(TitleStep.Download))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Download)
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => _model.UpdateStep(TitleStep.Complete))
                .AddTo(this);
        
            _model.Step
                .Where(step => step == TitleStep.Complete)
                .Delay(TimeSpan.FromSeconds(_model.DelaySecond))
                .Subscribe(_ => { /* 씬 전환 */ })
                .AddTo(this);
        }
    }
}
