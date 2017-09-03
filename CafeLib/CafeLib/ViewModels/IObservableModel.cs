using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CafeLib.Services;
using Xamarin.Forms;

namespace CafeLib.ViewModels
{
    public interface IObservableModel<out T> where T : class, new()
    {
        T Deserialize(string data);

        string Serialize();
    }
}
