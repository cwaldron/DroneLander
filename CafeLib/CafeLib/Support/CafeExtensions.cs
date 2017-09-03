using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CafeLib.ViewModels;
using Xamarin.Forms;

namespace CafeLib.Support
{
    internal static class CafeExtensions
    {
        public static IEnumerable<TypeInfo> CreatableTypes(this Assembly assembly)
        {
            return assembly
                .ExceptionSafeGetTypes()
                .Select(t => t)
                .Where(t => !t.IsAbstract)
                .Where(t => t.DeclaredConstructors.Any(c => !c.IsStatic && c.IsPublic))
                .Select(t => t);
        }

        public static IEnumerable<TypeInfo> ExceptionSafeGetTypes(this Assembly assembly)
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

        public static IEnumerable<TypeInfo> Inherits(this IEnumerable<TypeInfo> types, TypeInfo baseType)
        {
            return types.Where(baseType.IsAssignableFrom);
        }

        public static IEnumerable<TypeInfo> Inherits<TBase>(this IEnumerable<TypeInfo> types)
        {
            return types.Inherits(typeof(TBase).GetTypeInfo());
        }

        public static IEnumerable<TypeInfo> EndsWith(this IEnumerable<TypeInfo> types, string endsWith)
        {
            return types.Where(x => x.Name.EndsWith(endsWith));
        }

        public static T GetViewModel<T>(this Page view) where T : BaseViewModel
        {
            return view.BindingContext as T;
        }

        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
