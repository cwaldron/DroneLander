using System;
using System.Threading.Tasks;

namespace CafeLib.Runnable
{
    /// <summary>
    /// Runner interface.
    /// </summary>
    public interface IRunnable : IDisposable
    {
        /// <summary>
        /// Indicates the runnable object is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Start runnable object.
        /// </summary>
        /// <returns>task</returns>
        Task Start();

        /// <summary>
        /// Stop runnable object.
        /// </summary>
        /// <returns>task</returns>
        Task Stop();
    }
}
