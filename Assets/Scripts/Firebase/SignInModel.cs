namespace Playground.Firebase
{
    public class SignInModel
    {
        #region Const

        private const string WEB_CLIENT_ID = "994449946165-1o83tu2g77ktko179i9kbpshbdi1fje5.apps.googleusercontent.com";

        #endregion

        #region Private
        
        private SignInStepReactiveProperty _signInStep;
        
        #endregion
        
        #region Public

        public IReadOnlySignInStepReactiveProperty SignInStep => _signInStep;
        public string WebClientId => WEB_CLIENT_ID;
        public string GoogleIdToken { get; set; }

        #endregion

        public SignInModel() => _signInStep = new SignInStepReactiveProperty(Firebase.SignInStep.NONE);
        
        public void UpdateSignInStep(SignInStep step) => _signInStep.Value = step;
    }
}