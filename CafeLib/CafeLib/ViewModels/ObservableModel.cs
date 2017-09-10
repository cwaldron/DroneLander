using System;
using System.ComponentModel;

namespace CafeLib.ViewModels
{
    /// <summary>
    /// ObservableModel implemenation class.
    /// </summary>
    /// <typeparam name="T">serialization type</typeparam>
    public abstract class ObservableModel<T> : ObservableBase, ISerializableModel<T>
    {
        /// <summary>
        /// IsDirty indicator.
        /// </summary>
        public bool IsDirty { get; protected set; }

        /// <summary>
        /// ObservableModel constructor.
        /// </summary>
        protected ObservableModel()
        {
            PropertyChanged += ObservableModel_PropertyChanged;
        }

        /// <summary>
        /// ObservableModel_PropertyChanged event handler.
        /// </summary>
        protected virtual void ObservableModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        /// <summary>
        /// Deserialize data into an ObservableModel.
        /// </summary>
        /// <param name="data">serialized data</param>
        public virtual void Deserialize(T data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Serialize ObservableModel.
        /// </summary>
        /// <returns>serialize data</returns>
        public abstract T Serialize();

        /// <summary>
        /// Reset model data to default.
        /// </summary>
        public virtual void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
