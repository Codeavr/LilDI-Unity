using System.Collections.Generic;
using DI;
using DI.Builder;
using DI.Exceptions;
using NUnit.Framework;

namespace Tests
{
    public class BasicTests
    {
        [Test]
        public void BindAndResolve()
        {
            var di = new DiContainer();

            di.Bind<ICollection<int>>().As<LinkedList<int>>().ToScope(Scope.Singleton);

            var collection = di.Resolve<ICollection<int>>();

            collection.Add(1);
        }

        [Test]
        public void ResolveFromParent()
        {
            var parent = new DiContainer();
            parent.Bind<ICollection<int>>().As<LinkedList<int>>().ToScope(Scope.Singleton);

            var child = new DiContainer(parent);

            var collection = child.Resolve<ICollection<int>>();
            collection.Add(1);
        }

        [Test]
        public void ResolveSingletonEquals()
        {
            var di = new DiContainer();
            di.Bind<ICollection<int>>().As<LinkedList<int>>().ToScope(Scope.Singleton);

            var collection1 = di.Resolve<ICollection<int>>();
            var collection2 = di.Resolve<ICollection<int>>();

            Assert.AreEqual(collection1, collection2);
        }

        [Test]
        public void ThrowNotFoundContractResolveException()
        {
            Assert.Catch<BindingNotFoundException>(() => new DiContainer().Resolve<ICollection<int>>());
        }

        [Test]
        public void ThrowDuplicateBindingException()
        {
            var di = new DiContainer();
            di.Bind<ICollection<int>>().As<LinkedList<int>>().ToScope(Scope.Singleton);
            Assert.Catch<DuplicateBindingException>(() =>
                di.Bind<ICollection<int>>().As<LinkedList<int>>().ToScope(Scope.Singleton));
        }
    }
}