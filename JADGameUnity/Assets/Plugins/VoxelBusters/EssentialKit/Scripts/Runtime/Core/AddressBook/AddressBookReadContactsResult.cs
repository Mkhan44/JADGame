using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="AddressBook.ReadContacts(EventCallback{AddressBookReadContactsResult})"/> operation is completed.
    /// </summary>
    public class AddressBookReadContactsResult
    {
        #region Properties

        /// <summary>
        /// Contains the contacts details retrieved from address book.
        /// </summary>
        /// <value>If the requested operation was successful, this property holds an array of <see cref="IAddressBookContact"/> objects; otherwise, this is null.</value>
        public IAddressBookContact[] Contacts
        {
            get;
            internal set;
        }

        #endregion
    }
}