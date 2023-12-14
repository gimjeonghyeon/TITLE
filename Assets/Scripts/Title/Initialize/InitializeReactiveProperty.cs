using UniRx;

namespace Playground.Title
{
    public enum InitializeStep
    {
        None,
        FirebaseInitialize,
        Complete
    }

    public interface IReadOnlyInitializeStepReactiveProperty : IReadOnlyReactiveProperty<InitializeStep> { }

    public class InitializeStepReactiveProperty : ReactiveProperty<InitializeStep>, IReadOnlyInitializeStepReactiveProperty
    {
        public InitializeStepReactiveProperty() { }
        public InitializeStepReactiveProperty(InitializeStep initialValue) : base(initialValue) { }
    }
}