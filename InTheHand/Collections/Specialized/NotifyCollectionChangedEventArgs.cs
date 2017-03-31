// In The Hand - .NET Components for Mobility
//
// InTheHand.Collections.Specialized.INotifyCollectionChanged
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

namespace System.Collections.Specialized
{
    /// <summary>
    /// Represents the method that handles the <see cref="INotifyCollectionChanged.CollectionChanged"/> event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Information about the event.</param>
    public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="INotifyCollectionChanged.CollectionChanged"/> event.
    /// </summary>
    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        private NotifyCollectionChangedAction _action;
        private IList _newItems;
        private int _newStartingIndex;
        private IList _oldItems;
        private int _oldStartingIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a Reset change.
        /// </summary>
        /// <param name="action"></param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException(string.Format(InTheHand.Properties.Resources.WrongActionForCtor, NotifyCollectionChangedAction.Reset));
            }
            this.InitializeAdd(action, null, -1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a multi-item change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can be set to Reset, Add, or Remove.</param>
        /// <param name="changedItems">The items that are affected by the change.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException(InTheHand.Properties.Resources.MustBeResetAddOrRemoveActionForCtor, "action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItems != null)
                {
                    throw new ArgumentException(InTheHand.Properties.Resources.ResetActionRequiresNullItem, "action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                {
                    throw new ArgumentNullException("changedItems");
                }
                this.InitializeAddOrRemove(action, changedItems, -1);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a one-item change.
        /// </summary>
        /// <param name="action">The action that caused the event.
        /// This can be set to Reset, Add, or Remove.</param>
        /// <param name="changedItem">The item that is affected by the change.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException(InTheHand.Properties.Resources.MustBeResetAddOrRemoveActionForCtor, "action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                {
                    throw new ArgumentException(InTheHand.Properties.Resources.ResetActionRequiresNullItem, "action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                this.InitializeAddOrRemove(action, new object[] { changedItem }, -1);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a multi-item Replace change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can only be set to Replace.</param>
        /// <param name="newItems">The new items that are replacing the original items.</param>
        /// <param name="oldItems">The original items that are replaced.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException(string.Format(InTheHand.Properties.Resources.WrongActionForCtor, NotifyCollectionChangedAction.Replace));
            }
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }
            if (oldItems == null)
            {
                throw new ArgumentNullException("oldItems");
            }
            this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a multi-item change or a reset change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can be set to Reset, Add, or Remove.</param>
        /// <param name="changedItems">The items affected by the change.</param>
        /// <param name="startingIndex">The original items that are replaced.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException(InTheHand.Properties.Resources.MustBeResetAddOrRemoveActionForCtor, "action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItems != null)
                {
                    throw new ArgumentException(InTheHand.Properties.Resources.ResetActionRequiresNullItem, "action");
                }
                if (startingIndex != -1)
                {
                    throw new ArgumentException(InTheHand.Properties.Resources.ResetActionRequiresIndexMinus1, "action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                if (changedItems == null)
                {
                    throw new ArgumentNullException("changedItems");
                }
                if (startingIndex < -1)
                {
                    throw new ArgumentException(InTheHand.Properties.Resources.IndexCannotBeNegative, "startingIndex");
                }
                this.InitializeAddOrRemove(action, changedItems, startingIndex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a one-item change.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="changedItem">The item that is affected by the change.</param>
        /// <param name="index">The index where the change occurred.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (((action != NotifyCollectionChangedAction.Add) && (action != NotifyCollectionChangedAction.Remove)) && (action != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException(InTheHand.Properties.Resources.MustBeResetAddOrRemoveActionForCtor, "action");
            }
            if (action == NotifyCollectionChangedAction.Reset)
            {
                if (changedItem != null)
                {
                    throw new ArgumentException(InTheHand.Properties.Resources.ResetActionRequiresNullItem, "action");
                }
                if (index != -1)
                {
                    throw new ArgumentException(InTheHand.Properties.Resources.ResetActionRequiresIndexMinus1, "action");
                }
                this.InitializeAdd(action, null, -1);
            }
            else
            {
                this.InitializeAddOrRemove(action, new object[] { changedItem }, index);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a one-item Replace change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can only be set to Replace.</param>
        /// <param name="newItem">The new item that is replacing the original item.</param>
        /// <param name="oldItem">The original item that is replaced.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException(string.Format(InTheHand.Properties.Resources.WrongActionForCtor, NotifyCollectionChangedAction.Replace), "action");
            }
            this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, -1, -1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a multi-item Replace change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can only be set to Replace.</param>
        /// <param name="newItems">The new items that are replacing the original items.</param>
        /// <param name="oldItems">The original items that are replaced.</param>
        /// <param name="startingIndex">The index of the first item of the items that are being replaced.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException(string.Format(InTheHand.Properties.Resources.WrongActionForCtor, NotifyCollectionChangedAction.Replace), "action");
            }
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }
            if (oldItems == null)
            {
                throw new ArgumentNullException("oldItems");
            }
            this.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a multi-item Move change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can only be set to Move.</param>
        /// <param name="changedItems">The items affected by the change.</param>
        /// <param name="index">The new index for the changed items.</param>
        /// <param name="oldIndex">The old index for the changed items.</param>
        /// <exception cref="ArgumentException">If action is not Move or index is less than 0.</exception>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Move)
            {
                throw new ArgumentException(string.Format(InTheHand.Properties.Resources.WrongActionForCtor, NotifyCollectionChangedAction.Move), "action");
            }
            if (index < 0)
            {
                throw new ArgumentException(InTheHand.Properties.Resources.IndexCannotBeNegative, "index");
            }
            this.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a multi-item Move change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can only be set to Move.</param>
        /// <param name="changedItem">The item that is affected by the change.</param>
        /// <param name="index">The new index for the changed item.</param>
        /// <param name="oldIndex">The old index for the changed item.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Move)
            {
                throw new ArgumentException(string.Format(InTheHand.Properties.Resources.WrongActionForCtor, NotifyCollectionChangedAction.Move), "action");
            }
            if (index < 0)
            {
                throw new ArgumentException(InTheHand.Properties.Resources.IndexCannotBeNegative, "index");
            }
            object[] newItems = new object[] { changedItem };
            this.InitializeMoveOrReplace(action, newItems, newItems, index, oldIndex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventArgs"/> class that describes a one-item Replace change.
        /// </summary>
        /// <param name="action">The action that caused the event. This can be set to Replace.</param>
        /// <param name="newItem">The new item that is replacing the original item.</param>
        /// <param name="oldItem">The original item that is replaced.</param>
        /// <param name="index">The index of the item being replaced.</param>
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            this._newStartingIndex = -1;
            this._oldStartingIndex = -1;
            if (action != NotifyCollectionChangedAction.Replace)
            {
                throw new ArgumentException(string.Format(InTheHand.Properties.Resources.WrongActionForCtor, NotifyCollectionChangedAction.Replace), "action");
            }
            this.InitializeMoveOrReplace(action, new object[] { newItem }, new object[] { oldItem }, index, index);
        }

        private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
        {
            this._action = action;
            this._newItems = (newItems == null) ? null : InTheHand.Collections.ArrayListInTheHand.ReadOnly(newItems);
            this._newStartingIndex = newStartingIndex;
        }

        private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
        {
            if (action == NotifyCollectionChangedAction.Add)
            {
                this.InitializeAdd(action, changedItems, startingIndex);
            }
            else if (action == NotifyCollectionChangedAction.Remove)
            {
                this.InitializeRemove(action, changedItems, startingIndex);
            }
        }

        private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
        {
            this.InitializeAdd(action, newItems, startingIndex);
            this.InitializeRemove(action, oldItems, oldStartingIndex);
        }

        private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
        {
            this._action = action;
            this._oldItems = (oldItems == null) ? null : InTheHand.Collections.ArrayListInTheHand.ReadOnly(oldItems);
            this._oldStartingIndex = oldStartingIndex;
        }

        /// <summary>
        /// Gets the action that caused the event.
        /// </summary>
        public NotifyCollectionChangedAction Action
        {
            get
            {
                return this._action;
            }
        }

        /// <summary>
        /// Gets the list of new items involved in the change.
        /// </summary>
        public IList NewItems
        {
            get
            {
                return this._newItems;
            }
        }

        /// <summary>
        /// Gets the index at which the change occurred.
        /// </summary>
        public int NewStartingIndex
        {
            get
            {
                return this._newStartingIndex;
            }
        }
        /// <summary>
        /// Gets the list of items affected by a Replace, Remove, or Move action.
        /// </summary>
        public IList OldItems
        {
            get
            {
                return this._oldItems;
            }
        }

        /// <summary>
        /// Gets the index at which a Move, Remove, ore Replace action occurred.
        /// </summary>
        public int OldStartingIndex
        {
            get
            {
                return this._oldStartingIndex;
            }
        }
    }
}