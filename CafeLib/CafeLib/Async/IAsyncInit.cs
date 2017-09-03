using System.Threading.Tasks;

namespace CafeLib.Async
{
    /// <summary>
    /// Asynchronous initializer interface.
    /// </summary>
    public interface IAsyncInit<T>
    {
        /// <summary>
        /// The result of the asynchronous initialization of this instance.
        /// </summary>
        Task<T> InitAsync();
    }
}
