using System;
using System.Threading;
using System.Threading.Tasks;
using CafeLib.Runnable;

namespace CafeLib.ViewModels
{ 
    public abstract class RunnableViewModel : BaseViewModel, IRunnable
    {
        #region Private Variables

        private CancellationTokenSource _cancellationSource;
        private bool _disposed;

        #endregion

        #region Properties

        public bool IsRunning => _cancellationSource.IsCancellationRequested;

        public CancellationToken CancellationToken { get; protected set; }

        #endregion

        #region Methods

        public async Task Start()
        {
            _cancellationSource = new CancellationTokenSource();
            await Task.FromResult(0);
        }

        public async Task Stop()
        {
            _cancellationSource.Cancel();
            await Task.FromResult(0);
        }

        public async void Delay(int milliseconds)
        {
            try
            {
                await Task.Delay(milliseconds, CancellationToken);
            }
            catch
            {
                // ignore
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
                _cancellationSource?.Dispose();
            }
            catch
            {
                // ignore
            }
            finally
            {
                _cancellationSource = null;
            }
        }

        #endregion

    }
}
