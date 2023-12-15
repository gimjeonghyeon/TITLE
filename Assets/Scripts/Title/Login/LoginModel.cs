namespace Playground.Title
{
    public class LoginModel
    {
        #region Const

        private const string GOOGLE_LOGIN_BUTTON_TEXT = "Google Login";

        #endregion
        
        #region Private
        
        private readonly LoginStepReactiveProperty _step;
        
        #endregion

        #region Public

        public string GoogleLoginButtonText => GOOGLE_LOGIN_BUTTON_TEXT;
        public IReadOnlyLoginStepReactiveProperty Step => _step;

        #endregion
        
        public LoginModel() => _step = new LoginStepReactiveProperty(LoginStep.None);
    
        public void UpdateStep(LoginStep step) => _step.Value = step;
    }
}
