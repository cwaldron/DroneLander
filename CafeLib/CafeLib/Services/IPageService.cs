﻿using System;
using System.Threading.Tasks;
using CafeLib.Async;
using CafeLib.ViewModels;
using Xamarin.Forms;

namespace CafeLib.Services
{
    public interface IPageService : IServiceProvider, IAsyncInit<IPageService>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Page ResolvePage<T>() where T : BaseViewModel;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
