using DI;
using DI.Builder;
using NUnit.Framework;

namespace Tests
{
    public class FunnyTests
    {
        [Test]
        public void UnitGetsExcalibur()
        {
            var diContainer = new DiContainer();

            diContainer
                .Bind<Testables.IWeapon>()
                .As<Testables.Sword>()
                .ToScope(Scope.Singleton);

            var unit = diContainer.CreateInstance<Testables.Unit>();

            Assert.AreEqual("Excalibur", unit.Weapon.Name);
        }
    }
}