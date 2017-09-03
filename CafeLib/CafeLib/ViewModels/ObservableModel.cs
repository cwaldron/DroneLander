using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CafeLib.Services;
using Xamarin.Forms;

namespace CafeLib.ViewModels
{
    public abstract class ObservableModel<T> : ObservableBase, IObservableModel<T> where T : class, new()
    {
        public abstract T Deserialize(string data);

        public abstract string Serialize();
    }
}
