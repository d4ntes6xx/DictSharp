using System;
using System.Collections.Generic;
using System.Linq;

namespace DictSharp.Models
{
    internal class DictionaryEntry : IComparable<DictionaryEntry>
    {
        public string Word { get; set; }
        public List<string> Translations { get; set; }
        
        public DictionaryEntry(string word, string translation)
        {
            Word = word;
            Translations = new List<string> { translation };
        }
        
        public DictionaryEntry(string word, List<string> translations)
        {
            Word = word;
            Translations = translations ?? new List<string>();
        }
        
        public void AddTranslation(string translation)
        {
            if (!Translations.Contains(translation))
                Translations.Add(translation);
        }
        
        public bool RemoveTranslation(string translation)
        {
            if (Translations.Count <= 1)
                return false;
            return Translations.Remove(translation);
        }
        
        public override string ToString()
        {
            return $"{Word}: {string.Join(", ", Translations)}";
        }
        
        public int CompareTo(DictionaryEntry other)
        {
            if (other == null) return 1;
            return string.Compare(Word, other.Word, StringComparison.OrdinalIgnoreCase);
        }
    }
}