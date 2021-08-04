#if UNITY_ANDROID
using System;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.AddressBookCore.Android
{
    internal sealed class AddressBookContact : AddressBookContactBase, IAddressBookContact
    {
#region Fields

        private     NativeAddressBookContact    m_instance;
        private     NativeContext               m_context;

#endregion

#region Constructors

        public AddressBookContact(NativeAddressBookContact nativeAddressBook)
        {
            m_instance = nativeAddressBook;
        }

        ~AddressBookContact()
        {
            Dispose(false);
        }

#endregion

#region Base implementation

        protected override string GetFirstNameInternal()
        {
            return m_instance.GetGivenName();
        }

        protected override string GetMiddleNameInternal()
        {
            return "";
        }

        protected override string GetLastNameInternal() 
        {
            return m_instance.GetFamilyName();
        }

        protected override string[] GetPhoneNumbersInternal()
        {
            return m_instance.GetPhoneNumbers();
        }

        protected override string[] GetEmailAddressesInternal()
        {
            return m_instance.GetEmailAddresses();
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            m_instance.GetProfilePicture().Load(new NativeLoadAssetListener
            {
                onSuccessCallback = (NativeByteBuffer data) =>
                {
                    callback(data.GetBytes(), null);
                },
                onFailureCallback = (string error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

#endregion
    }
}
#endif