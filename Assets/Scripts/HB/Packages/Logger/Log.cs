using System;
using System.Collections.Generic;

namespace HB.Packages.Logger
{
    public abstract class Logger
    {
        #region Public Fields

        public Log.Level LogLevel;

        #endregion

        #region Public Methods

        public abstract void Debug(string channel, string message);
        public abstract void Info(string channel, string message);
        public abstract void Warn(string channel, string message);
        public abstract void Error(string channel, string message);

        #endregion

        #region Protected Methods

        protected virtual string GetLogFormat(string channel, Log.Level level, string message)
        {
            return $"[{DateTime.Now:HH:mm:ss}][{channel}][{level}] {message}";
        }

        #endregion
    }

    public class UnityLogger : Logger
    {
        #region Public Methods

        public void LogFormatted(string channel, Log.Level logLevel, string message)
        {
            if (LogLevel <= logLevel)
                UnityEngine.Debug.Log(GetLogFormat(channel, Log.Level.Debug, message));
        }

        public override void Debug(string channel, string message)
        {
            LogFormatted(channel, Log.Level.Debug, message);
        }

        public override void Info(string channel, string message)
        {
            LogFormatted(channel, Log.Level.Info, message);
        }

        public override void Warn(string channel, string message)
        {
            if (LogLevel <= Log.Level.Warn)
                UnityEngine.Debug.LogWarning(GetLogFormat(channel, Log.Level.Debug, message));
        }

        public override void Error(string channel, string message)
        {
            if (LogLevel <= Log.Level.Debug)
                UnityEngine.Debug.LogError(GetLogFormat(channel, Log.Level.Debug, message));
        }

        #endregion
    }

    public static class Log
    {
        #region  Nested Types

        public enum Level
        {
            Debug,
            Info,
            Warn,
            Error,
            Disable
        }

        #endregion

        #region  Static

        private static readonly List<Logger> Loggers = new List<Logger> {new UnityLogger()};

        #endregion

        #region  Constructors

        static Log()
        {
        }

        #endregion

        #region Public Methods

        public static void SetLevel(Level level)
        {
            foreach (Logger logger in Loggers)
                logger.LogLevel = level;
        }

        public static void Debug(string channel, string message)
        {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
            foreach (Logger logger in Loggers)
                logger.Debug(channel, message);
#endif            
        }

        public static void Info(string channel, string message)
        {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
            foreach (Logger logger in Loggers)
                logger.Info(channel, message);
#endif            
        }

        public static void Warn(string channel, string message)
        {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
            foreach (Logger logger in Loggers)
                logger.Warn(channel, message);
#endif            
        }

        public static void Error(string channel, string message)
        {
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
            foreach (Logger logger in Loggers)
                logger.Error(channel, message);
#endif            
        }

        #endregion
    }
}