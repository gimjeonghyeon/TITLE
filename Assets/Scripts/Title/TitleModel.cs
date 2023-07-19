using Title;
using UnityEngine;

public class TitleModel : MonoBehaviour
{
    #region Private

    private readonly TitleStepReactiveProperty _step;
    #endregion

    #region Public
    public IReadOnlyTitleStepReactiveProperty Step => _step;
    #endregion
    
    public TitleModel() => _step = new TitleStepReactiveProperty(TitleStep.None);
    
    public void UpdateStep(TitleStep step) => _step.Value = step;
}
