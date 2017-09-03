using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using CafeLib.Runnable.Runner;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client;

namespace CafeLib.SignalR
{
    public class SignalRClient : RunnerBase
    {
        public Uri Url { get; }

        public int ConnectionAttempts { get; private set; }

        public bool IsConnected => Connection?.State == ConnectionState.Connected;

        public string ConnectionId => Connection?.ConnectionId;

        internal HubConnection Connection { get; private set; }

        protected Action<SignalRMessage> EventListener { get; private set; }

        protected IHubProxy HubProxy { get; private set; }

        protected MethodBridge Bridge { get; }

        public SignalRClient(string url, Action<SignalRMessage> listener)
            : this(url, null, listener)
        {
        }

        public SignalRClient(string url, MethodBridge methodBridge = null, Action<SignalRMessage> listener = null)
            : base(10)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(url));
            Url = new Uri(url);
            EventListener = listener;
            RunnerEvent = x => listener?.Invoke(new SignalRMessage(this, x));
            if (methodBridge != null)
            {
                Bridge = methodBridge;
                Bridge.Client = this;
            }
        }

        public SignalRClient AddSignalRListener(Action<SignalRMessage> listener)
        {
            EventListener = listener;
            return this;
        }

        public override async Task Stop()
        {
            await Disconnect();
            await base.Stop();
        }

        public async Task CallServer(string method, params object[] parameters)
        {
            if (HubProxy != null)
            {
                await HubProxy.Invoke(method, parameters);
            }
        }

        protected override async Task Run()
        {
            try
            {
                if (!IsConnected)
                {
                    await ConnectClientListeners();
                    Delay = 5000;
                }
            }
            catch (Exception ex)
            {
                RunnerEvent?.Invoke(new SignalRMessage(this, $"Connection attempts: {ConnectionAttempts} => {ex.Message}"));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disconnect().Wait();
            }

            base.Dispose(disposing);
        }

        private async Task ConnectClientListeners()
        {
            Connection = new HubConnection(Url.AbsoluteUri, Url.Query);
            Connection.Error += OnConnectionError;
            Connection.StateChanged += OnHubStateChanged;

            HubProxy = Connection.CreateHubProxy("CommunicationHub");
            HubProxy.On<string>("NotifyMessage", message =>
            {
                EventListener?.Invoke(new SignalRMessage(this, $"Incoming data: {message}"));
            });

            HubProxy.On<string, string>("CallClient", (method, data) =>
            {
                Bridge?.Invoke(method, data);
            });

            await Connection.Start();
        }

        private void OnHubStateChanged(StateChange state)
        {
            switch (state.NewState)
            {
                case ConnectionState.Connected:
                    ConnectionAttempts = 0;
                    EventListener?.Invoke(new SignalRMessage(this, "Connected"));
                    break;

                case ConnectionState.Disconnected:
                    Disconnect().Wait();
                    EventListener?.Invoke(new SignalRMessage(this, "Disconnected"));
                    break;

                case ConnectionState.Connecting:
                case ConnectionState.Reconnecting:
                    ++ConnectionAttempts;
                    break;
            }
        }

        private void OnConnectionError(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Disconnect().Wait();
        }

        /// <summary>
        /// Disconnect the connection.
        /// </summary>
        private async Task Disconnect()
        {
            try
            {
                if (Connection == null) return;
                Connection.Error -= OnConnectionError;
                Connection.StateChanged -= OnHubStateChanged;
                await ServerDisconnect();
            }
            catch
            {
                // ignored
            }
            finally
            {
                await base.Stop();
            }
        }

        /// <summary>
        /// Disconnect from the server.
        /// </summary>
        /// <returns></returns>
        private async Task ServerDisconnect()
        {
            try
            {
                if (IsConnected)
                {
                    await CallServer("ClientDisconnect", "123");
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                //Connection.Disconnect();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}
