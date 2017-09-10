using System;
using System.Collections.Concurrent;

namespace CafeLib.Services
{
    internal class PropertyService : ServiceBase, IPropertyService
    {
        private readonly ConcurrentDictionary<string, object> _dictionary;

        /// <summary>
        /// PropertyService constructor.
        /// </summary>
        public PropertyService()
        {
            _dictionary = new ConcurrentDictionary<string, object>();
        }

        /// <inheritdoc />
        public T GetProperty<T>()
        {
            return (T)_dictionary[typeof(T).FullName];
        }

        /// <inheritdoc />
        public void SetProperty<T>(T value)
        {
            _dictionary.AddOrUpdate(typeof(T).FullName, value, (k, v) => value);
        }

        /// <inheritdoc />
        public T GetProperty<T>(string key)
        {
            return (T)_dictionary[key];
        }

        /// <inheritdoc />
        public void SetProperty<T>(string key, T value)
        {
            _dictionary.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <inheritdoc />
        public T GetProperty<T>(Guid guid)
        {
            return (T)_dictionary[guid.ToString("B")];
        }

        /// <inheritdoc />
        public void SetProperty<T>(Guid guid, T value)
        {
            _dictionary.AddOrUpdate(guid.ToString("B"), value, (k, v) => value);
        }
    }
}
