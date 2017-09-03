using System.Threading.Tasks;
using CafeLib.Async;
using CafeLib.Services;
using CafeLib.Support;
using DroneLander.ViewModels;
using Xamarin.Forms;

namespace DroneLander
{
    public class Bootstrapper : SingletonBase<Bootstrapper>, IAsyncInit<Bootstrapper>
    {
        public static object[] Args { get; private set; }

        /// <summary>
        /// Initialize the Navigation Page here.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<NavigationPage> InitApplication(params object[] args)
        {
            // Retain initial arguments.
            Args = args;

            // Initialize services.
            await ((IAsyncInit<Bootstrapper>)Instance).InitAsync();

            // Initialize the root page.
            var pageService = await ServiceProvider.Resolve<IPageService>().InitAsync();
            var rootPage = pageService.ResolvePage(new MainViewModel());
            return new NavigationPage(rootPage);
        }

        /// <summary>
        /// Register services here.
        /// </summary>
        /// <returns>bootstrapper</returns>
        async Task<Bootstrapper> IAsyncInit<Bootstrapper>.InitAsync()
        {
            await ServiceProvider.InitAsync(typeof(Bootstrapper));
            return await Task.FromResult(this);
        }
    }
}
