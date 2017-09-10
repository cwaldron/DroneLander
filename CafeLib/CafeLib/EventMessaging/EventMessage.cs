using System;
using CafeLib.Services;

namespace CafeLib.EventMessaging
{
    public abstract class EventMessage : IEventMessage
    {
        public DateTime TimeStamp { get; }

        protected EventMessage()
        {
            TimeStamp = DateTime.UtcNow;
        }

        protected EventMessage(IEventMessage message)
        {
            TimeStamp = message.TimeStamp;
        }
    }
}
