using CafeLib.Runnable.Runner;
using CafeLib.Runnable.EventMessage;
using SignalRConnectionState = Microsoft.AspNet.SignalR.Client.ConnectionState;

namespace CafeLib.SignalR
{
    public class SignalRMessage : RunnerEventMessage
    {
        public enum ConnectionState
        {
            Off,
            Connecting,
            Connected,
            Reconnecting,
            Disconnected
        }

        public SignalRClient Client { get; }

        public ConnectionState State { get; private set; }


        internal SignalRMessage(SignalRClient client, string message)
            : base(message)
        {
            Client = client;
            SetState(client);
        }

        internal SignalRMessage(SignalRClient client, IEventMessage eventMessage)
            : base(eventMessage)
        {
            Client = client;
            SetState(client);
        }

        private void SetState(SignalRClient client)
        {
            if (client.Connection != null)
            {
                switch (client.Connection.State)
                {
                    case SignalRConnectionState.Connected:
                        State = ConnectionState.Connected;
                        break;

                    case SignalRConnectionState.Connecting:
                        State = ConnectionState.Connecting;
                        break;

                    case SignalRConnectionState.Reconnecting:
                        State = ConnectionState.Reconnecting;
                        break;

                    case SignalRConnectionState.Disconnected:
                        State = ConnectionState.Disconnected;
                        break;
                }
            }
            else
            {
                State = ConnectionState.Off;
            }
        }
    }
}
