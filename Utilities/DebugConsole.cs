using System;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace DictSharp.Utilities
{
    internal static class DebugConsole
    {
        private static Process _debugProcess;
        private static StreamWriter _debugWriter;
        private static string _debugLogFile;
        
        public static void CreateDebugConsole()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                
                _debugLogFile = Path.Combine(Path.GetTempPath(), $"dictsharp_debug_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                
                string batchFile = Path.Combine(Path.GetTempPath(), "debug_viewer.bat");
                File.WriteAllText(batchFile, 
                    $"@echo off\r\n" +
                    $"chcp 65001 > nul\r\n" +
                    $"title DEBUG CONSOLE - Dictionary App\r\n" +
                    $"color 0F\r\n" +
                    $"echo ╔═══════════════════════════════════════════╗\r\n" +
                    $"echo ║           DEBUG CONSOLE ACTIVE            ║\r\n" +
                    $"echo ╚═══════════════════════════════════════════╝\r\n" +
                    $"echo Started at: {DateTime.Now}\r\n" +
                    $"echo Log file: {_debugLogFile}\r\n" +
                    $"echo {new string('═', 40)}\r\n" +
                    $"echo.\r\n" +
                    $"type \"{_debugLogFile}\" 2>nul\r\n" +
                    $":loop\r\n" +
                    $"timeout /t 1 /nobreak >nul\r\n" +
                    $"cls\r\n" +
                    $"type \"{_debugLogFile}\" 2>nul\r\n" +
                    $"goto loop\r\n"
                );
                
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start \"Debug Console\" cmd.exe /c \"{batchFile}\"",
                    UseShellExecute = false,
                    CreateNoWindow = false
                };
                
                _debugProcess = Process.Start(startInfo);
                
                File.WriteAllText(_debugLogFile, "");
                _debugWriter = new StreamWriter(_debugLogFile, true) { AutoFlush = true };
                
                Logger.WriteLog($"Debug console created. Log file: {_debugLogFile}");
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Failed to create debug console: {ex.Message}");
            }
        }
        
        public static void WriteLine(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            if (_debugWriter != null)
            {
                string timestamp = $"[{DateTime.Now:HH:mm:ss.fff}]";
                string coloredMsg = $"{timestamp} {message}";
                
                _debugWriter.WriteLine(coloredMsg);
                Logger.WriteLog(message);
            }
        }
        
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args), ConsoleColor.Gray);
        }
        
        public static void WriteError(string message)
        {
            WriteLine($"[ERROR] {message}", ConsoleColor.Red);
        }
        
        public static void WriteWarning(string message)
        {
            WriteLine($"[WARN] {message}", ConsoleColor.Yellow);
        }
        
        public static void WriteSuccess(string message)
        {
            WriteLine($"[OK] {message}", ConsoleColor.Green);
        }
        
        public static void WriteSeparator()
        {
            WriteLine(new string('─', 50), ConsoleColor.DarkGray);
        }
    }
}