using System;
using System.Threading.Tasks;

namespace CafeLib.Runnable.Runner
{
    /// <summary>
    /// Runner interface.
    /// </summary>
    public interface IRunner : IDisposable
    {
        Task Start();
        Task Stop();
        bool IsRunning { get; }
    }
}
