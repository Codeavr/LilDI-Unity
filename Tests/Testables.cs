using DI;

namespace Tests
{
    public class Testables
    {
        public class A
        {
        }

        public class B
        {
            public A A { get; }

            public B(A a)
            {
                A = a;
            }
        }

        public class C
        {
            public A A { get; }
            public B B { get; }

            public C(A a, B b)
            {
                A = a;
                B = b;
            }
        }

        public class D
        {
            public A A { get; }
            public B B { get; }

            public D(A a, B b)
            {
                A = a;
                B = b;
            }
        }


        public class Fizz
        {
            public Fizz(Buzz buzz)
            {
            }
        }

        public class Buzz
        {
            public Buzz(Fizz fizz)
            {
            }
        }

        public class TwoConstructors
        {
            public readonly A AFromConstructor;

            public TwoConstructors()
            {
            }

            public TwoConstructors(A aFromConstructor)
            {
                AFromConstructor = aFromConstructor;
            }
        }

        public class TwoConstructorsWithInject
        {
            public readonly A AFromConstructor;

            public TwoConstructorsWithInject()
            {
            }

            [Inject]
            public TwoConstructorsWithInject(A aFromConstructor)
            {
                AFromConstructor = aFromConstructor;
            }
        }

        public class ConstructorWithOneOptionalArgument
        {
            public readonly A AFromConstructor;

            public ConstructorWithOneOptionalArgument(A aFromConstructor = null)
            {
                AFromConstructor = aFromConstructor;
            }
        }

        public class ConstructWithTwoArgumentsWhereOneIsOptional
        {
            public readonly A A;
            public readonly B B;

            public ConstructWithTwoArgumentsWhereOneIsOptional(A a, B b = null)
            {
                A = a;
                B = b;
            }
        }

        public class MethodWithInjectAttribute
        {
            public A A;
            public B B;

            public MethodWithInjectAttribute(A a)
            {
                A = a;
            }

            [Inject]
            public void AddB(B b)
            {
                B = b;
            }
        }
        
        public class CircularOnSelf
        {
            public CircularOnSelf(CircularOnSelf circularOnSelf)
            {
            }
        }

        public interface IWeapon
        {
            string Name { get; }
        }

        public class Sword : IWeapon
        {
            public string Name => "Excalibur";
        }

        public class Unit
        {
            public IWeapon Weapon { get; }

            public Unit(IWeapon weapon)
            {
                Weapon = weapon;
            }
        }
    }
}