// In The Hand - .NET Components for Mobility
//
// InTheHand.Net.NetworkInformation.IPAddressInformationCollection
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
    /// Stores a set of <see cref="IPAddressInformation"/> types.
    /// </summary>
    public sealed class IPAddressInformationCollection : ICollection<IPAddressInformation>, IEnumerable<IPAddressInformation>, IEnumerable
    {
        private Collection<IPAddressInformation> addresses = new Collection<IPAddressInformation>();

        /// <summary>
        /// Gets the <see cref="IPAddressInformation"/> instance at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>The <see cref="IPAddressInformation"/> at the specified location.</returns>
        public IPAddressInformation this[int index]
        {
            get
            {
                return this.addresses[index];
            }
        }

        internal void InternalAdd(IPAddressInformation address)
        {
            this.addresses.Add(address);
        }

        #region ICollection<IPAddressInformation> Members
        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(IPAddressInformation item)
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
        /// Checks whether the collection contains the specified <see cref="IPAddressInformation"/> object.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(IPAddressInformation item)
        {
            return addresses.Contains(item);
        }
        /// <summary>
        /// Copies the elements in this collection to a one-dimensional array of type <see cref="IPAddressInformation"/>.
        /// </summary>
        /// <param name="array">A one-dimensional array that receives a copy of the collection.</param>
        /// <param name="arrayIndex">The zero-based index in array at which the copy begins.</param>
        public void CopyTo(IPAddressInformation[] array, int arrayIndex)
        {
            addresses.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Gets the number of <see cref="IPAddressInformation"/> types in this collection.
        /// </summary>
        public int Count
        {
            get { return addresses.Count; }
        }
        /// <summary>
        /// Gets a value that indicates whether access to this collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }
        /// <summary>
        /// Throws a <see cref="NotSupportedException"/> because this operation is not supported for this collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(IPAddressInformation item)
        {
            throw new NotSupportedException(Properties.Resources.net_collection_readonly);
        }

        #endregion

        #region IEnumerable<IPAddressInformation> Members
        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IPAddressInformation> GetEnumerator()
        {
            return addresses.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an object that can be used to iterate through this collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return addresses.GetEnumerator();
        }

        #endregion
    }

    
}