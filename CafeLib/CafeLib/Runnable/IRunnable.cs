using System;
using System.Threading.Tasks;

namespace CafeLib.Runnable
{
    /// <summary>
    /// Runner interface.
    /// </summary>
    public interface IRunnable : IDisposable
    {
        Task Start();
        Task Stop();
        bool IsRunning { get; }
    }
}
