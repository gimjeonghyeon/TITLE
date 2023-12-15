using UniRx;

namespace Playground.Firebase
{
    public enum SignInStep
    {
        NONE,
        GOOGLE_SIGN_IN,
        GOOGLE_SIGN_IN_WITH_CREDENTIAL,
        COMPLETE
    }

    public interface IReadOnlySignInStepReactiveProperty : IReadOnlyReactiveProperty<SignInStep> { }
    
    public class SignInStepReactiveProperty : ReactiveProperty<SignInStep>, IReadOnlySignInStepReactiveProperty
    {
        public SignInStepReactiveProperty() { }
        public SignInStepReactiveProperty(SignInStep initialValue) : base(initialValue) { }
    }
}