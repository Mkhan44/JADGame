using UnityEngine;

using Exception = System.Exception;

namespace VoxelBusters.CoreLibrary
{
    public static class DebugLogger 
    {
        #region Static fields

        private     static      LogLevel        s_logLevel;

        #endregion

        #region Static methods

        public static void SetLogLevel(LogLevel value)
        {
            s_logLevel  = value;
        }

        #endregion

        #region Log methods

        public static void Log(string message, Object context = null)
        {
            // check whther logging is required
            if (IgnoreLog(LogLevel.Info))
            {
                return;
            }

            var     formattedMessage    = "[VoxelBusters] " + message;
            Debug.Log(formattedMessage, context);
        }

        public static void LogFormat(string format, params object[] arguments)
        {
            LogFormat(null, format, arguments);
        }

        public static void LogFormat(Object context, string format, params object[] arguments)
        {
            // check whther logging is required
            if (IgnoreLog(LogLevel.Info))
            {
                return;
            }

            var     formattedMessage    = "[VoxelBusters] " + string.Format(format, arguments);
            Debug.Log(formattedMessage, context);
        }

        public static void LogWarning(string message, Object context = null)
        {
            // check whther logging is required
            if (IgnoreLog(LogLevel.Warning))
            {
                return;
            }

            var     formattedMessage    = "[VoxelBusters] " + message;
            Debug.LogWarning(formattedMessage, context);
        }

        public static void LogWarningFormat(string format, params object[] arguments)
        {
            LogWarningFormat(null, format, arguments);
        }

        public static void LogWarningFormat(Object context, string format, params object[] arguments)
        {
            // check whther logging is required
            if (IgnoreLog(LogLevel.Warning))
            {
                return;
            }

            var     formattedMessage    = "[VoxelBusters] " + string.Format(format, arguments);
            Debug.LogWarning(formattedMessage, context);
        }

        public static void LogError(string message, Object context = null)
        {
            // check whther logging is required
            if (IgnoreLog(LogLevel.Error))
            {
                return;
            }

            var     formattedMessage    = "[VoxelBusters] " + message;
            Debug.LogError(formattedMessage, context);
        }

        public static void LogErrorFormat(string format, params object[] arguments)
        {
            LogErrorFormat(null, format, arguments);
        }

        public static void LogErrorFormat(Object context, string format, params object[] arguments)
        {
            // check whther logging is required
            if (IgnoreLog(LogLevel.Error))
            {
                return;
            }

            var     formattedMessage    = "[VoxelBusters] " + string.Format(format, arguments);
            Debug.LogError(formattedMessage, context);
        }

        public static void LogException(Exception exception, Object context = null)
        {
            // check whther logging is required
            if (IgnoreLog(LogLevel.Critical))
            {
                return;
            }

            Debug.LogException(exception, context);
        }

        #endregion

        #region Private static methods

        private static bool IgnoreLog(LogLevel level)
        {
            return (level < s_logLevel);
        }

        #endregion

        #region Nested types

        public enum LogLevel
        {            
            Info = 0,

            Warning,

            Error,

            Critical,

            None
        }

        #endregion
    }
}