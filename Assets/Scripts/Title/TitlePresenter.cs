using System;
using Title;
using UniRx;
using UnityEngine;

public class TitlePresenter : MonoBehaviour
{
    #region Private
    private TitleModel _model = new();
    #endregion

    private void Start()
    {
        _model.UpdateStep(TitleStep.Initialize);

        _model.Step
            .Where(step => step == TitleStep.Initialize)
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(3)))
            .Subscribe(_ => _model.UpdateStep(TitleStep.Login))
            .AddTo(this);
        
        _model.Step
            .Where(step => step == TitleStep.Login)
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(3)))
            .Subscribe(_ => _model.UpdateStep(TitleStep.Update))
            .AddTo(this);
        
        _model.Step
            .Where(step => step == TitleStep.Update)
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(3)))
            .Subscribe(_ => _model.UpdateStep(TitleStep.Download))
            .AddTo(this);
        
        _model.Step
            .Where(step => step == TitleStep.Download)
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(3)))
            .Subscribe(_ => _model.UpdateStep(TitleStep.Complete))
            .AddTo(this);
        
        _model.Step
            .Where(step => step == TitleStep.Complete)
            .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(3)))
            .Subscribe(_ => { /* 씬 전환 */ })
            .AddTo(this);

        _model.Step
            .Subscribe(step => Debug.Log(step))
            .AddTo(this);
    }
}
