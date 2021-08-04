using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    public abstract class PlayerBase : NativeObjectBase, IPlayer
    {
        #region Abstract methods

        protected abstract string GetIdInternal();

        protected abstract string GetAliasInternal();
        
        protected abstract string GetDisplayNameInternal();
        
        protected abstract void LoadImageInternal(LoadImageInternalCallback callback);

        #endregion

        #region Base class methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("Player { ");
            sb.Append("Id: ").Append(Id).Append(" ");
            sb.Append("Alias: ").Append(Alias).Append(" ");
            sb.Append("DisplayName: ").Append(DisplayName).Append(" ");
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IGameServicesPlayer implementation

        public string Id
        {
            get
            {
                return GetIdInternal();
            }
        }

        public string Alias
        {
            get
            {
                return GetAliasInternal();
            }
        }

        public string DisplayName
        {
            get
            {
                return GetDisplayNameInternal();
            }
        }

        public void LoadImage(EventCallback<TextureData> callback)
        {
            LoadImageInternal((imageData, error) =>
            {
                // send result to caller object
                var     data    = (imageData == null) ? null : new TextureData(imageData);
                CallbackDispatcher.InvokeOnMainThread(callback, data, error);
            });
        }

        #endregion
    }
}