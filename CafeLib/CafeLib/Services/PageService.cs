﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CafeLib.Extensions;
using CafeLib.Support;
using CafeLib.Support.Resolution;
using CafeLib.ViewModels;
using CafeLib.Views;
using Xamarin.Forms;

namespace CafeLib.Services
{
    internal class PageService : ServiceBase, IPageService, INavigationService
    {
        #region Private Members

        private readonly Assembly _entryAssembly;
        private readonly Dictionary<Type, ViewModelResolver> _viewModelResolvers;
        private readonly Dictionary<Type, PageResolver> _pageResolvers;

        public IReadOnlyList<Page> ModalStack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IReadOnlyList<Page> NavigationStack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Constructors

        public PageService(Type entryType)
        {
            _entryAssembly = entryType.GetTypeInfo().Assembly;
            _viewModelResolvers = new Dictionary<Type, ViewModelResolver>();
            _pageResolvers = new Dictionary<Type, PageResolver>();
        }

        #endregion

        #region Initializers

        public async Task<IPageService> InitAsync()
        {
            RegisterViewModels();
            return await Task.FromResult(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Display Alert 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ok"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
        }

        /// <summary>
        /// Navigate to page associated to the view model.
        /// </summary>
        /// <param name="viewModel">view model</param>
        /// <param name="animate"></param>
        /// <returns>true if successful otherwise false</returns>
        public async Task<bool> PushAsync(BaseViewModel viewModel, bool animate=false)
        {
            var page = ResolvePage(viewModel);
            if (page == null) return false;

            page.SetViewModel(viewModel);
            await PushAsync(page, animate);
            return true;
        }

        /// <summary>
        /// Push async navigation.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="animate"></param>
        public async Task PushAsync(Page page, bool animate=false)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page, animate);
        }

        /// <summary>
        /// Pop async navigation.
        /// </summary>
        /// <param name="animate"></param>
        /// <returns>page</returns>
        public async Task<Page> PopAsync(bool animate=false)
        {
            return await Application.Current.MainPage.Navigation.PopAsync(animate);
        }

        /// <summary>
        /// Resolves page associasted with the viewmodel.
        /// </summary>
        /// <param name="viewModelType">view model type</param>
        /// <returns>bounded page</returns>
        public Page ResolvePage(Type viewModelType)
        {
            if (!viewModelType.GetTypeInfo().IsSubclassOf(typeof(BaseViewModel)))
            {
                throw new ArgumentException($"{nameof(viewModelType)} is not a type of BaseViewModel");
            }

            // Leave if no page resolver exist for this view model type.
            if (!_pageResolvers.ContainsKey(viewModelType)) return null;

            // Resolve the page.
            var pageResolver = _pageResolvers[viewModelType];
            return (Page)pageResolver.Resolve(pageResolver.GetResolveType());
        }

        /// <summary>
        /// Resolves viewmodel to is assocated view.
        /// </summary>
        /// <typeparam name="T">type of BaseViewModel</typeparam>
        /// <returns>bounded page</returns>
        public Page ResolvePage<T>(T viewModel) where T : BaseViewModel
        {
            var page = ResolvePage(viewModel.GetType());

            page.BindingContext = viewModel;

            return page;
        }

        public Page ResolvePage<T>() where T : BaseViewModel
        {
            return ResolvePage(typeof(T));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Register view models.
        /// </summary>
        private void RegisterViewModels()
        {
            var viewModelTypeInfos = _entryAssembly.CreatableTypes().Inherits(typeof(BaseViewModel).GetTypeInfo()).EndsWith("ViewModel");

            foreach (var viewModelTypeInfo in viewModelTypeInfos)
            {
                // Get the view type.
                var pageType = FindPageType(viewModelTypeInfo);
                if (pageType == null || _viewModelResolvers.ContainsKey(viewModelTypeInfo.AsType())) continue;
                _viewModelResolvers.Add(viewModelTypeInfo.AsType(), new ViewModelResolver(viewModelTypeInfo.AsType()));
                _pageResolvers.Add(viewModelTypeInfo.AsType(), new PageResolver(pageType));
            }
        }

        /// <summary>
        /// Find corresponding page type for a view model tyoe.
        /// </summary>
        /// <param name="viewModelTypeInfo"></param>
        /// <returns></returns>
        private Type FindPageType(TypeInfo viewModelTypeInfo)
        {
            var viewAttribute = viewModelTypeInfo.GetCustomAttributes<PageAttribute>().SingleOrDefault();
            if (viewAttribute != null)
            {
                return viewAttribute.PageType;
            }
            else
            {
                var viewTypeInfo = _entryAssembly.CreatableTypes().Inherits(typeof(Page).GetTypeInfo()).SingleOrDefault(x => x.Name + "Model" == viewModelTypeInfo.Name);
                return viewTypeInfo?.AsType();
            }
        }

        #endregion
    }
}
