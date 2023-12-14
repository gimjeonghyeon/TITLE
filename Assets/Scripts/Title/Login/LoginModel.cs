namespace Playground.Title
{
    public class LoginModel
    {
        #region Const

        private const string GOOGLE_LOGIN_BUTTON_TEXT = "Google Login";
        private const string GOOGLE_ID_TOKEN = "994449946165-1o83tu2g77ktko179i9kbpshbdi1fje5.apps.googleusercontent.com";
        private const string GOOGLE_ACCESS_TOKEN = "";

        #endregion
        
        #region Private
        
        private readonly LoginStepReactiveProperty _step;
        
        #endregion

        #region Public

        public string GoogleLoginButtonText => GOOGLE_LOGIN_BUTTON_TEXT;
        public string GoogleIdToken => GOOGLE_ID_TOKEN;
        public string GoogleAccessToken => GOOGLE_ACCESS_TOKEN;

        public IReadOnlyLoginStepReactiveProperty Step => _step;

        #endregion
        
        public LoginModel() => _step = new LoginStepReactiveProperty(LoginStep.None);
    
        public void UpdateStep(LoginStep step) => _step.Value = step;
    }
}
