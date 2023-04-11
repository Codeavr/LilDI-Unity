using DI;
using DI.Builder;
using DI.Exceptions;
using NUnit.Framework;

namespace Tests
{
    public class OptionalArgumentsTests
    {
        [Test]
        public void SingleOptionalArgumentInConstructor()
        {
            var container = new DiContainer();

            container
                .BindConcrete<Testables.ConstructorWithOneOptionalArgument>()
                .ToScope(Scope.Singleton);

            var instance = container.Resolve<Testables.ConstructorWithOneOptionalArgument>();

            Assert.NotNull(instance);
            Assert.Null(instance.AFromConstructor);
        }

        [Test]
        public void OneExistsAndOneIsOptionalArgumentsInConstructor()
        {
            var container = new DiContainer();

            container
                .BindConcrete<Testables.A>()
                .ToScope(Scope.Singleton);

            container
                .BindConcrete<Testables.ConstructWithTwoArgumentsWhereOneIsOptional>()
                .ToScope(Scope.Singleton);

            var instance = container.Resolve<Testables.ConstructWithTwoArgumentsWhereOneIsOptional>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.A);
            Assert.Null(instance.B);
        }

        [Test]
        public void ThrowsIfParamsIsNotOptionalAndDoesntExist()
        {
            var container = new DiContainer();

            container
                .BindConcrete<Testables.ConstructWithTwoArgumentsWhereOneIsOptional>()
                .ToScope(Scope.Singleton);

            Assert.Catch<BindingNotFoundException>(() =>
            {
                container.Resolve<Testables.ConstructWithTwoArgumentsWhereOneIsOptional>();
            });
        }
    }
}