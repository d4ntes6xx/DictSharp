using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DictSharp.Enums;
using DictSharp.Models;

namespace DictSharp.Services
{
    internal class FileStorage
    {
        private string _filePath;
        
        public FileStorage(DictionaryType type)
        {
            string fileName = type == DictionaryType.EnglishRussian 
                ? "en_ru_dictionary.txt" 
                : "ru_en_dictionary.txt";
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }
        
        public void Save(Dictionary<string, DictionaryEntry> dictionary)
        {
            try
            {
                var lines = new List<string>();
                foreach (var entry in dictionary.Values)
                {
                    string translationsLine = string.Join("|", entry.Translations);
                    lines.Add($"{entry.Word}:{translationsLine}");
                }
                File.WriteAllLines(_filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving dictionary: {ex.Message}");
            }
        }
        
        public Dictionary<string, DictionaryEntry> Load()
        {
            var dictionary = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase);
            
            if (!File.Exists(_filePath))
            {
                return dictionary;
            }
            
            try
            {
                var lines = File.ReadAllLines(_filePath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    
                    int separatorIndex = line.IndexOf(':');
                    if (separatorIndex == -1)
                        continue;
                    
                    string word = line.Substring(0, separatorIndex);
                    string translationsPart = line.Substring(separatorIndex + 1);
                    var translations = translationsPart.Split('|').ToList();
                    
                    dictionary[word] = new DictionaryEntry(word, translations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dictionary: {ex.Message}");
            }
            
            return dictionary;
        }
    }
}