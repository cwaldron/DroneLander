﻿using CafeLib.Runnable.EventMessage;

namespace CafeLib.Runnable.Runner
{
    public class RunnerEventMessage : EventMessageBase
    {
        public string Name { get; set; }

        public string Message { get; }

        public ErrorLevel ErrorLevel { get; set; }

        public RunnerEventMessage()
        {
            ErrorLevel = ErrorLevel.Info;
        }

        public RunnerEventMessage(string message)
            : this(null, ErrorLevel.Info, message)
        {
        }

        public RunnerEventMessage(string name, string message)
            : this(name, ErrorLevel.Info, message)
        {
        }

        public RunnerEventMessage(ErrorLevel errorLevel, string message)
            : this(null, errorLevel, message)
        {
        }

        public RunnerEventMessage(string name, ErrorLevel errorLevel, string message)
        {
            Name = name;
            Message = message ?? string.Empty;
            ErrorLevel = errorLevel;
        }

        public RunnerEventMessage(IEventMessage eventMessage)
            : base(eventMessage)
        {
            if (eventMessage is RunnerEventMessage message)
            {
                Name = message.Name;
                Message = message.Message ?? string.Empty;
                ErrorLevel = message.ErrorLevel;
            }
        }
    }
}
