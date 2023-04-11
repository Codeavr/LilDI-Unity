using System;
using System.Reflection;

namespace DI
{
    internal struct DependencyInfo
    {
        public Type Type;
        public bool IsOptional;

        public static implicit operator DependencyInfo(ParameterInfo parameterInfo) =>
            new()
            {
                Type = parameterInfo.ParameterType,
                IsOptional = parameterInfo.IsOptional
            };
    }
}