using System;
using CafeLib.ViewModels;

namespace CafeLib.Support.Resolution
{
    internal sealed class ViewModelResolver : ResolverBase<BaseViewModel>
    {
        public ViewModelResolver(Type resolveType)
            : base(resolveType)
        {
        }
    }
}
