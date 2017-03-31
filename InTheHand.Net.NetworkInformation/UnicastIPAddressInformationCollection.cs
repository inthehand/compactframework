// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.UnicastIPAddressInformationCollection
// 
// Copyright (c) 2010 In The Hand Ltd, All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InTheHand.Net.NetworkInformation
{
    /// <summary>
    /// Stores a set of <see cref="UnicastIPAddressInformation"/> types.
    /// </summary>
    public sealed class UnicastIPAddressInformationCollection : ICollection<UnicastIPAddressInformation>, IEnumerable<UnicastIPAddressInformation>, IEnumerable
    {
        private Collection<UnicastIPAddressInformation> addresses = new Collection<UnicastIPAddressInformation>();

        internal UnicastIPAddressInformationCollection()
        {
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="address"></param>
        public void Add(UnicastIPAddressInformation address)
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
        /// Checks whether the collection contains the specified <see cref="UnicastIPAddressInformation"/> object.
        /// </summary>
        /// <param name="address">The <see cref="UnicastIPAddressInformation"/> object to be searched in the collection.</param>
        /// <returns>true if the <see cref="UnicastIPAddressInformation"/> object exists in the collection; otherwise, false.</returns>
        public bool Contains(UnicastIPAddressInformation address)
        {
            return this.addresses.Contains(address);
        }

        /// <summary>
        /// Copies the elements in this collection to a one-dimensional array of type <see cref="UnicastIPAddressInformation"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        public void CopyTo(UnicastIPAddressInformation[] array, int offset)
        {
            this.addresses.CopyTo(array, offset);
        }

        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<UnicastIPAddressInformation> GetEnumerator()
        {
            return this.addresses.GetEnumerator();
        }

        internal void InternalAdd(UnicastIPAddressInformation address)
        {
            this.addresses.Add(address);
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool Remove(UnicastIPAddressInformation address)
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
        /// Gets the number of <see cref="UnicastIPAddressInformation"/> types in this collection.
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
        /// Gets the <see cref="UnicastIPAddressInformation"/> instance at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>The <see cref="UnicastIPAddressInformation"/> at the specified location.</returns>
        public UnicastIPAddressInformation this[int index]
        {
            get
            {
                return this.addresses[index];
            }
        }
    }
}