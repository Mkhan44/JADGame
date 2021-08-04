using System;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    internal static class Diagnostics
    {
        #region Messages

        public  static  readonly    Error   kFeatureNotSupported            = new Error(description: "The requested operation could not be completed because this feature is not supported on current platform.");

        public  const   string              kCreateNativeObjectError        = "Failed to create native object.";

        #endregion

        #region Exception methods

        public static VBException PluginNotConfiguredException()
        {
            return new VBException("Please configure your NativePlugins before you start using it in your project.");
        }

        #endregion

        #region Log methods

        public static void LogNotSupportedInEditor(string featureName = "This")
        {
            DebugLogger.LogWarning(string.Format("{0} feature is not supported by simulator.", featureName));
        }

        public static void LogNotSupported(string featureName = "This")
        {
            DebugLogger.LogWarning(string.Format("{0} feature is not supported.", featureName));
        }

        #endregion
    }
}