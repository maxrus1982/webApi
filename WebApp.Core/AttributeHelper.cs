﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApp.Core
{
    public static class AttributeHelper
    {
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            object[] __attrs = type.GetCustomAttributes(typeof(T), true);

            if (__attrs.Any())
            {
                return (T)__attrs[0];
            }
            return null;
        }

        public static IEnumerable<T> GetAttributes<T>(this Type type) where T : Attribute
        {
            object[] __attrs = type.GetCustomAttributes(typeof(T), true);

            if (__attrs.Any())
            {
                return (IEnumerable<T>)__attrs.AsEnumerable();
            }
            return null;
        }

        public static T GetPropertyAttribute<T>(this PropertyInfo info) where T : Attribute
        {
            object[] __attrs = info.GetCustomAttributes(typeof(T), true);
            if (__attrs.Any())
            {
                return (T)__attrs[0];
            }            
            return null;
        }

        public static Boolean HasAttribute<T>(this PropertyInfo info) where T : Attribute
        {
            object[] __attrs = info.GetCustomAttributes(typeof(T), true);
            return (__attrs.Any());
        }

        public static Boolean HasTypeAttribute<T>(this Type type) where T : Attribute
        {
            object[] __attrs = type.GetCustomAttributes(typeof(T), true);
            return (__attrs.Any());
        }

        public static T GetMethodAttribute<T>(this MethodInfo info) where T : Attribute
        {
            object[] __attrs = info.GetCustomAttributes(typeof(T), true);
            if (__attrs.Any())
            {
                return (T)__attrs[0];
            }            
            return null;
        }

        public static T[] GetMethodAttributes<T>(this MethodInfo info) where T : Attribute
        {
            object[] __attrs = info.GetCustomAttributes(typeof(T), true);
            if (__attrs.Any())
            {
                return (T[])__attrs;
            }

            return null;
        }

        public static Boolean HasAttribute<T>(this MethodInfo info) where T : Attribute
        {
            object[] __attrs = info.GetCustomAttributes(typeof(T), true);
            return (__attrs.Any());
        }
        
        public static IList<Type> GetTypes<T>(String assemblyName) where T : Attribute
        {
            var __assm = Assembly.LoadFrom(assemblyName);
            return GetTypes<T>(__assm);
        }

        public static IList<Type> GetTypes<T>(this Assembly assembly) where T : Attribute
        {
            var __list = new List<Type>();

            if (assembly != null)
            {
                __list.AddRange(assembly.GetTypes().Where(type => type.HasTypeAttribute<T>()));

                assembly.GetTypes().Where(t => t.HasTypeAttribute<T>()).ToList().ForEach(__list.Add);
            }

            return __list;
        }
    }
}
