using DI;
using DI.Builder;
using DI.Exceptions;
using NUnit.Framework;

namespace Tests
{
    public class DependencyTests
    {
        [Test]
        public void DependencyTest()
        {
            var diContainer = new DiContainer();
            diContainer.BindConcrete<Testables.A>().ToScope(Scope.Singleton);
            diContainer.BindConcrete<Testables.B>().ToScope(Scope.Singleton);

            var a = diContainer.Resolve<Testables.A>();
            Assert.NotNull(a);

            var b = diContainer.Resolve<Testables.B>();
            Assert.NotNull(b);

            Assert.IsTrue(ReferenceEquals(a, b.A));
        }

        [Test]
        public void CircularDependencyTest()
        {
            var diContainer = new DiContainer();
            diContainer.BindConcrete<Testables.Fizz>().ToScope(Scope.Singleton);
            diContainer.BindConcrete<Testables.Buzz>().ToScope(Scope.Singleton);

            Assert.Catch<CircularDependencyException>(() => diContainer.Resolve<Testables.Fizz>());
        }

        [Test]
        public void CircularDependencyOnSelfTest()
        {
            var diContainer = new DiContainer();
            diContainer.BindConcrete<Testables.CircularOnSelf>().ToScope(Scope.Singleton);

            Assert.Catch<CircularDependencyException>(() => diContainer.Resolve<Testables.CircularOnSelf>());
        }
    }
}