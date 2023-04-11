using System;

namespace DI.Exceptions
{
    public class DuplicateBindingException : Exception
    {
        public DuplicateBindingException(IBinding binding) : base($"Duplicate binding for {binding.ContractType}")
        {
        }
    }
}