using System;
using System.IO;

namespace DictSharp.Utilities
{
    internal static class Logger
    {
        private static string _logFile;
        private static bool _isDebugMode = false;
        
        public static void Initialize(bool debugMode)
        {
            _isDebugMode = debugMode;
            
            if (_isDebugMode)
            {
                string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                _logFile = Path.Combine(logDir, $"debug_{timestamp}.log");
                
                WriteLog("=== DEBUG SESSION STARTED ===");
                WriteLog($"Application started at {DateTime.Now}");
                WriteLog($"Command line arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");
                WriteLog("=================================");
            }
        }
        
        public static void WriteLog(string message)
        {
            if (!_isDebugMode) return;
            
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
                File.AppendAllText(_logFile, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
        
        public static void WriteLog(string format, params object[] args)
        {
            if (!_isDebugMode) return;
            WriteLog(string.Format(format, args));
        }
        
        public static void WriteSeparator()
        {
            WriteLog(new string('-', 50));
        }
        
        public static void WriteException(Exception ex, string context = "")
        {
            if (!_isDebugMode) return;
            
            WriteLog($"=== EXCEPTION {(string.IsNullOrEmpty(context) ? "" : $"IN {context}")} ===");
            WriteLog($"Type: {ex.GetType().Name}");
            WriteLog($"Message: {ex.Message}");
            WriteLog($"StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                WriteLog($"Inner Exception: {ex.InnerException.Message}");
            }
            WriteLog("=================================");
        }
        
        public static string GetLogFilePath()
        {
            return _logFile;
        }
        
        public static bool IsDebugMode()
        {
            return _isDebugMode;
        }
    }
}