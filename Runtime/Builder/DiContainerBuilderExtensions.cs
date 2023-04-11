namespace DI.Builder
{
    public static class DiContainerBuilderExtensions
    {
        public static Contract<TContract> Bind<TContract>(this DiContainer diContainer)
        {
            return new Contract<TContract>(diContainer);
        }

        public static UnscopedBinding<TConcrete, TConcrete> BindConcrete<TConcrete>(this DiContainer diContainer)
            where TConcrete : class
        {
            return diContainer.Bind<TConcrete>().As<TConcrete>();
        }

        public static Binding<TContract, TConcrete> ToScope<TContract, TConcrete>
        (
            this UnscopedBinding<TContract, TConcrete> binding,
            Scope scope
        ) where TConcrete : TContract
        {
            return new Binding<TContract, TConcrete>(binding.DiContainer, scope, binding.Arguments);
        }

        public static UnscopedBinding<TContract, TConcrete> WithArguments<TContract, TConcrete>
        (
            this UnscopedBinding<TContract, TConcrete> binding,
            params object[] arguments
        ) where TConcrete : TContract
        {
            binding.Arguments = arguments;
            return binding;
        }
    }
}