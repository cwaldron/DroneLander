﻿using System;
using Xamarin.Forms;

namespace CafeLib.Support.Resolution
{
    internal sealed class PageResolver : ResolverBase<Page>
    {
        public PageResolver(Type resolveType)
            : base(resolveType)
        {
        }
    }
}
