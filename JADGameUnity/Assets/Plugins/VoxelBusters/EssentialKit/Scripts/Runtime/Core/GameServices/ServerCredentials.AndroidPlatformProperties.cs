using System;
using System.Collections;
using System.Text;

namespace VoxelBusters.EssentialKit
{
    public partial class ServerCredentials
    {
        public class AndroidPlatformProperties
        {
			public string ServerAuthCode
			{
				get;
				private set;
			}

			public string IdToken
			{
				get;
				private set;
			}

			public string Email
			{
				get;
				private set;
			}

			#region Constructors

			public AndroidPlatformProperties(string serverAuthCode, string idToken, string email)
            {
				ServerAuthCode	= serverAuthCode;
				IdToken			= idToken;
				Email			= email;
			}

			#endregion

		}
    }
}
