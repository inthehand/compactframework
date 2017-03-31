// In The Hand - .NET Components for Mobility
//
// InTheHand.Collections.ArrayList
// 
// Copyright (c) 2010-2014 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;

namespace InTheHand.Collections
{
    /// <summary>
    /// Helper for <see cref="ArrayList"/> class.
    /// </summary>
    public static class ArrayListInTheHand
    {
        /// <summary>
        /// Returns a read-only <see cref="IList"/> wrapper.
        /// </summary>
        /// <param name="list">The <see cref="IList"/> to wrap</param>
        /// <returns>A read-only <see cref="IList"/> wrapper around list.</returns>
        public static IList ReadOnly(IList list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return new ReadOnlyList(list);
        }

        /// <exclude/>
        [Serializable]
        private sealed class ReadOnlyList : IList, ICollection, IEnumerable
        {
            private IList _list;

            internal ReadOnlyList(IList l)
            {
                this._list = l;
            }

            public int Add(object obj)
            {
                throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }

            public void Clear()
            {
                throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }

            public bool Contains(object obj)
            {
                return this._list.Contains(obj);
            }

            public void CopyTo(Array array, int index)
            {
                this._list.CopyTo(array, index);
            }

            public IEnumerator GetEnumerator()
            {
                return this._list.GetEnumerator();
            }

            public int IndexOf(object value)
            {
                return this._list.IndexOf(value);
            }

            public void Insert(int index, object obj)
            {
                throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }

            public void Remove(object value)
            {
                throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
            }

            public int Count
            {
                get
                {
                    return this._list.Count;
                }
            }

            public bool IsFixedSize
            {
                get
                {
                    return true;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return this._list.IsSynchronized;
                }
            }

            public object this[int index]
            {
                get
                {
                    return this._list[index];
                }

                set
                {
                    throw new NotSupportedException(Properties.Resources.NotSupported_ReadOnlyCollection);
                }
            }

            public object SyncRoot
            {
                get
                {
                    return this._list.SyncRoot;
                }
            }
        }
    }
}