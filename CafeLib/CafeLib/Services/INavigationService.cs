using System;
using System.Threading.Tasks;
using Xamarin.Forms;

using CafeLib.ViewModels;

namespace CafeLib.Services
{
    public interface INavigationService : IServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task<bool> PushAsync(BaseViewModel viewModel, bool animate=false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task PushAsync(Page page, bool animate=false);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="animate"></param>
        /// <returns></returns>
        Task<Page> PopAsync(bool animate=false);
    }
}
