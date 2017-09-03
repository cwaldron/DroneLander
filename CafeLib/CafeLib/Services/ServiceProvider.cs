using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CafeLib.Support;

namespace CafeLib.Services
{
    public sealed class ServiceProvider : SingletonBase<ServiceProvider>, IServiceProvider
    {
        #region Member Variables

        private readonly Dictionary<string, object> _configuration;
        private readonly Dictionary<Type, Creator> _creators;
        private readonly Dictionary<Type, object> _services;
        private static readonly object Mutex = new object();

        #endregion

        #region Delegates

        public delegate object Creator(params object[] args);

        #endregion

        #region Constructors

        /// <summary>
        /// ServiceProvider instance constructor.
        /// </summary>
        public ServiceProvider()
        {
            // Create dictionaries.
            _configuration = new Dictionary<string, object>();
            _creators = new Dictionary<Type, Creator>();
            _services = new Dictionary<Type, object>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static Dictionary<string, object> Configuration => Instance._configuration;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return _services[serviceType];
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Initialize service provider
        /// </summary>
        /// <param name="bootstrapType"></param>
        public static async Task InitAsync(Type bootstrapType)
        {
            Register<IEventService>(x => new EventService());
            Register<IPageService>(x => new PageService(bootstrapType));
            Register<INavigationService>(x => (INavigationService)Resolve<IPageService>());
            await Task.FromResult(0);
        }

        /// <summary>
        /// Register the specified service creator.
        /// </summary>
        /// <param name='creator'>
        /// Creator.
        /// </param>
        /// <typeparam name='T'>
        /// The 1st type parameter.
        /// </typeparam>
        public static void Register<T>(Creator creator)
        {
            lock (Mutex)
            {
                if (!Instance._creators.ContainsKey(typeof(T)))
                {
                    Instance._creators.Add(typeof(T), creator);
                }
            }
        }

        /// <summary>
        /// Resolve the specified service type.
        /// </summary>
        /// <param name='p'>
        /// P.
        /// </param>
        /// <typeparam name='T'>
        /// The 1st type parameter.
        /// </typeparam>
        public static T Resolve<T>(params object[] p)
        {
            lock (Mutex)
            {
                if (!Instance._services.ContainsKey(typeof(T)))
                {
                    Instance._services.Add(typeof(T), Create<T>(p));
                }
            }

            return (T)Instance.GetService(typeof(T));
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns>
        /// The configuration.
        /// </returns>
        /// <param name='name'>
        /// Name.
        /// </param>
        /// <typeparam name='T'>
        /// The 1st type parameter.
        /// </typeparam>
        public static T GetConfiguration<T>(string name)
        {
            return (T)Instance._configuration[name];
        }

        /// <summary>
        /// Shuts down.
        /// </summary>
        public static void ShutDown()
        {
            foreach (var service in Instance._services)
            {
                (service.Value as IDisposable)?.Dispose();
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Creates an instance of the service type.
        /// </summary>
        /// <param name='p'>
        /// P.
        /// </param>
        /// <typeparam name='T'>
        /// The 1st type parameter.
        /// </typeparam>
        public static T Create<T>(params object[] p)
        {
            return (T)Instance._creators[typeof(T)](p);
        }

        #endregion
    }
}
