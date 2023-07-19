using UniRx;

namespace Title
{
    public enum TitleStep
    {
        None,
        Initialize,
        Login,
        Update,
        Download,
        Complete
    }

    public interface IReadOnlyTitleStepReactiveProperty : IReadOnlyReactiveProperty<TitleStep> { }
    
    public class TitleStepReactiveProperty : ReactiveProperty<TitleStep>, IReadOnlyTitleStepReactiveProperty
    {
        public TitleStepReactiveProperty() { }
        public TitleStepReactiveProperty(TitleStep initialValue) : base(initialValue) { }
    }
}