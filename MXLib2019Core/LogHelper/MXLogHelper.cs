using System;
using System.IO;
using System.Reflection;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MXLib2019Core.LogHelper
{
    public class MXLogHelper
    {
        private static Logger _logger = null;
        private static readonly object _syncLock = new object();

        private MXLogHelper() { }

        /// <summary>
        /// Logger 物件是否已經建立
        /// </summary>
        public static bool IsReady
        {
            get
            {
                lock (_syncLock)
                {
                    return _logger != null;
                }
            }
        }

        /// <summary>
        /// 清除現有 Logger 物件以重建新的 Logger
        /// </summary>
        public static void Reset()
        {
            lock (_syncLock)
            {
                if (_logger != null)
                {
                    _logger = null;
                }
            }
        }

        /// <summary>
        /// 在程式執行檔所在目錄建立 log 目錄並產生 log file
        /// </summary>
        public static void CreateDefaultLogger()
        {
            // 如果已經建立Logger物件, 就不重複建立
            if (_logger != null)
            {
                return;
            }

            string codeBase = null;
            if (Assembly.GetEntryAssembly() != null)
            {
                codeBase = Assembly.GetEntryAssembly().CodeBase;
            }

            if (codeBase == null && Assembly.GetCallingAssembly() != null)
            {
                codeBase = Assembly.GetCallingAssembly().CodeBase;
            }

            if (codeBase == null && Assembly.GetExecutingAssembly() != null)
            {
                codeBase = Assembly.GetExecutingAssembly().CodeBase;
            }

            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string filePath = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path) + ".log";

            string logFileName = string.Format("{0}\\log\\{1}", filePath, fileName);

            CreateFileLogger(logFileName, true);
        }

        public static void CreateFileLogger(string logFileName, bool includeConsoleLog = false)
        {
            lock (_syncLock)
            {
                // 如果已經建立Logger物件, 就不重複建立
                if (_logger != null)
                {
                    return;
                }

                var config = new LoggingConfiguration();

                // FileTarget
                var fileTarget = new FileTarget("FileLogger");
                fileTarget.FileName = logFileName;
                fileTarget.Encoding = Encoding.Default;
                fileTarget.CreateDirs = true;
                fileTarget.Layout = "${longdate}|(${level:uppercase=true}) - ${message}";
                fileTarget.ArchiveOldFileOnStartup = true;
                fileTarget.ArchiveAboveSize = 10 * 1204;
                fileTarget.ArchiveEvery = FileArchivePeriod.Day;
                fileTarget.ArchiveDateFormat = "yyyy-MM-dd";
                fileTarget.ArchiveNumbering = ArchiveNumberingMode.DateAndSequence;
                string archiveFolder = Path.GetDirectoryName(logFileName) + @"\archives";
                string archiveFile = Path.GetFileNameWithoutExtension(logFileName) + "-{#}.log";
                fileTarget.ArchiveFileName = archiveFolder + @"\" + archiveFile;

                config.AddTarget(fileTarget);
                config.AddRuleForAllLevels(fileTarget);
                //

                // ConsoleTarget
                if (includeConsoleLog)
                {
                    var consoleTarget = new ColoredConsoleTarget("ConsoleLogger")
                    {
                        Layout = @"${date:format=MM-dd HH\:mm\:ss.fff} ${level} ${message}"
                    };

                    config.AddTarget(consoleTarget);
                    config.AddRuleForAllLevels(consoleTarget);
                }
                //

                LogManager.Configuration = config;
                _logger = LogManager.GetCurrentClassLogger();
            }
        }

        public static void CreateConsoleLogger()
        {
            lock (_syncLock)
            {
                // 如果已經建立Logger物件, 就不重複建立
                if (_logger != null)
                {
                    return;
                }

                var config = new LoggingConfiguration();
                var consoleTarget = new ColoredConsoleTarget("ConsoleLogger")
                {
                    Layout = @"${date:format=MM-dd HH\:mm\:ss.fff} ${level} ${message}"
                };

                config.AddTarget(consoleTarget);
                config.AddRuleForAllLevels(consoleTarget);
                LogManager.Configuration = config;
                _logger = LogManager.GetCurrentClassLogger();
            }
        }

        // Implement interface
        public static void Info(string message)
        {
            _logger.Info(message);
        }

        public static void Info(string message, Exception exception)
        {
            _logger.Info(exception, message);
        }

        public static void Warn(string message)
        {
            _logger.Warn(message);
        }

        public static void Warn(string message, Exception exception)
        {
            _logger.Warn(exception, message);
        }

        public static void Error(string message)
        {
            _logger.Error(message);
        }

        public static void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }
    }
}
