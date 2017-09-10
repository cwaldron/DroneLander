using System;

namespace CafeLib.EventMessaging
{
    public interface IEventMessage
    {
        DateTime TimeStamp { get; }
    }
}
