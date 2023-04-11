namespace DI.Builder
{
    public class Contract<T>
    {
        public DiContainer DiContainer { get; }

        public Contract(DiContainer diContainer)
        {
            DiContainer = diContainer;
        }

        public UnscopedBinding<T, TConcrete> As<TConcrete>() where TConcrete : T
        {
            return new UnscopedBinding<T, TConcrete>(DiContainer);
        }
    }
}