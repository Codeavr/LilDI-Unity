using DI;
using DI.Builder;
using NUnit.Framework;

namespace Tests
{
    public class ResolvingWithArgumentsTests
    {
        [Test]
        public void ResolveWithArguments()
        {
            var di = new DiContainer();
            var a = new Testables.A();

            di.BindConcrete<Testables.B>().WithArguments(a).ToScope(Scope.Singleton);

            var b = di.Resolve<Testables.B>();
            
            Assert.NotNull(b);
            Assert.NotNull(b.A);
            Assert.AreEqual(a, b.A);
        }
    }
}