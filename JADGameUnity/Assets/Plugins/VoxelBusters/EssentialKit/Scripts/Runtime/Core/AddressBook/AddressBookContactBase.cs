using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.AddressBookCore
{
    public abstract class AddressBookContactBase : NativeObjectBase, IAddressBookContact
    {
        #region Static fields

        internal    static  Texture2D           defaultImage;

        #endregion

        #region Fields

        private      NativeOperationResultContainer<TextureData>        m_cachedData;

        #endregion

        #region Abstract methods

        protected abstract string GetFirstNameInternal();

        protected abstract string GetMiddleNameInternal();

        protected abstract string GetLastNameInternal();

        protected abstract string[] GetPhoneNumbersInternal();

        protected abstract string[] GetEmailAddressesInternal();

        protected abstract void LoadImageInternal(LoadImageInternalCallback callback);

        #endregion

        #region Base methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("AddressBookContact { ");
            sb.Append("FirstName: ").Append(FirstName).Append(" ");
            sb.Append("LastName: ").Append(LastName);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IAddressBookContact implementation

        public string FirstName
        {
            get
            {
                return GetFirstNameInternal();
            }
        }

        public string MiddleName
        {
            get
            {
                return GetMiddleNameInternal();
            }
        }

        public string LastName
        {
            get
            {
                return GetLastNameInternal();
            }
        }

        public string[] PhoneNumbers
        {
            get
            {
                return GetPhoneNumbersInternal();
            }
        }

        public string[] EmailAddresses
        {
            get
            {
                return GetEmailAddressesInternal();
            }
        }

        public void LoadImage(EventCallback<TextureData> callback)
        {
            // check whether cached inforamtion is available
            if (null == m_cachedData)
            {
                // send proxy results
                if (defaultImage != null)
                {
                    var     proxyData           = new TextureData(defaultImage);
                    var     proxyDataContainer  = NativeOperationResultContainer<TextureData>.Create(result: proxyData); 
                    CallbackDispatcher.InvokeOnMainThread(callback, proxyDataContainer);
                }

                // make actual call
                LoadImageInternal((imageData, error) =>
                {
                    // create data container
                    var     actualData          = (imageData == null) ? null : new TextureData(imageData);
                    var     actualDataContainer = NativeOperationResultContainer<TextureData>.Create(result: actualData); 

                    // save result
                    m_cachedData    = actualDataContainer;

                    // send result to caller object
                    CallbackDispatcher.InvokeOnMainThread(callback, actualDataContainer);
                });
            }
            else
            {
                CallbackDispatcher.InvokeOnMainThread(callback, m_cachedData);
            }
        }

        #endregion
    }
}