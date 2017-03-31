// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.MulticastIPAddressInformationCollection
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Stores a set of <see cref="MulticastIPAddressInformation"/> types.
    /// </summary>
    public sealed class MulticastIPAddressInformationCollection : ICollection<MulticastIPAddressInformation>, IEnumerable<MulticastIPAddressInformation>, IEnumerable
    {
        private Collection<MulticastIPAddressInformation> addresses = new Collection<MulticastIPAddressInformation>();

        internal MulticastIPAddressInformationCollection()
        {
        }
        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="address"></param>
        public void Add(MulticastIPAddressInformation address)
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
        /// Checks whether the collection contains the specified <see cref="MulticastIPAddressInformation"/> object.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Contains(MulticastIPAddressInformation address)
        {
            return this.addresses.Contains(address);
        }
        /// <summary>
        /// Copies the elements in this collection to a one-dimensional array of type <see cref="MulticastIPAddressInformation"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        public void CopyTo(MulticastIPAddressInformation[] array, int offset)
        {
            this.addresses.CopyTo(array, offset);
        }

        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MulticastIPAddressInformation> GetEnumerator()
        {
            return this.addresses.GetEnumerator();
        }

        internal void InternalAdd(MulticastIPAddressInformation address)
        {
            this.addresses.Add(address);
        }
        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Remove(MulticastIPAddressInformation address)
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
        /// Gets the number of <see cref="MulticastIPAddressInformation"/> types in this collection.
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
        /// Gets the <see cref="MulticastIPAddressInformation"/> instance at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>The <see cref="MulticastIPAddressInformation"/> at the specified location.</returns>
        public MulticastIPAddressInformation this[int index]
        {
            get
            {
                return this.addresses[index];
            }
        }
    }
}