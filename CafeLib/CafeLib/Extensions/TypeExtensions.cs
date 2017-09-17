using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CafeLib.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns the type activator.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="type">type object</param>
        /// <returns>type activator</returns>
        public static Func<T> GetActivator<T>(this Type type)
        {
            return (Func<T>)Expression.Lambda(Expression.New(GetDefaultConstructor(type))).Compile();
        }

        /// <summary>
        /// Returns the type activator.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="type">type object</param>
        /// <returns>type activator</returns>
        public static T CreateInstance<T>(this Type type)
        {
            return GetActivator<T>(type).Invoke();
        }

        /// <summary>
        /// Gets the default constructor.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        ///     Default constructor if found otherwise null
        /// </returns>
        public static ConstructorInfo GetDefaultConstructor(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => !c.GetParameters().Any());
        }

        /// <summary>
        /// Determines anonymous type.
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>true if anonymous; false otherwise</returns>
        public static bool IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetTypeInfo().GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            var isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;
            return isAnonymousType;
        }

        /// <summary>
        /// Default value comparison for value types.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="value">value</param>
        /// <returns>true if equal value type default value otherwise false</returns>
        public static bool IsDefault<T>(this T value) where T : struct
        {
            var isDefault = value.Equals(default(T));
            return isDefault;
        }

        /// <summary>
        /// Gets an assembly creatable type information.
        /// </summary>
        /// <param name="assembly">assembly</param>
        /// <returns>enumerable list of type information</returns>
        public static IEnumerable<TypeInfo> CreatableTypes(this Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Select(t => t)
                .Where(t => !t.IsAbstract)
                .Where(t => t.DeclaredConstructors.Any(c => !c.IsStatic && c.IsPublic))
                .Select(t => t);
        }

        /// <summary>
        /// Get assembly types.
        /// </summary>
        /// <param name="assembly">assembly</param>
        /// <returns>enumerable list of assembly types</returns>
        public static IEnumerable<TypeInfo> GetTypes(this Assembly assembly)
        {
            try
            {
                return assembly.DefinedTypes;
            }
            catch (ReflectionTypeLoadException)
            {
                return new List<TypeInfo>();
            }
        }

        /// <summary>
        /// Get derive types.
        /// </summary>
        /// <param name="types">list of types</param>
        /// <param name="baseType">base type</param>
        /// <returns>enumerable list of derived types</returns>
        public static IEnumerable<TypeInfo> Inherits(this IEnumerable<TypeInfo> types, TypeInfo baseType)
        {
            return types.Where(baseType.IsAssignableFrom);
        }

        /// <summary>
        /// Get derived types
        /// </summary>
        /// <typeparam name="TBase">base type</typeparam>
        /// <param name="types">list of types</param>
        /// <returns>enumerable list of derived types</returns>
        public static IEnumerable<TypeInfo> Inherits<TBase>(this IEnumerable<TypeInfo> types)
        {
            return types.Inherits(typeof(TBase).GetTypeInfo());
        }

        /// <summary>
        /// Find types ending with matching string.
        /// </summary>
        /// <param name="types">list of types</param>
        /// <param name="endsWith">matching string</param>
        /// <returns>enumerable list of types ending with matching string</returns>
        public static IEnumerable<TypeInfo> EndsWith(this IEnumerable<TypeInfo> types, string endsWith)
        {
            return types.Where(x => x.Name.EndsWith(endsWith));
        }
    }
}
