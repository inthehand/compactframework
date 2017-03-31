// In The Hand - .NET Components for Mobility
//
// InTheHand.Collections.ObjectModel.ObservableCollection<T>
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Collections.ObjectModel
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <remarks>You can enumerate over any collection that implements the IEnumerable interface.
    /// However, to set up dynamic bindings so that insertions or deletions in the collection update the UI automatically, the collection must implement the <see cref="INotifyCollectionChanged"/> interface.
    /// This interface exposes the CollectionChanged event, an event that should be raised whenever the underlying collection changes.
    /// <para>Mobile In The Hand provides the ObservableCollection(T) class, which is an implementation of a data collection that implements the <see cref="INotifyCollectionChanged"/> interface.
    /// For .NET Compact Framework it also implements IBindingList to support UI changes in Windows Forms.</para>
    /// </remarks>
    public class ObservableCollection<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged, IBindingList
    {
        private bool busy;

        private const string CountString = "Count";
        private const string IndexerName = "Item[]";

        private event PropertyChangedEventHandler propertyChanged;

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                this.propertyChanged += value;
            }
            remove
            {
                this.propertyChanged -= value;
            }
        }
 


        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class.
        /// </summary>
        public ObservableCollection()
        {
        }

        private void CheckReentrancy()
        {
            if (this.busy)
            {
                throw new InvalidOperationException(InTheHand.Properties.Resources.ObservableCollectionReentrancyNotAllowed);
            }
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        /// <remarks>The base class calls this method when the list is being cleared.
        /// This implementation raises the <see cref="CollectionChanged"/> event.
        /// <para>For more information, see the <see cref="Collection{T}.ClearItems"/> method of the <see cref="Collection{T}"/> base class.</para>
        /// </remarks>
        protected override void ClearItems()
        {
            this.CheckReentrancy();
            base.ClearItems();
            this.OnPropertyChanged(CountString);
            this.OnPropertyChanged(IndexerName);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            this.CheckReentrancy();
            base.InsertItem(index, item);
            this.OnPropertyChanged(CountString);
            this.OnPropertyChanged(IndexerName);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        /// <remarks>Subclasses can override the <see cref="MoveItem"/> method to provide custom behavior for this method.</remarks>
        public void Move(int oldIndex, int newIndex)
        {
            this.MoveItem(oldIndex, newIndex);
        }

        /// <summary>
        /// Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        /// <remarks><para>This implementation raises the CollectionChanged event.</para>
        /// <para>Subclasses can override this protected method to provide custom behavior for the Move method.</para>
        /// </remarks>
        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            this.CheckReentrancy();
            T item = base[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, item);
            this.OnPropertyChanged(IndexerName);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        /// <remarks>Properties and methods that modify this collection raise the <see cref="CollectionChanged"/> event through this virtual method.</remarks>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.busy = true;
                try
                {
                    this.CollectionChanged(this, e);
                }
                finally
                {
                    this.busy = false;
                }
            }
            if (this.listChanged != null)
            {
                this.busy = true;
                try
                {
                    ListChangedEventArgs lcea = null;
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            lcea = new ListChangedEventArgs(ListChangedType.ItemAdded, e.NewStartingIndex);
                            break;
                        case NotifyCollectionChangedAction.Move:
                            lcea = new ListChangedEventArgs(ListChangedType.ItemMoved, e.NewStartingIndex, e.OldStartingIndex);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            lcea = new ListChangedEventArgs(ListChangedType.ItemDeleted, e.OldStartingIndex);
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            lcea = new ListChangedEventArgs(ListChangedType.ItemChanged, e.OldStartingIndex);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            lcea = new ListChangedEventArgs(ListChangedType.Reset, -1);
                            break;
                    }
                    this.listChanged(this, lcea);
                }
                finally
                {
                    this.busy = false;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the provided arguments.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.propertyChanged != null)
            {
                this.busy = true;
                try
                {
                    this.propertyChanged(this, e);
                }
                finally
                {
                    this.busy = false;
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Removes the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <remarks>The base class calls this method when an item is removed from the collection.
        /// This implementation raises the <see cref="CollectionChanged"/> event.
        /// <para>For more information, see the <see cref="Collection{T}.RemoveItem"/> method of the <see cref="Collection{T}"/> base class.</para>
        /// </remarks>
        protected override void RemoveItem(int index)
        {
            this.CheckReentrancy();
            T changedItem = base[index];
            base.RemoveItem(index);
            this.OnPropertyChanged(CountString);
            this.OnPropertyChanged(IndexerName);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItem, index));
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        /// <remarks>The base class calls this method when an item is set in the collection.
        /// This implementation raises the <see cref="CollectionChanged"/> event.
        /// <para>For more information, see the <see cref="Collection{T}.SetItem"/> method of the <see cref="Collection{T}"/> base class.</para>
        /// </remarks>
        protected override void SetItem(int index, T item)
        {
            this.CheckReentrancy();
            T oldItem = base[index];
            base.SetItem(index, item);
            this.OnPropertyChanged(IndexerName);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index));
        }


        #region IBindingList Members

        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        object IBindingList.AddNew()
        {
            throw new NotSupportedException();
        }

        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return false; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        bool IBindingList.IsSorted
        {
            get { return false; }
        }

        private event ListChangedEventHandler listChanged;

        event ListChangedEventHandler IBindingList.ListChanged
        {
            add
            {
                listChanged += value;
            }
            remove
            {
                listChanged -= value;
            }
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }

        #endregion
    }
}