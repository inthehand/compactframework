// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.IPAddressCollection
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Stores a set of <see cref="IPAddress"/> types.
    /// </summary>
    public sealed class IPAddressCollection : ICollection<IPAddress>, IEnumerable<IPAddress>, IEnumerable
    {
        private Collection<IPAddress> addresses = new Collection<IPAddress>();

        internal IPAddressCollection() {}

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="address"></param>
        public void Add(IPAddress address)
        {
            throw new NotSupportedException(Properties.Resources.net_collection_readonly);
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException(Properties.Resources.net_collection_readonly);
        }

        /// <summary>
        /// Checks whether the collection contains the specified <see cref="IPAddress"/> object.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Contains(IPAddress address)
        {
            return addresses.Contains(address);
        }

        /// <summary>
        /// Copies the elements in this collection to a one-dimensional array of type <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
        /// <param name="offset">The zero-based index in array at which the copy begins.</param>
        public void CopyTo(IPAddress[] array, int offset)
        {
            this.addresses.CopyTo(array, offset);
        }

        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IPAddress> GetEnumerator()
        {
            return this.addresses.GetEnumerator();
        }

        internal void InternalAdd(IPAddress address)
        {
            this.addresses.Add(address);
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Remove(IPAddress address)
        {
            throw new NotSupportedException(Properties.Resources.net_collection_readonly);
        }

        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return addresses.GetEnumerator();
        }

        /// <summary>
        /// Gets the number of <see cref="IPAddress"/> types in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.addresses.Count;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether access to this collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the <see cref="IPAddress"/> instance at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>The <see cref="IPAddress"/> at the specified location.</returns>
        public IPAddress this[int index]
        {
            get
            {
                return this.addresses[index];
            }
        }
    }
}