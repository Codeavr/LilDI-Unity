using System;
using System.Collections.Generic;
using System.Linq;

namespace DI.Exceptions
{
    internal static class ExceptionHelper
    {
        internal static string StackToString(IEnumerable<Type> stack)
        {
            return string.Join("\n", stack.Select(type => $" -> {type}"));
        }
    }

    public class CircularDependencyException : Exception
    {
        internal CircularDependencyException(Type contractType, Stack<Type> stack) : base
        (
            $"Circular dependency detected for {contractType}. Stack:\n{ExceptionHelper.StackToString(stack)}"
        )
        {
        }
    }

    public class BindingNotFoundException : Exception
    {
        internal BindingNotFoundException(Type contractType, Stack<Type> stack) : base
        (
            $"No binding found for {contractType}. Stack:\n{ExceptionHelper.StackToString(stack)}"
        )
        {
        }
    }
}