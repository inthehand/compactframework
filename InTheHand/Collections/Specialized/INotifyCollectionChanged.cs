// In The Hand - .NET Components for Mobility
//
// InTheHand.Collections.Specialized.INotifyCollectionChanged
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

namespace System.Collections.Specialized
{
    /// <summary>
    /// Notifies listeners of dynamic changes, such as when items get added and removed or the whole list is refreshed.
    /// </summary>
    /// <remarks>You can enumerate over any collection that implements the <see cref="IEnumerable"/> interface.
    /// However, to set up dynamic bindings so that insertions or deletions in the collection update the UI automatically, the collection must implement the <see cref="INotifyCollectionChanged"/> interface.
    /// This interface exposes the <see cref="CollectionChanged"/> event that must be raised whenever the underlying collection changes.</remarks>
    public interface INotifyCollectionChanged
    {
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        /// <remarks>The event handler receives an argument of type <see cref="NotifyCollectionChangedEventArgs"/>,which contains data that is related to this event.</remarks>
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}