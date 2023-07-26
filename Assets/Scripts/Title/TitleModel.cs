using UnityEngine;

namespace Title
{
    public class TitleModel : MonoBehaviour
    {
        #region Const
        private const int DELAY_SECOND = 1;
        #endregion
        
        #region Private
        private readonly TitleStepReactiveProperty _step;
        #endregion

        #region Public
        public int DelaySecond => DELAY_SECOND;
        public IReadOnlyTitleStepReactiveProperty Step => _step;
        #endregion
    
        public TitleModel() => _step = new TitleStepReactiveProperty(TitleStep.None);
    
        public void UpdateStep(TitleStep step) => _step.Value = step;
    }
}
