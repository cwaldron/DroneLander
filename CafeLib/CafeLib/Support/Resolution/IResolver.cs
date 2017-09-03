using System;

namespace CafeLib.Support.Resolution
{
    public interface IResolver<in T> where T : class
    {
        Type GetResolveType();

        TU Resolve<TU>() where TU : T;

        object Resolve(Type type);
    }
}
