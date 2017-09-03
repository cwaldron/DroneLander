using System;
using System.Threading.Tasks;
using CafeLib.Services;
using Xamarin.Forms;

namespace CafeLib.ViewModels
{
    public abstract class BaseViewModel : ObservableBase
    {
        #region Properties

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        protected INavigationService Navigation => ServiceProvider.Resolve<INavigationService>();

        #endregion

        #region Methods

        /// <summary>
        /// Displays an alert on the page.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="ok">OK</param>
        public void DisplayAlert(string title, string message, string ok = "OK")
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, message, ok);
            });
        }

        /// <summary>
        /// Displays an alert on the page.
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="message">message</param>
        /// <param name="ok">ok text</param>
        /// <param name="cancel">cancellation text</param>
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            var complete = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var answer = await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
                complete.SetResult(answer);
            });

            return await complete.Task;
        }

        /// <summary>
        /// Get the associated page.
        /// </summary>
        /// <typeparam name="T">page type</typeparam>
        /// <returns></returns>
        protected T GetPage<T>() where T : Page
        {
            return (T)ServiceProvider.Resolve<IPageService>().ResolvePage(this);
        }

        /// <summary>
        /// Runs an action on the main thread.
        /// </summary>
        /// <param name="action">action</param>
        protected virtual async Task RunOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
            await Task.FromResult(0);
        }

        #endregion
    }
}
