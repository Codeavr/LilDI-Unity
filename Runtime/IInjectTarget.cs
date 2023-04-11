using System.Collections.Generic;

namespace DI
{
    internal interface IInjectTarget
    {
        IEnumerable<DependencyInfo> GetDependencies();
        void Inject(object[] parameters);
    }
}