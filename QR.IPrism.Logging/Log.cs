using System;
using System.Collections.Generic;
using System.Globalization;
using log4net;

namespace QR.IPrism.Logging
{
    /// <summary>
    /// Log4Net wrapper class contains methods to log SRP Application Messages. 
    /// Application Messages may be logged on the basis of below following categories,
    /// 1. Information Messages
    /// 2. Warning Messages
    /// 3. Error Messages
    /// 
    /// 4. Fatal Error Messages. 
    /// This class contains various overload methods which can be used for respective message type.
    /// </summary>
    public static class Log
    {

        #region Private Variables
        private static readonly Dictionary<Type, ILog> Loggers = new Dictionary<Type, ILog>();
        private static bool _logInitialized;
        private static readonly object Lock = new object();
        private const string MessagePrefix = "Error in RequestID:{0} \n";
        private const string ServerSideErrorSource = "SRVCD";
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize Configuration Settings for log4net.
        /// </summary>
        private static void Initialize()
        {
            _logInitialized = true;

        }
        /// <summary>
        /// This is method serialized exception object
        /// </summary>
        /// <param name="e"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        private static string SerializeException(Exception e, string exceptionMessage)
        {
            if (e == null) return string.Empty;

            exceptionMessage = string.Format(CultureInfo.InvariantCulture,
            "{0}{1}{2}\n{3}",
            exceptionMessage,
            string.IsNullOrEmpty(exceptionMessage) ? string.Empty : "\n\n",
            e.Message,
            e.StackTrace);

            if (e.InnerException != null)
                exceptionMessage = SerializeException(e.InnerException, exceptionMessage);

            return exceptionMessage;
        }

        /// <summary>
        /// This method return Logger class  instance of interface type
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static ILog GetLogger(Type source)
        {
            lock (Lock)
            {
                if (Loggers.ContainsKey(source))
                {
                    return Loggers[source];
                }
                ILog logger = LogManager.GetLogger(source);
                Loggers.Add(source, logger);
                return logger;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This Method Serialize the Exception object.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string SerializeException(Exception exception)
        {
            return SerializeException(exception, string.Empty);

        }

        /// <summary>
        /// Method to logged message for debugging purpose.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Debug(object source, string message)
        {
            Debug(source.GetType(), message);
        }

        /// <summary>
        /// An overload method to logged message for debugging purpose.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="ps"></param>
        public static void Debug(object source, string message, params object[] ps)
        {
            Debug(source.GetType(), string.Format(message, ps));
        }

        /// <summary>
        /// An overload method to logged message for debugging purpose.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Debug(Type source, string message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsDebugEnabled)
                logger.Debug(message);
        }

        /// <summary>
        /// This method is use to logged informative messages.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        /// <summary>
        /// An overload method is use to logged informative messages.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Info(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsInfoEnabled)
                logger.Info(message);
        }

        /// <summary>
        /// This method is use to logged warning messages.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        /// <summary>
        ///  This overload method is use to logged warning messages.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void Warn(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsWarnEnabled)
                logger.Warn(message);
        }

        /// <summary>
        /// This method is used to log a message object and exception for debug category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        /// <summary>
        /// This overload method is used to log a message object and exception for debug category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(Type source, object message, Exception exception)
        {
            GetLogger(source).Debug(message, exception);
        }

        /// <summary>
        /// This method is used to log a message object and exception for information category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        /// <summary>
        /// This overload method is used to log a message object and exception for debug category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(Type source, object message, Exception exception)
        {
            GetLogger(source).Info(message, exception);
        }

        /// <summary>
        /// This method is used to log a message object and exception for warning category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        /// <summary>
        /// This overload method is used to log a message object and exception for warning category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(Type source, object message, Exception exception)
        {
            GetLogger(source).Warn(message, exception);
        }

        /// <summary>
        /// This method is used to log a message object and exception for error category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(object source, object message, Exception exception, string corelationId)
        {
            Error(source.GetType(), message, exception, ServerSideErrorSource,corelationId);
        }
        /// <summary>
        /// This method is used to log a message object and exception for error category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(object source, object message, Exception exception, string exceptionSource, string corelationId)
        {
            Error(source.GetType(), message, exception,exceptionSource,corelationId);
        }

       
        /// <summary>
        /// This overload method is used to log a message object and exception for error category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        private static void Error(Type source, object message, Exception exception, string exceptionSource, string corelationId)
        {
            // TODO - This needs to be corrected
            log4net.LogicalThreadContext.Properties["LogSourceCode"] = exceptionSource;
            log4net.LogicalThreadContext.Properties["CorelationId"] = corelationId;
            GetLogger(source).Error(
                string.Format(MessagePrefix, "") + message
                , exception);
        }
       
       
        /// <summary>
        /// This method is used to log a message object and exception for fatal error category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        /// <summary>
        /// This overload method is used to log a message object and exception for fatal error category
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(Type source, object message, Exception exception)
        {
            GetLogger(source).Fatal(message, exception);
        }

        /// <summary>
        /// Initialize Method
        /// </summary>
        public static void EnsureInitialized()
        {
            if (!_logInitialized)
            {
                Initialize();
            }
        }
        #endregion

    }
}
