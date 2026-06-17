using System;
using System.Collections.Generic;
using System.Linq;
using DictSharp.Enums;
using DictSharp.Models;
using DictSharp.Services;
//using DictSharp.Utilities;

namespace DictSharp.Dictionaries
{
    internal class BilingualDictionary
    {
        private Dictionary<string, DictionaryEntry> _entries;
        private DictionaryType _type;
        private FileStorage _storage;
        
        public BilingualDictionary(DictionaryType type)
        {
            _type = type;
            _entries = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase);
            _storage = new FileStorage(type);
            LoadFromFile();
        }
        
        private void LoadFromFile()
        {
            var loaded = _storage.Load();
            if (loaded != null)
            {
                _entries = loaded;
            }
        }
        
        private void SaveToFile()
        {
            _storage.Save(_entries);
        }
        
        public void AddWord(string word, string translation, bool debugMode = false)
        {
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(translation))
            {
                /*if (debugMode) 
                {
                    Logger.WriteLog("Add Word - Empty word input detected");
                    DebugConsole.WriteError("Add Word - Empty word input detected");
                }*/
                throw new ArgumentException("Word and translation cannot be empty");
            }
            if (_entries.ContainsKey(word))
            {
                _entries[word].AddTranslation(translation);
            }
            else
            {
                _entries[word] = new DictionaryEntry(word, translation);
            }
            SaveToFile();
            Console.WriteLine($"Word '{word}' added/updated with translation '{translation}'");
        }
        
        public void ReplaceWord(string oldWord, string newWord)
        {
            if (!_entries.ContainsKey(oldWord))
            {
                Console.WriteLine($"\nWord '{oldWord}' not found");
                return;
            }
            
            var entry = _entries[oldWord];
            _entries.Remove(oldWord);
            entry.Word = newWord;
            _entries[newWord] = entry;
            SaveToFile();
            Console.WriteLine($"\nWord '{oldWord}' replaced with '{newWord}'");
        }
        
        public void ReplaceTranslation(string word, string oldTranslation, string newTranslation)
        {
            if (!_entries.ContainsKey(word))
            {
                Console.WriteLine($"\nWord '{word}' not found");
                return;
            }
            
            var entry = _entries[word];
            int index = entry.Translations.FindIndex(t => 
                t.Equals(oldTranslation, StringComparison.OrdinalIgnoreCase));
            
            if (index == -1)
            {
                Console.WriteLine($"\nTranslation '{oldTranslation}' not found for '{word}'");
                return;
            }
            
            entry.Translations[index] = newTranslation;
            SaveToFile();
            Console.WriteLine($"\nTranslation '{oldTranslation}' replaced with '{newTranslation}' for '{word}'");
        }
        
        public void DeleteWord(string word)
        {
            if (!_entries.ContainsKey(word))
            {
                Console.WriteLine($"\nWord '{word}' not found");
                return;
            }
            
            _entries.Remove(word);
            SaveToFile();
            Console.WriteLine($"\nWord '{word}' and all its translations deleted");
        }
        
        public void DeleteTranslation(string word, string translation)
        {
            if (!_entries.ContainsKey(word))
            {
                Console.WriteLine($"\nWord '{word}' not found");
                return;
            }
            
            var entry = _entries[word];
            if (!entry.RemoveTranslation(translation))
            {
                Console.WriteLine($"\nCannot delete last translation of '{word}'. Word would have no translations.");
                return;
            }
            
            SaveToFile();
            Console.WriteLine($"\nTranslation '{translation}' deleted from '{word}'");
        }
        
        public void Search(string word)
        {
            if (_entries.TryGetValue(word, out DictionaryEntry entry))
            {
                Console.WriteLine($"\n=== Translation for '{word}' ===");
                for (int i = 0; i < entry.Translations.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {entry.Translations[i]}");
                }
            }
            else
            {
                Console.WriteLine($"\nWord '{word}' not found in dictionary");
            }
        }
        
        public void ExportToFile(string word, string filename)
        {
            if (!_entries.TryGetValue(word, out DictionaryEntry entry))
            {
                Console.WriteLine($"\nWord '{word}' not found");
                return;
            }
            
            System.IO.File.WriteAllText(filename, entry.ToString());
            Console.WriteLine($"\nExported '{word}' to {filename}");
        }
        
        public void ShowAllWords()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("Dictionary is empty");
                return;
            }
            
            Console.WriteLine($"\n\t === All entries ({_entries.Count}) ===");
            foreach (var entry in _entries.Values.OrderBy(e => e.Word))
            {
                Console.WriteLine(entry);
            }
        }
        
        public string GetDictionaryTypeName()
        {
            return _type == DictionaryType.EnglishRussian 
                ? "English-Russian" 
                : "Russian-English";
        }
    }
}