namespace Playground.Title
{
    public class InitializeModel
    {
        #region Private
        
        private readonly InitializeStepReactiveProperty _step;
        
        #endregion

        #region Public

        public IReadOnlyInitializeStepReactiveProperty Step => _step;

        #endregion
        
        public InitializeModel() => _step = new InitializeStepReactiveProperty(InitializeStep.None);
    
        public void UpdateStep(InitializeStep step) => _step.Value = step;
    }
}