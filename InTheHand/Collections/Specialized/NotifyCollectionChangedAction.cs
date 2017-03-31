// In The Hand - .NET Components for Mobility
//
// InTheHand.Collections.Specialized.NotifyCollectionChangedAction
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

namespace System.Collections.Specialized
{
    /// <summary>
    /// Describes the action that caused a <see cref="INotifyCollectionChanged.CollectionChanged"/> event. 
    /// </summary>
    public enum NotifyCollectionChangedAction
    {
        /// <summary>
        /// One or more items were added to the collection.
        /// </summary>
        Add,

        /// <summary>
        /// One or more items were removed from the collection.
        /// </summary>
        Remove,

        /// <summary>
        /// One or more items were replaced in the collection.
        /// </summary>
        Replace,

        /// <summary>
        /// One or more items were moved within the collection.
        /// </summary>
        Move,

        /// <summary>
        /// The content of the collection changed dramatically.
        /// </summary>
        Reset
    }
}