using System;
using System.Collections.Generic;
using DictSharp.Dictionaries;

namespace DictSharp.Utilities
{
    internal static class MenuHelper
    {
        private static readonly string[] DictionaryArt = new string[]
        {
            "         ,..........   ..........,         ",
            "     ,..,'          '.'          ',..,     ",
            "    ,' ,'            :            ', ',    ",
            "   ,' ,'             :             ', ',   ",
            "  ,' ,'              :              ', ',  ",
            " ,' ,'............., : ,.............', ', ",
            ",'  '............   '.'   ............'  ',", 
            " '''''''''''''''''';''';''''''''''''''''''''",
            "                    '''                    "
        };
        
        private static int GetActualWidth(string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            
            int width = 0;
            foreach (char c in text)
            {
                if (c == '\t')
                {
                    width = ((width / 4) + 1) * 4;
                }
                else
                {
                    width++;
                }
            }
            return width;
        }
        
        private static string StripTabsForDisplay(string text)
        {
            return text.Replace("\t", "    ");
        }
        
        private static void CenterText(string text, int width)
        {
            string displayText = StripTabsForDisplay(text);
            int padding = (width - displayText.Length) / 2;
            if (padding < 0) padding = 0;
            Console.Write(new string(' ', padding));
            Console.WriteLine(displayText);
        }
        
        private static void DrawAsciiArt(int boxWidth)
        {
            foreach (string line in DictionaryArt)
            {
                CenterText(line, boxWidth);
            }
        }
        
        private static void DrawBox(string title, List<string> items, out int width, bool showArt = true)
        {
            string displayTitle = StripTabsForDisplay(title);
            List<string> displayItems = new List<string>();
            
            int maxLength = displayTitle.Length;
            foreach (var item in items)
            {
                string displayItem = StripTabsForDisplay(item);
                displayItems.Add(displayItem);
                if (displayItem.Length > maxLength)
                    maxLength = displayItem.Length;
            }
            
            width = maxLength + 4;
            if (width < 60) width = 60;
            
            if (showArt)
            {
                DrawAsciiArt(width);
                Console.WriteLine();
            }
            
            string topBorder = "╭" + new string('─', width - 2) + "╮";
            string bottomBorder = "╰" + new string('─', width - 2) + "╯";
            
            Console.WriteLine(topBorder);
            Console.WriteLine($"│ {displayTitle.PadRight(width - 4)} │");
            Console.WriteLine($"├{new string('─', width - 2)}┤");
            
            foreach (var displayItem in displayItems)
            {
                Console.WriteLine($"│ {displayItem.PadRight(width - 4)} │");
            }
            
            Console.WriteLine(bottomBorder);
        }
        
        private static void DrawSimpleBox(string title, out int width, bool showArt = false)
        {
            string displayTitle = StripTabsForDisplay(title);
            width = displayTitle.Length + 4;
            if (width < 40) width = 40;
            
            if (showArt)
            {
                DrawAsciiArt(width);
                Console.WriteLine();
            }
            
            string topBorder = "╭" + new string('─', width - 2) + "╮";
            string bottomBorder = "╰" + new string('─', width - 2) + "╯";
            
            Console.WriteLine(topBorder);
            Console.WriteLine($"│ {displayTitle.PadRight(width - 4)} │");
            Console.WriteLine(bottomBorder);
        }
        
        private static void PressAnyKey()
        {
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
        }
        
        public static bool ShowMainMenu(BilingualDictionary dictionary, bool debugMode = false)
        {
            while (true)
            {
                Console.Clear();
                
                var menuItems = new List<string>
                {
                    "               1. Add word and translation",
                    "               2. Replace word",
                    "               3. Replace translation",
                    "               4. Delete word",
                    "               5. Delete translation",
                    "               6. Search translation",
                    "               7. Export word to file",
                    "               8. Show all words",
                    "               9. Exit to Main Menu",
                    "               0. Exit"
                };
                
                string title = $"           === {dictionary.GetDictionaryTypeName()} Dictionary ===";
                DrawBox(title, menuItems, out int width, showArt: true);
                
                // if (debugMode)
                // {
                //     Console.ForegroundColor = ConsoleColor.DarkGray;
                //     Console.WriteLine($"\n   [DEBUG] Log file: {Logger.GetLogFilePath()}");
                //     Console.ResetColor();
                // }
                
                Console.Write("\n\t\t    Your choice: ");
                string choice = Console.ReadLine();
/*
                if (debugMode)
                {
                    Logger.WriteLog($"User choice in dictionary menu: '{choice}'");
                }
                */
                switch (choice)
                {
                    case "1":
                        /*if (debugMode)
                        {
                            Logger.WriteLog("Entering Add Word menu");
                        }*/
                        AddWordMenu(dictionary);
                        break;
                    case "2":
                        /*if (debugMode)
                        {
                            Logger.WriteLog("Entering Replace Word menu");
                        }*/
                        ReplaceWordMenu(dictionary);
                        break;
                    case "3":
                        /*
                        Logger.WriteLog("Entering Replace Translation menu");*/
                        ReplaceTranslationMenu(dictionary);
                        break;
                    case "4":
                        /*if (debugMode) 
                        { 
                        Logger.WriteLog("Entering Delete Word menu");
                        
                        }*/
                        DeleteWordMenu(dictionary);
                        break;
                    case "5":
                        // if (debugMode) 
                        // { 
                        // Logger.WriteLog("Entering Delete Translation menu");
                        // }
                        DeleteTranslationMenu(dictionary);
                        break;
                    case "6":
                        // if (debugMode) 
                        // { 
                        //     Logger.WriteLog("Entering Search menu");
                        // }
                        SearchMenu(dictionary);
                        break;
                    case "7":
                        // if (debugMode) 
                        // { 
                        //     Logger.WriteLog("Entering Export menu");
                        // }
                        ExportMenu(dictionary);
                        break;
                    case "8":
                        // if (debugMode) 
                        // { 
                        //     Logger.WriteLog("Viewing all words"); 
                        // }
                        ShowAllMenu(dictionary);
                        break;
                    case "9":
                        // if (debugMode)
                        // {
                        //     Logger.WriteLog("User chose to return to main menu (change dictionary type)");    
                        // }
                        return true;
                    case "0":
                        // if (debugMode)
                        // {
                        //     Logger.WriteLog("User chose to exit application");
                        // }
                        Console.Clear();
                        DrawSimpleBox("             Goodbye! ", out _, showArt: false);
                        return false;
                    default:
                        Console.WriteLine("\n\t\t  [!] Invalid choice");
                        PressAnyKey();
                        break;
                }
            }
        }
        
        static void AddWordMenu(BilingualDictionary dictionary/*, bool debugMode = false*/)
        {
            Console.Clear();
            DrawSimpleBox("             Add Word ", out int width, showArt: false);
            
            // if (debugMode)
            // {
            //     Logger.WriteLog($"Add Word - Waiting for word input");
            //     DebugConsole.WriteLine($"Add Word - Waiting for word input");
            // }

            Console.Write($"\n   Enter word: ");
            string word = Console.ReadLine();

            // if (debugMode)
            // {
            //     Logger.WriteLog($"Add Word - Word input: '{word ?? "null"}'");
            //     DebugConsole.WriteLine($"Add Word - Word input: '{word ?? "null"}'");
            // }

            Console.Write($"   Enter translation: ");
            string translation = Console.ReadLine();

            // if (debugMode)
            // {
            //     Logger.WriteLog($"Add Word - Translation input: '{translation ?? "null"}'");
            //     DebugConsole.WriteLine($"Add Word - Translation input: '{translation ?? "null"}'");
            // }
            
            dictionary.AddWord(word, translation);
            // if (debugMode)
            // { 
            //     Logger.WriteLog($"Successfully added word '{word}' with translation '{translation}'");
            //     DebugConsole.WriteSuccess($"Successfully added word '{word}' with translation '{translation}'");
            // }
            PressAnyKey();
        }
        
        static void ReplaceWordMenu(BilingualDictionary dictionary)
        {
            Console.Clear();
            DrawSimpleBox("           Replace Word ", out int width, showArt: false);
            
            Console.Write($"\n   Enter word to replace: ");
            string oldWord = Console.ReadLine();
            Console.Write($"   Enter new word: ");
            string newWord = Console.ReadLine();
            
            dictionary.ReplaceWord(oldWord, newWord);
            PressAnyKey();
        }
        
        static void ReplaceTranslationMenu(BilingualDictionary dictionary /*, bool debugMode = false*/)
        {
            Console.Clear();
            DrawSimpleBox("         Replace Translation ", out int width, showArt: false);
            
            Console.Write($"\n   Enter word: ");
            string word = Console.ReadLine();
            Console.Write($"   Enter old translation: ");
            string oldTranslation = Console.ReadLine();
            Console.Write($"   Enter new translation: ");
            string newTranslation = Console.ReadLine();
            
            dictionary.ReplaceTranslation(word, oldTranslation, newTranslation);
            PressAnyKey();
        }
        
        static void DeleteWordMenu(BilingualDictionary dictionary /*, bool debugMode = false*/)
        {
            Console.Clear();
            DrawSimpleBox("            Delete Word ", out int width, showArt: false);
            
            Console.Write($"\n   Enter word to delete: ");
            string word = Console.ReadLine();
            
            dictionary.DeleteWord(word);
            PressAnyKey();
        }
        
        static void DeleteTranslationMenu(BilingualDictionary dictionary /*, bool debugMode = false*/)
        {
            Console.Clear();
            DrawSimpleBox("         Delete Translation ", out int width, showArt: false);
            
            Console.Write($"\n   Enter word: ");
            string word = Console.ReadLine();
            Console.Write($"   Enter translation to delete: ");
            string translation = Console.ReadLine();
            
            dictionary.DeleteTranslation(word, translation);
            PressAnyKey();
        }
        
        static void SearchMenu(BilingualDictionary dictionary/*, bool debugMode = false*/)
        {
            Console.Clear();
            DrawSimpleBox("         Search Translation ", out int width, showArt: false);
            
            Console.Write($"\n   Enter word to search: ");
            string word = Console.ReadLine();
            
            dictionary.Search(word);
            PressAnyKey();
        }
        
        static void ExportMenu(BilingualDictionary dictionary/*, bool debugMode = false*/)
        {
            Console.Clear();
            DrawSimpleBox("            Export Word ", out int width, showArt: false);
            
            Console.Write($"\n   Enter word to export: ");
            string word = Console.ReadLine();
            Console.Write($"   Enter filename (e.g., export.txt)");
            Console.Write("\n   > ");
            string filename = Console.ReadLine();
            
            dictionary.ExportToFile(word, filename);
            PressAnyKey();
        }
        
        static void ShowAllMenu(BilingualDictionary dictionary/*, bool debugMode = false*/)
        {
            Console.Clear();
            DrawSimpleBox("         Dictionary Contents ", out int width, showArt: false);
            
            dictionary.ShowAllWords();
            PressAnyKey();
        }
    }
}