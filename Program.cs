using System;
using System.Linq;
using System.Text;
using System.Threading;
using DictSharp.Dictionaries;
using DictSharp.Enums;
using DictSharp.Utilities;

namespace DictSharp
{
    internal class Program
    {
        //private static bool _debugMode = false;
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Dictionary Application ===");
                Console.WriteLine("Select dictionary type:");
                Console.WriteLine("1. English-Russian");
                Console.WriteLine("2. Russian-English");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");
                
                string input = Console.ReadLine();
                DictionaryType type;

                try
                {
                    if (input == "0")
                    {
                        Console.WriteLine("\nGoodbye!");
                        Thread.Sleep(3000);
                        Console.Clear();
                        Environment.Exit(0);
                    }
                    if (input == "1")
                    {
                        type = DictionaryType.EnglishRussian;
                    }
                    else if (input == "2")
                    {
                        type = DictionaryType.RussianEnglish;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid dictionary type selection: '{input}'. Please enter 1 or 2.");
                    }
                    
                    BilingualDictionary dictionary = new BilingualDictionary(type);
                    bool backToMain = MenuHelper.ShowMainMenu(dictionary);
                    
                    if (!backToMain)
                    {
                        Thread.Sleep(3000);
                        Console.Clear();
                        Environment.Exit(0);
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\n[!] Error: {ex.Message}");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[!] Unexpected error: {ex.Message}");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        
        static void Main(string[] args)
        {   
            // Console.OutputEncoding = Encoding.UTF8;
            // Console.InputEncoding = Encoding.UTF8;

            // _debugMode = args.Contains("--debug");
            
            // Logger.Initialize(_debugMode);
            
            // if (_debugMode)
            // {
            //     Thread.Sleep(100);
            //     DebugConsole.CreateDebugConsole();
            //     Thread.Sleep(100);
                
            //     DebugConsole.WriteSeparator();
            //     DebugConsole.WriteLine("DEBUG MODE ACTIVATED");
            //     DebugConsole.WriteLine($"Working directory: {Environment.CurrentDirectory}");
            //     DebugConsole.WriteLine($"Log file: {Logger.GetLogFilePath()}");
            //     DebugConsole.WriteSeparator();
            // }
            
            MainMenu();
            
            /*if (!_debugMode)
            {
                DebugConsole.CloseDebugConsole();
            }*/
        }
    }
}