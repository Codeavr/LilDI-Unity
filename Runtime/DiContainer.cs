using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DI.Exceptions;
using DI.Extensions;
using JetBrains.Annotations;

namespace DI
{
    public class DiContainer
    {
        private readonly DiContainer _parent;
        private readonly Dictionary<Type, IBinding> _bindings = new();
        private readonly Dictionary<Type, object> _singletons = new();

        public DiContainer(DiContainer parent = null)
        {
            _parent = parent;
        }

        public void AddBinding(IBinding binding)
        {
            if (_bindings.ContainsKey(binding.ContractType))
                throw new DuplicateBindingException(binding);

            _bindings.Add(binding.ContractType, binding);
        }

        public TContract Resolve<TContract>()
        {
            return (TContract)Resolve(typeof(TContract));
        }

        public object Resolve(Type contractType)
        {
            return InnerResolve(contractType, new Stack<Type>());
        }

        public T CreateInstance<T>(params object[] arguments)
        {
            return (T)CreateInstance(new Stack<Type>(), typeof(T), arguments);
        }

        private object InnerResolve(Type contractType, Stack<Type> stack)
        {
            if (!TryInnerResolve(contractType, stack, out var instance))
            {
                throw new BindingNotFoundException(contractType, stack);
            }

            return instance;
        }

        private bool TryInnerResolve(Type contractType, Stack<Type> stack, out object instance)
        {
            if (_bindings.TryGetValue(contractType, out var binding))
            {
                if (stack.Contains(contractType))
                    throw new CircularDependencyException(contractType, stack);

                stack.Push(contractType);

                instance = binding.Scope switch
                {
                    Scope.Singleton => ResolveSingleton(binding, stack),
                    Scope.Transient => CreateInstance(binding, stack),
                    _ => throw new NotImplementedException()
                };

                if (stack.Pop() != contractType)
                {
                    throw new Exception("Stack is corrupted");
                }

                return true;
            }

            if (_parent != null)
            {
                return _parent.TryInnerResolve(contractType, stack, out instance);
            }

            instance = null;
            return false;
        }

        private object ResolveSingleton(IBinding binding, Stack<Type> stack)
        {
            return ResolveSingleton(stack, binding.ConcreteType, binding.Arguments);
        }

        private object ResolveSingleton(Stack<Type> stack, Type contractType, object[] args)
        {
            if (!_singletons.TryGetValue(contractType, out var instance))
            {
                instance = CreateInstance(stack, contractType, args);
                _singletons.Add(contractType, instance);
            }

            return instance;
        }

        private object CreateInstance(IBinding binding, Stack<Type> stack)
        {
            return CreateInstance(stack, binding.ConcreteType, binding.Arguments);
        }

        private object CreateInstance(Stack<Type> stack, Type targetType, params object[] arguments)
        {
            var injectionConstructor = GetConstructor(targetType);

            var parameters = injectionConstructor
                .GetParameters()
                .Select<ParameterInfo, DependencyInfo>(param => param);

            var instancesOfDependencies = ResolveInstancesOfDependencies(stack, parameters, arguments);

            var instance = injectionConstructor.Invoke(instancesOfDependencies.ToArray());

            foreach (var injectTarget in GetInjectTargets(instance, targetType))
            {
                var dependencies = injectTarget.GetDependencies();
                instancesOfDependencies = ResolveInstancesOfDependencies(stack, dependencies);

                injectTarget.Inject(instancesOfDependencies.ToArray());
            }

            return instance;
        }

        private IEnumerable<IInjectTarget> GetInjectTargets(object instance, IReflect targetType)
        {
            return targetType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttribute<InjectAttribute>() != null)
                .Select(method => new MethodInjectTarget(instance, method));
        }

        private IEnumerable<object> ResolveInstancesOfDependencies
        (
            Stack<Type> stack,
            IEnumerable<DependencyInfo> dependencies,
            IEnumerable<object> existingInstances = null
        )
        {
            var bindingInstances = existingInstances?.ToList() ?? new List<object>();

            foreach (var dependency in dependencies)
            {
                // TODO: optimize to avoid closure
                var index = bindingInstances
                    .FindIndex(0, instance => instance
                        .GetType()
                        .Implements(dependency.Type));

                if (index < 0)
                {
                    if (TryInnerResolve(dependency.Type, stack, out var resolvedInstance))
                    {
                        yield return resolvedInstance;
                    }
                    else if (dependency.IsOptional)
                    {
                        yield return default;
                    }
                    else
                    {
                        throw new BindingNotFoundException(dependency.Type, stack);
                    }
                }
                else
                {
                    var instance = bindingInstances[index];
                    bindingInstances.RemoveAt(index);

                    yield return instance;
                }
            }
        }

        [Pure]
        private static ConstructorInfo GetConstructor(Type targetType)
        {
            var constructors = targetType.GetConstructors();
            if (constructors.Length == 1) return constructors.First();

            var injectConstructor = constructors
                .FirstOrDefault(constructor => constructor
                    .GetCustomAttribute<InjectAttribute>() != null);

            if (injectConstructor == null)
            {
                injectConstructor = constructors
                    .OrderBy(constructor => constructor.GetParameters().Length)
                    .First();
            }

            return injectConstructor;
        }
    }
}