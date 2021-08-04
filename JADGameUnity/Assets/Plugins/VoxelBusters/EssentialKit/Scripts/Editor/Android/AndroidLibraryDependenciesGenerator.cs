#if UNITY_EDITOR && UNITY_ANDROID
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System;

namespace VoxelBusters.EssentialKit.Editor.Android
{
	/// <summary>
	/// Play-Services Dependencies for Cross Platform Native Plugins 2.0 : Essential Kit.
	/// </summary>
	[InitializeOnLoad]
	public class AndroidLibraryDependenciesGenerator
	{
		/// <summary>
		/// This is used to create a settings file
		/// which contains the dependencies specific to your plugin.
		/// </summary>
		private static readonly string DependencyName = "CrossPlatformEssentialKitDependencies.xml";

		
		private static readonly string BillingClientVersionString		= "3.0.2+";
		private static readonly string PlayServicesVersionString		= "21.0.0+";
		private static readonly string PlayServicesNearByVersionString	= "17.0.0+";
		private static readonly string PlayServicesAuthVersionString	= "18.1.0+";
		private static readonly string AndroidXCoreVersionString		= "1.3.2+";
		private static readonly string ExifInterfaceVersionString		= "1.3.0+";
        private static readonly string FCMVersionString					= "20.2.4+";
		private static readonly string PlayCoreVersionString			= "1.9.0";

		/// <summary>
		/// Initializes static members of the <see cref="AndroidLibraryDependenciesGenerator"/> class.
		/// </summary>
		static AndroidLibraryDependenciesGenerator()
		{
			CreateLibraryDependencies();
			EditorApplication.update -= Update;
			EditorApplication.update += Update;
		}

		public static bool CreateLibraryDependencies()
        {
			return CreateLibraryDependenciesInternal(Constants.kPluginEditorSourcePath + DependencyName);
		}

		private static void Update()
        {
			if (!EditorPrefs.HasKey("refresh-feature-dependencies") || EditorPrefs.GetBool("refresh-feature-dependencies")) // TODO: Remove this static key and have a callback system to get triggered when feature selection happens.
            {
				if (CreateLibraryDependencies())
				{
					EditorPrefs.SetBool("refresh-feature-dependencies", false);
				}
			}
        }

		private static bool CreateLibraryDependenciesInternal(string path)
		{
			// Settings
			XmlWriterSettings xmlSettings 	= new XmlWriterSettings();
			xmlSettings.Encoding			= new System.Text.UTF8Encoding(true);
			xmlSettings.ConformanceLevel	= ConformanceLevel.Document;
			xmlSettings.Indent 				= true;
			xmlSettings.NewLineOnAttributes	= true;
			xmlSettings.IndentChars			= "\t";

			EssentialKitSettings settings;
			if (!EssentialKitSettingsEditorUtility.SettingsExists)
            {
				return false;
            }

			settings = EssentialKitSettingsEditorUtility.DefaultSettings;
				
			// Generate and write dependecies
			using (XmlWriter xmlWriter = XmlWriter.Create(path, xmlSettings))
			{
				xmlWriter.WriteStartDocument();
				{
					xmlWriter.WriteComment("DONT MODIFY HERE. AUTO GENERATED DEPENDENCIES FROM AndroidLibraryDependenciesGenerator.cs [Cross Platform Essential Kit : Native Plugins 2.0");

					xmlWriter.WriteStartElement("dependencies");
					{
						xmlWriter.WriteStartElement ("androidPackages");
						{
							if (settings.BillingServicesSettings.IsEnabled)
                            {
								xmlWriter.WriteComment("Dependency added for using Billing Services");
								AndroidDependency billingDependency = new AndroidDependency("com.android.billingclient", "billing", BillingClientVersionString);
								WritePackageDependency(xmlWriter, billingDependency);
							}

							if (settings.GameServicesSettings.IsEnabled)
							{
								xmlWriter.WriteComment("Dependency added for using Google Play Services");

								AndroidDependency playServicesGamesDependency = new AndroidDependency("com.google.android.gms", "play-services-games", PlayServicesVersionString);
								playServicesGamesDependency.AddPackageID("extra-google-m2repository");
								playServicesGamesDependency.AddPackageID("extra-android-m2repository");

								WritePackageDependency(xmlWriter, playServicesGamesDependency);

								AndroidDependency playServicesNearbyDependency = new AndroidDependency("com.google.android.gms", "play-services-nearby", PlayServicesNearByVersionString);
								WritePackageDependency(xmlWriter, playServicesNearbyDependency);

                            	AndroidDependency playServicesAuthDependency = new AndroidDependency("com.google.android.gms", "play-services-auth", PlayServicesAuthVersionString);
                                WritePackageDependency(xmlWriter, playServicesAuthDependency);
                            }

							if (settings.NotificationServicesSettings.IsEnabled && (settings.NotificationServicesSettings.PushNotificationServiceType != PushNotificationServiceType.None))
							{
								xmlWriter.WriteComment("Dependency added for using Remote Notifications");
                                AndroidDependency fcmDependency = new AndroidDependency("com.google.firebase", "firebase-messaging", FCMVersionString);
								WritePackageDependency(xmlWriter, fcmDependency);
							}

							xmlWriter.WriteComment("Dependency added for using RateMyApp or Utilities");
							AndroidDependency playCoreDependency = new AndroidDependency("com.google.android.play", "core", PlayCoreVersionString);
							WritePackageDependency(xmlWriter, playCoreDependency);

							//https://developer.android.com/topic/libraries/support-library/packages.html
							// Marshmallow permissions requires app-compat. Also used by some old API's for compatibility.

							xmlWriter.WriteComment("Dependency added for using AndroidX Core Libraries");
							AndroidDependency androidXDependency	= new AndroidDependency("androidx.core", "core", AndroidXCoreVersionString);
							WritePackageDependency(xmlWriter, androidXDependency);

							AndroidDependency exifDependency	= new AndroidDependency("androidx.exifinterface", "exifinterface", ExifInterfaceVersionString);
							WritePackageDependency(xmlWriter, exifDependency);

						}
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndDocument();
			}

			return true;
		}


		private static void WritePackageDependency(XmlWriter xmlWriter, AndroidDependency dependency)
		{
			xmlWriter.WriteStartElement ("androidPackage");
			{
				xmlWriter.WriteAttributeString ("spec", String.Format("{0}:{1}:{2}", dependency.Group, dependency.Artifact, dependency.Version));

				List<string> packageIDs = dependency.PackageIDs;

				if (packageIDs != null)
				{
					xmlWriter.WriteStartElement ("androidSdkPackageIds");
					{
						foreach(string _each in packageIDs)
						{
							xmlWriter.WriteStartElement ("androidSdkPackageId");
							{
								xmlWriter.WriteString (_each);
							}
							xmlWriter.WriteEndElement ();
						}
					}
					xmlWriter.WriteEndElement ();
				}

			}
			xmlWriter.WriteEndElement ();
		}
	}

	public class AndroidDependency
	{
        private readonly string m_group;
		private readonly string m_artifact;
		private readonly string m_version;

		private	List<string>	m_packageIDs;


		public string Group
		{
			get
			{
				return m_group;
			}
		}

		public string Artifact
		{
			get
			{
				return m_artifact;
			}
		}

		public string Version
		{
			get
			{
				return m_version;
			}
		}

		public List<string> PackageIDs
		{
			get
			{
				return m_packageIDs;
			}
		}

		public AndroidDependency(string _group, string _artifact, string _version)
		{
			m_group = _group;
			m_artifact = _artifact;
			m_version = _version;
		}

		public void AddPackageID(string _packageID)
		{
			if (m_packageIDs == null)
				m_packageIDs = new List<string>();


			m_packageIDs.Add(_packageID);
		}
	}
}
#endif
