using DI;
using DI.Builder;
using NUnit.Framework;

namespace Tests
{
    public class InjectConstructorTests
    {
        [Test]
        public void MultipleConstructorsWithoutInjectShouldSucceed()
        {
            var container = new DiContainer();

            container.BindConcrete<Testables.A>().ToScope(Scope.Singleton);
            container.BindConcrete<Testables.TwoConstructors>().ToScope(Scope.Singleton);

            var instance = container.Resolve<Testables.TwoConstructors>();

            Assert.NotNull(instance);
            Assert.Null(instance.AFromConstructor);
        }

        [Test]
        public void MultipleConstructorsAndOneWithInjectShouldSucceed()
        {
            var container = new DiContainer();

            container.BindConcrete<Testables.A>().ToScope(Scope.Singleton);
            container.BindConcrete<Testables.TwoConstructorsWithInject>().ToScope(Scope.Singleton);

            var instance = container.Resolve<Testables.TwoConstructorsWithInject>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.AFromConstructor);
        }
    }
}