using System;

namespace DI
{
    public interface IBinding
    {
        Type ContractType { get; }
        Type ConcreteType { get; }
        Scope Scope { get; }
        object[] Arguments { get; }
    }
}