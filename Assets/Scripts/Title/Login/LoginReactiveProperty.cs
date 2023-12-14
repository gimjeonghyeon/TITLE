using UniRx;

namespace Playground.Title
{
    public enum LoginStep
    {
        None,
        SignIn,
        Complete
    }

    public interface IReadOnlyLoginStepReactiveProperty : IReadOnlyReactiveProperty<LoginStep> { }

    public class LoginStepReactiveProperty : ReactiveProperty<LoginStep>, IReadOnlyLoginStepReactiveProperty
    {
        public LoginStepReactiveProperty() { }
        public LoginStepReactiveProperty(LoginStep initialValue) : base(initialValue) { }
    }
}