using System;
using System.Threading.Tasks;
using CafeLib.Async;
using CafeLib.ViewModels;
using Xamarin.Forms;

namespace CafeLib.Services
{
    public interface IPageService : IServiceProvider, IAsyncInit<IPageService>
    {
        /// <summary>
        /// Binds view model to associated page.
        /// </summary>
        /// <typeparam name="T">BaseViewModel type</typeparam>
        /// <returns>page</returns>
        Page BindViewModel<T>(T viewModel) where T : BaseViewModel;

        /// <summary>
        /// Resolves the page for the view model
        /// </summary>
        /// <typeparam name="T">BaseViewModel</typeparam>
        /// <returns>page</returns>
        Page ResolvePage<T>(T viewModel) where T : BaseViewModel;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ok"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);
    }
}
