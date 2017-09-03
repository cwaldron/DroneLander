using System;
using System.Threading;
using System.Threading.Tasks;
using CafeLib.Runnable.EventMessage;

namespace CafeLib.Runnable.Runner
{
    /// <summary>
    /// Runs a background task.
    /// </summary>
    public abstract class RunnerBase : IRunner
    {
        #region Private Variables

        private static readonly object Mutex = new object();
        private int _delay;
        private bool _disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Runner name.
        /// </summary>
        protected string Name { get; }

        /// <summary>
        /// Log event handler.
        /// </summary>
        protected Action<IEventMessage> RunnerEvent { get; set; }

        /// <summary>
        /// Runner delay duration in milliseconds.
        /// </summary>
        protected int Delay
        {
            get => _delay;
            set => _delay = value > 0 ? value : 0;
        }

        /// <summary>
        /// Cancellation source.
        /// </summary>
        protected CancellationTokenSource CancellationSource { get; set; }

        /// <summary>
        /// Determines whether the service is running.
        /// </summary>
        public bool IsRunning => CancellationSource != null && !CancellationSource.IsCancellationRequested;

        #endregion

        #region Constructors

        /// <summary>
        /// ServiceRunnerBase constructor.
        /// </summary>
        /// <param name="delay">runner delay duration</param>
        protected RunnerBase(uint delay = 0)
        {
            Name = GetType().Name;
            CancellationSource = null;
            Delay = (int)delay;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the service.
        /// </summary>
        public virtual async Task Start()
        {
            lock (Mutex)
            {
                if (!IsRunning)
                {
                    CancellationSource = new CancellationTokenSource();
                    RunnerEvent?.Invoke(new RunnerEventMessage(ErrorLevel.Diagnostic, $"{Name} started."));
                    Runner();
                }
            }
            await Task.FromResult(0);
        }

        /// <summary>
        /// Stop the service.
        /// </summary>
        public virtual async Task Stop()
        {
            if (IsRunning)
            {
                CancellationSource.Cancel();
                RunnerEvent?.Invoke(new RunnerEventMessage(ErrorLevel.Diagnostic, $"{Name} stopped."));
            }

            await Task.FromResult(0);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Run the service.
        /// </summary>
        protected abstract Task Run();

        /// <summary>
        /// Runs the task with an exception guard and delay. 
        /// </summary>
        /// <returns>awaitable task</returns>
        protected async Task RunTask()
        {
            while (IsRunning)
            {
                try
                {
                    await Run();
                }
                catch (Exception ex)
                {
                    RunnerEvent.Invoke(new RunnerEventMessage($"{Name} exception: {ex.Message} {ex.InnerException?.Message}"));
                }

                await Task.Delay(Delay, CancellationSource.Token);
            }
        }

        /// <summary>
        /// Run the task in the background.
        /// </summary>
        protected void Runner()
        {
            Task.Run(async () =>
            {
                try
                {
                    await RunTask();
                }
                catch
                {
                    // ignore
                }
                finally
                {
                    ExitTask();
                }

            }, CancellationSource.Token);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Exit the background task.
        /// </summary>
        private void ExitTask()
        {
            lock (Mutex)
            {
                CancellationSource?.Dispose();
                CancellationSource = null;
                RunnerEvent.Invoke(new RunnerEventMessage(ErrorLevel.Diagnostic, $"{Name} stopped."));
            }
        }

        #endregion

        #region IDisposible

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(!_disposed);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose concurrent queue.
        /// </summary>
        /// <param name="disposing">indicate whether the queue is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            try
            {
                CancellationSource?.Dispose();
            }
            catch
            {
                // ignore
            }
            finally
            {
                CancellationSource = null;
            }
        }

        #endregion
    }
}
