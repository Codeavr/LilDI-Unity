using System;

namespace DI.Builder
{
    public class Binding<TContract, TConcrete> : IBinding where TConcrete : TContract
    {
        public Type ContractType => typeof(TContract);
        public Type ConcreteType => typeof(TConcrete);
        public Scope Scope => _scope;
        public object[] Arguments => _arguments;

        private readonly Scope _scope;
        private readonly object[] _arguments;

        public Binding(DiContainer diContainer, Scope scope, params object[] arguments)
        {
            _scope = scope;
            _arguments = arguments;

            diContainer.AddBinding(this);
        }
    }
}