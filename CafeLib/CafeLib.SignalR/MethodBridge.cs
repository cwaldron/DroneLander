using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CafeLib.SignalR
{
    public abstract class MethodBridge
    {
        #region Private Members

        private readonly Dictionary<string, Action<string>> _bridgeMap = new Dictionary<string, Action<string>>();

        #endregion

        #region Automatic Properties

        internal SignalRClient Client { get; set; }

        public string ConnectionId => Client.ConnectionId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MethodBridge class.
        /// </summary>
        protected MethodBridge()
        {
            // Populate the selector map.
            Populate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke the method.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal void Invoke(string method, string data)
        {
            if (_bridgeMap.ContainsKey(method))
            {
                _bridgeMap[method](data);
            }
        }

        /// <summary>
        /// Callback to server.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="args"></param>
        public async void ServerCallback(string callback, params object[] args)
        {
            var data = JsonConvert.SerializeObject(args);
            if (Client != null)
            {
                await Client?.CallServer("ServerCallback", callback, data);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Map the method alias and to its action delegate in the bridge map.
        /// </summary>
        /// <param name='method'>method alias</param>
        /// <param name="methodInfo">method info</param>
        private void MapBridgeEntry(string method, MethodInfo methodInfo)
        {
            var action = (Action<string>)methodInfo.CreateDelegate(typeof(Action<string>), this);

            if (_bridgeMap.ContainsKey(method))
            {
                _bridgeMap[method] = action;
            }
            else
            {
                _bridgeMap.Add(method, action);
            }
        }

        /// <summary>
        /// Populate the bridge map for this instance.
        /// </summary>
        private void Populate()
        {
            foreach (var methodInfo in GetType().GetTypeInfo().DeclaredMethods)
            {
                foreach (var attribute in methodInfo.GetCustomAttributes(typeof(MethodExportAttribute), false))
                {
                    var attr = (MethodExportAttribute) attribute;
                    MapBridgeEntry(attr.Method, methodInfo);
                }
            }
        }

        #endregion
    }
}
