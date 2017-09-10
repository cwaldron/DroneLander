﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CafeLib.EventMessaging;

namespace CafeLib.Services
{
    internal class EventService : ServiceBase, IEventService
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>> _magazine;

        /// <summary>
        /// EventBus constructor.
        /// </summary>
        public EventService()
        {
            _magazine = new ConcurrentDictionary<Type, ConcurrentDictionary<Guid, object>>();
        }

        /// <summary>
        /// Subscribe the specified handler.
        /// </summary>
        /// <param name='action'>
        /// Event action.
        /// </param>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public Guid Subscribe<T>(Action<T> action) where T : IEventMessage
        {
            var subscribers = _magazine.GetOrAdd(typeof(T), new ConcurrentDictionary<Guid, object>());
            var key = new Guid();
            subscribers.TryAdd(key, action);
            return key;
        }

        /// <summary>
        /// Publish the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public void Publish<T>(T message) where T : IEventMessage
        {
            if (_magazine.ContainsKey(typeof(T)))
            {
                var subscribers = _magazine[typeof(T)];
                foreach (var subscriber in subscribers)
                {
                    ((Action<T>)subscriber.Value)?.Invoke(message);
                }
            }
        }

        /// <summary>
        /// Publish the specified message.
        /// </summary>
        /// <param name='message'>
        /// Message.
        /// </param>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public async Task PublishAsync<T>(T message) where T : IEventMessage
        {
            if (_magazine.ContainsKey(typeof(T)))
            {
                var subscribers = _magazine[typeof(T)];
                foreach (var subscriber in subscribers)
                {
                    await Task.Run(() => ((Action<T>)subscriber.Value)?.Invoke(message));
                }
            }
        }

        /// <summary>
        /// Unsubscribe the specified handler.
        /// </summary>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public void Unsubscribe<T>() where T : IEventMessage
        {
            if (_magazine.ContainsKey(typeof(T)))
            {
                var subscribers = _magazine[typeof(T)];
                foreach (KeyValuePair<Guid, object> subscriber in subscribers)
                {
                    object value;
                    subscribers.TryRemove(subscriber.Key, out value);

                }

                ConcurrentDictionary<Guid, object> value2;
                _magazine.TryRemove(typeof(T), out value2);
            }
        }

        /// <summary>
        /// Unsubscribe the specified handler.
        /// </summary>
        /// <param name="actionId"></param>
        /// <typeparam name='T'>
        /// Type of IEventMessage.
        /// </typeparam>
        public void Unsubscribe<T>(Guid actionId) where T : IEventMessage
        {
            if (_magazine.ContainsKey(typeof(T)))
            {
                var subscribers = _magazine[typeof(T)];
                object value1;
                subscribers.TryRemove(actionId, out value1);
                if (subscribers.Count == 0)
                {
                    ConcurrentDictionary<Guid, object> value2;
                    _magazine.TryRemove(typeof(T), out value2);
                }
            }
        }
    }
}