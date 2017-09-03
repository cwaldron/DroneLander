using System;

namespace CafeLib.Services
{
    public interface IEventMessage
    {
        DateTime TimeStamp { get; }
    }
}
