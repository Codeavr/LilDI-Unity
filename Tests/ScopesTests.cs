using DI;
using DI.Builder;
using NUnit.Framework;

namespace Tests
{
    public class ScopesTests
    {
        [Test]
        public void SingletonShouldBeTheSame()
        {
            var di = new DiContainer();
            di.BindConcrete<Testables.A>().ToScope(Scope.Singleton);

            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(ReferenceEquals(di.Resolve<Testables.A>(), di.Resolve<Testables.A>()));
        }

        [Test]
        public void TransientShouldBeDifferent()
        {
            var di = new DiContainer();
            di.BindConcrete<Testables.A>().ToScope(Scope.Transient);

            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(ReferenceEquals(di.Resolve<Testables.A>(), di.Resolve<Testables.A>()));
        }

        [Test]
        public void ComplexSingletonShouldBeTheSame()
        {
            var di = new DiContainer();
            di.BindConcrete<Testables.A>().ToScope(Scope.Singleton);
            di.BindConcrete<Testables.B>().ToScope(Scope.Singleton);
            di.BindConcrete<Testables.C>().ToScope(Scope.Singleton);
            di.BindConcrete<Testables.D>().ToScope(Scope.Singleton);

            Assert.IsTrue(ReferenceEquals(di.Resolve<Testables.D>().A, di.Resolve<Testables.A>()));
        }

        [Test]
        public void ComplexTransientShouldBeDifferent()
        {
            var di = new DiContainer();
            di.BindConcrete<Testables.A>().ToScope(Scope.Transient);
            di.BindConcrete<Testables.B>().ToScope(Scope.Transient);
            di.BindConcrete<Testables.C>().ToScope(Scope.Transient);
            di.BindConcrete<Testables.D>().ToScope(Scope.Transient);

            var a = di.Resolve<Testables.A>();
            var b = di.Resolve<Testables.B>();
            var c = di.Resolve<Testables.C>();
            var d = di.Resolve<Testables.D>();

            Assert.IsFalse(ReferenceEquals(a, b.A));
            Assert.IsFalse(ReferenceEquals(a, c.A));
            Assert.IsFalse(ReferenceEquals(a, d.A));

            Assert.IsFalse(ReferenceEquals(b, c.B));
            Assert.IsFalse(ReferenceEquals(b, d.B));
        }
    }
}