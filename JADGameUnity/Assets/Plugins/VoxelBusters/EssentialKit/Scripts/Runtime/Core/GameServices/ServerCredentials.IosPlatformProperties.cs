
using System.Collections;

namespace VoxelBusters.EssentialKit
{
    public partial class ServerCredentials
    {
        public class IosPlatformProperties
        {
			#region Constants

			private const string kCredentialsPublicKeyUrl = "public-key-url";
			private const string kCredentialsSignature = "signature";
			private const string kCredentialsSalt = "salt";
			private const string kCredentialsTimestamp = "timestamp";

            #endregion

            #region Fields

            public string PublicKeyUrl
			{
				get;
				private set;
			}

			public byte[] Signature
			{
				get;
				private set;
			}

			public byte[] Salt
			{
				get;
				private set;
			}

			public long Timestamp
			{
				get;
				private set;
			}

			#endregion

			#region Constructors

			public IosPlatformProperties(string publicKeyUrl, byte[] signature, byte[] salt, long timestamp)
			{
				PublicKeyUrl	= publicKeyUrl;
				Signature		= signature;
				Salt			= salt;
				Timestamp		= timestamp;
			}

			#endregion

			#region Private methods

			private void Load(IDictionary json)
			{
				PublicKeyUrl = (string)json[kCredentialsPublicKeyUrl];

				var signature = (string)json[kCredentialsSignature];
				if (!string.IsNullOrEmpty(signature))
				{
					Signature = System.Convert.FromBase64String(signature);
				}

				string salt = (string)json[kCredentialsSalt];
				if (!string.IsNullOrEmpty(salt))
				{
					Salt = System.Convert.FromBase64String(salt);
				}

				string timestamp = (string)json[kCredentialsTimestamp];

				if (!string.IsNullOrEmpty(timestamp))
				{
					Timestamp = long.Parse(timestamp);
				}
			}

            #endregion
        }
    }
}
