// In The Hand - .NET Components for Mobility
//
// InTheHand.Collections.ObjectModel.ReadOnlyObservableCollection<T>
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Collections.ObjectModel
{
    /// <summary>
    /// Represents a read-only <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <remarks>This class is a read-only wrapper around an <see cref="ObservableCollection{T}"/>.</remarks>
    public class ReadOnlyObservableCollection<T> : ReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private event NotifyCollectionChangedEventHandler collectionChanged;

        private event PropertyChangedEventHandler propertyChanged;

        /// <summary>
        /// Occurs when an item is added or removed.
        /// </summary>
        protected event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                collectionChanged += value;
            }
            remove
            {
                collectionChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        protected event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                propertyChanged += value;
            }
            remove
            {
                propertyChanged -= value;
            }
        }

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                collectionChanged += value;
            }
            remove
            {
                collectionChanged -= value;
            }
        }
        
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                propertyChanged += value;
            }
            remove
            {
                propertyChanged -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyObservableCollection{T}"/> class that serves as a wrapper around the specified <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <param name="list">The <see cref="ObservableCollection{T}"/> with which to create this instance of the <see cref="ReadOnlyObservableCollection{T}"/> class.</param>
        public ReadOnlyObservableCollection(ObservableCollection<T> list)
            : base(list)
        {
            ((INotifyCollectionChanged)base.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(this.HandleCollectionChanged);
            ((INotifyPropertyChanged)base.Items).PropertyChanged += new PropertyChangedEventHandler(this.HandlePropertyChanged);
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event using the provided arguments.
        /// </summary>
        /// <param name="args">Arguments of the event being raised.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (this.collectionChanged != null)
            {
                this.collectionChanged(this, args);
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event using the provided arguments.
        /// </summary>
        /// <param name="args">Arguments of the event being raised.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (this.propertyChanged != null)
            {
                this.propertyChanged(this, args);
            }
        }
    }
}