using DI;
using DI.Builder;
using DI.Exceptions;
using NUnit.Framework;

namespace Tests
{
    public class MethodInjectionTests
    {
        [Test]
        public void InjectConstructorAndMethodTest()
        {
            var di = new DiContainer();

            di
                .BindConcrete<Testables.A>()
                .ToScope(Scope.Singleton);

            di
                .BindConcrete<Testables.B>()
                .ToScope(Scope.Singleton);

            di
                .BindConcrete<Testables.MethodWithInjectAttribute>()
                .ToScope(Scope.Singleton);

            var instance = di.Resolve<Testables.MethodWithInjectAttribute>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.A);
            Assert.NotNull(instance.B);
        }

        [Test]
        public void InjectConstructorButFailOnMethod()
        {
            var di = new DiContainer();

            di.BindConcrete<Testables.A>().ToScope(Scope.Singleton);
            di.BindConcrete<Testables.MethodWithInjectAttribute>().ToScope(Scope.Singleton);

            Assert.Catch<BindingNotFoundException>(() => di.Resolve<Testables.MethodWithInjectAttribute>());
        }
    }
}