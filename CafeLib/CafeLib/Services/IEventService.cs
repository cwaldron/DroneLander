using System;
using System.Threading.Tasks;
using CafeLib.EventMessaging;

namespace CafeLib.Services
{
    public interface IEventService : IServiceProvider
    {
        /// <summary>
        /// Subscribe the specified handler.
        /// </summary>
        /// <param name='action'>
        /// Action.
        /// </param>
        /// <typeparam name='T'>
        /// Event message type parameter.
        /// </typeparam>
        Guid Subscribe<T>(Action<T> action) where T : IEventMessage;

        /// <summary>
        /// Publish the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <typeparam name='T'>
        /// Event message type parameter.
        /// </typeparam>
        void Publish<T>(T message) where T : IEventMessage;

        /// <summary>
        /// Publish the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <typeparam name='T'>
        /// Event message type parameter.
        /// </typeparam>
        Task PublishAsync<T>(T message) where T : IEventMessage;


        /// <summary>
        /// Unsubscribe the specified handler.
        /// </summary>
        /// <param name='action'>
        /// Action.
        /// </param>
        /// <typeparam name='T'>
        /// Event message type parameter.
        /// </typeparam>
        void Unsubscribe<T>() where T : IEventMessage;


        /// <summary>
        /// Unsubscribe the specified handler.
        /// </summary>
        /// <param name='action'>
        /// Action.
        /// </param>
        /// <typeparam name='T'>
        /// Event message type parameter.
        /// </typeparam>
        void Unsubscribe<T>(Guid actionId) where T : IEventMessage;
    }
}
