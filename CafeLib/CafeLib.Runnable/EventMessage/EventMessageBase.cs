using System;

namespace CafeLib.Runnable.EventMessage
{
    public abstract class EventMessageBase : IEventMessage
    {
        public DateTime TimeStamp { get; }

        protected EventMessageBase()
        {
            TimeStamp = DateTime.UtcNow;
        }

        protected EventMessageBase(IEventMessage message)
        {
            TimeStamp = message.TimeStamp;
        }
    }
}
