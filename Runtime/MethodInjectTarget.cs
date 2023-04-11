using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DI
{
    internal class MethodInjectTarget : IInjectTarget
    {
        private readonly object _instance;
        private readonly MethodBase _method;

        public MethodInjectTarget(object instance, MethodBase method)
        {
            _instance = instance;
            _method = method;
        }

        public IEnumerable<DependencyInfo> GetDependencies() => _method
            .GetParameters()
            .Select<ParameterInfo, DependencyInfo>(param => param);

        public void Inject(object[] parameters)
        {
            _method.Invoke(_instance, parameters);
        }
    }
}