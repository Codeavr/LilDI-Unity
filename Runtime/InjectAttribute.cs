using System;

namespace DI
{
    [JetBrains.Annotations.MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
    }
}