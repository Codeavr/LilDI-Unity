namespace DI.Builder
{
    public class UnscopedBinding<TContract, TConcrete> where TConcrete : TContract
    {
        public DiContainer DiContainer { get; }
        internal object[] Arguments { get; set; }

        public UnscopedBinding(DiContainer diContainer, params object[] arguments)
        {
            DiContainer = diContainer;
            Arguments = arguments;
        }
    }
}