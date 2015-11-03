using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System;
using System.Linq;
using System.Threading;

namespace WebApp.Core
{
    public static class IoC
    {
        private static readonly object _syncRoot = new object();

        private static IUnityContainer _containerInstance;

        public static T BuildUp<T>(T existing)
        {
            return Container.BuildUp<T>(existing);
        }

        public static Object BuildUp(Type t, Object existing)
        {
            return Container.BuildUp(t, existing);
        }

        public static T BuildUp<T>(T existing, string name)
        {
            return Container.BuildUp<T>(existing, name);
        }

        public static Object BuildUp(Type t, Object existing, string name)
        {
            return Container.BuildUp(t, existing, name);
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }

        public static Object Resolve(Type t)
        {
            return Container.Resolve(t);
        }

        public static Object Resolve(Type t, string name)
        {
            return Container.Resolve(t, name);
        }

        public static T Resolve<T>(params ResolverOverride[] overrides)
        {
            return Container.Resolve<T>(overrides);
        }

        public static T Resolve<T>(string name, params ResolverOverride[] overrides)
        {
            return Container.Resolve<T>(name, overrides);
        }

        public static object Resolve(Type t, params ResolverOverride[] overrides)
        {
            return Container.Resolve(t, overrides);
        }

        public static T TryResolve<T>()
        {
            return TryResolve<T>(null);
        }

        public static T TryResolve<T>(String name)
        {
            try
            {
                return Container.IsRegistered<T>(name) ? Container.Resolve<T>(name) : default(T);
            }
            catch (Exception __ex)
            {
                return default(T);
            }
        }

        public static void RegisterType<T>(params InjectionMember[] injectionMembers)
        {
            Container.RegisterType<T>(injectionMembers);
        }

        public static void RegisterType<T>(String name, params InjectionMember[] injectionMembers)
        {
            Container.RegisterType<T>(name, injectionMembers);
        }

        public static void RegisterType(Type t, params InjectionMember[] injectionMembers)
        {
            Container.RegisterType(t, injectionMembers);
        }

        public static void RegisterType(Type t, String name, params InjectionMember[] injectionMembers)
        {
            Container.RegisterType(t, name, injectionMembers);
        }

        public static void RegisterInstance<TInterface>(string name, TInterface instance)
        {
            Container.RegisterInstance(name, instance);
        }
        public static void RegisterInstance<TInterface>(TInterface instance)
        {
            Container.RegisterInstance(instance);
        }

        public static void RemoveInstance<TInterface>(String name)
        {
            try
            {
                var __tr = Container.Registrations.First(r => r.RegisteredType == typeof(TInterface) && r.Name == name);
                __tr.LifetimeManager.RemoveValue();
            }
            catch (Exception __ex)
            {
                
            }
        }

        public static void RemoveInstance<TInterface>()
        {
            try
            {
                var __tr = Container.Registrations.First(r => r.RegisteredType == typeof(TInterface) && String.IsNullOrWhiteSpace(r.Name));
                __tr.LifetimeManager.RemoveValue();
            }
            catch (Exception __ex)
            {
                
            }
        }

        public static void CloseContainer()
        {
            if (_containerInstance != null)
            {
                _containerInstance.Dispose();
                _containerInstance = null;
            }
        }

        public static IDictionary<String, Object> Vault
        {
            get
            {
                var __vault = TryResolve<IDictionary<String, Object>>("Vault");
                if (__vault == default(IDictionary<String, Object>))
                {
                    __vault = new ConcurrentDictionary<String, Object>();
                    RegisterInstance("Vault", __vault);
                }
                return __vault;
            }
        }


        public static Type Get(Type type, String name)
        {
            ContainerRegistration __reg = Container.Registrations
                .FirstOrDefault(x => x.RegisteredType == type && x.Name == (name ?? x.Name));

            return __reg != null ? __reg.MappedToType : null;
        }
        public static Type Get(Type type)
        {
            return Get(type, null);
        }

        public static IUnityContainer Container
        {
            get
            {
                if (_containerInstance == null)
                {
                    WaitLock.Lock(_syncRoot, 10000, () =>
                    {
                        if (_containerInstance == null)
                        {
                            _containerInstance = new UnityContainer();
                            try
                            {
                                _containerInstance.LoadConfiguration();
                                var __section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

                                if (__section.Containers.Any())
                                {

                                    if (__section.Containers[0].Registrations.Any())
                                    {
                                        foreach (var __reg in __section.Containers[0].Registrations)
                                        {
                                            
                                        }
                                    }
                                }


                                if (_containerInstance.Registrations.Any())
                                {
                                    foreach (var __reg in _containerInstance.Registrations)
                                    {

                                    }
                                }
                            }
                            catch (FileLoadException __ex)
                            {
                                
                            }
                            catch (Exception __ex)
                            {
                                
                            }
                        }
                    });
                }
                return _containerInstance;
            }
        }

        public static IEnumerable<TIComponent> ResolveAll<TIComponent>(this IUnityContainer container)
            where TIComponent : class
        {
            return container.ResolveAll(typeof(TIComponent)).Cast<TIComponent>();
        }

        public static IEnumerable<TIComponent> ResolveAll<TIComponent>()
            where TIComponent : class
        {
            return Container.ResolveAll<TIComponent>();
        }
        public static IEnumerable<TIComponent> ResolveAllInheritance<TIComponent>(this IUnityContainer container) where TIComponent : class
        {
            List<TIComponent> lst = new List<TIComponent>();
            if (container != null)
            {
                var matches = container.Registrations.Where(r => r.MappedToType.GetInterfaces().Contains(typeof(TIComponent)) == true);

                foreach (var registration in matches)
                {
                    lst.Add(container.Resolve(registration.MappedToType) as TIComponent);
                }
            }
            return lst.AsEnumerable<TIComponent>();
        }

        public static IEnumerable<Type> FromAssembliesInBin(String path, String namePart)
        {
            return FromCheckedAssemblies(GetAssembliesInPath(path, namePart, true, true, true), true);
        }
        private static IEnumerable<Type> FromCheckedAssemblies(IEnumerable<Assembly> assemblies, bool skipOnError)
        {
            return assemblies
                    .SelectMany(a =>
                    {
                        IEnumerable<TypeInfo> types;

                        try
                        {
                            types = a.DefinedTypes;
                        }
                        catch (ReflectionTypeLoadException e)
                        {
                            if (!skipOnError)
                            {
                                throw;
                            }

                            types = e.Types.TakeWhile(t => t != null).Select(t => t.GetTypeInfo());
                        }

                        return types.Where(ti => ti.IsClass & !ti.IsAbstract && !ti.IsValueType && ti.IsVisible).Select(ti => ti.AsType());
                    });
        }
        private static IEnumerable<Assembly> GetAssembliesInPath(string path, string namePart, bool includeSystemAssemblies, bool includeUnityAssemblies, bool skipOnError)
        {

            return GetAssemblyNames(path, namePart, skipOnError)
                    .Select(an => LoadAssembly(Path.GetFileNameWithoutExtension(an), skipOnError))
                    .Where(a => a != null);
        }
        private static Assembly LoadAssembly(string assemblyName, bool skipOnError)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception e)
            {
                if (!(skipOnError && (e is FileNotFoundException || e is FileLoadException || e is BadImageFormatException)))
                {
                    throw;
                }

                return null;
            }
        }
        private static IEnumerable<string> GetAssemblyNames(string path, string namePart = "", bool skipOnError = true)
        {
            try
            {
                return Directory.EnumerateFiles(path, String.Format("{0}*.dll", namePart))
                    .Concat(Directory.EnumerateFiles(path, String.Format("{0}*.exe", namePart)));
            }
            catch (Exception e)
            {
                if (!(skipOnError && (e is DirectoryNotFoundException || e is IOException || e is SecurityException || e is UnauthorizedAccessException)))
                {
                    throw;
                }

                return new string[0];
            }
        }
    }

    public class WaitLock : IDisposable
    {
        private readonly object[] _padlocks;
        private readonly bool[] _securedFlags;

        public WaitLock(int milliSecondTimeout, object padlock)
        {
            _padlocks = new[] { padlock };
            _securedFlags = new[] { Monitor.TryEnter(padlock, milliSecondTimeout) };
        }

        public WaitLock(int milliSecondTimeout, params object[] padlocks)
        {
            _padlocks = padlocks;
            _securedFlags = new bool[_padlocks.Length];
            for (var __i = 0; __i < _padlocks.Length; __i++)
                _securedFlags[__i] = Monitor.TryEnter(padlocks[__i], milliSecondTimeout);
        }
        public WaitLock(params object[] padlocks)
        {
            _padlocks = padlocks;
            _securedFlags = new bool[_padlocks.Length];
            for (var __i = 0; __i < _padlocks.Length; __i++)
                _securedFlags[__i] = Monitor.TryEnter(padlocks[__i], 1000);
        }

        public bool Secured
        {
            get { return _securedFlags.All(s => s); }
        }

        public static void Lock(object[] padlocks, int millisecondTimeout, Action codeToRun)
        {
            using (var __bolt = new WaitLock(millisecondTimeout, padlocks))
                if (__bolt.Secured)
                    codeToRun();
                else
                    throw new TimeoutException(string.Format("Safe.Lock wasn't able to acquire a lock in {0}ms",
                                                             millisecondTimeout));
        }

        public static void Lock(object padlock, int millisecondTimeout, Action codeToRun)
        {
            using (var __bolt = new WaitLock(millisecondTimeout, padlock))
                if (__bolt.Secured)
                    codeToRun();
                else
                    throw new TimeoutException(string.Format("Safe.Lock wasn't able to acquire a lock in {0}ms",
                                                             millisecondTimeout));
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            for (var __i = 0; __i < _securedFlags.Length; __i++)
                if (_securedFlags[__i])
                {
                    Monitor.Exit(_padlocks[__i]);
                    _securedFlags[__i] = false;
                }
        }

        #endregion
    }
}
