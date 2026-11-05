using System;
using System.Collections.Generic;
using System.Linq;

namespace TextProcessor
{
    public static class TextService
    {
        public static List<Sentence> SortByWordCount(Text text) =>
            text.Sentences.OrderBy(s => s.WordCount).ToList();

        public static List<Sentence> SortByLength(Text text) =>
            text.Sentences.OrderBy(s => s.Length).ToList();

        public static List<string> FindWordsInQuestions(Text text, int length) =>
            text.Sentences
                .Where(s => s.IsQuestion)
                .SelectMany(s => s.Elements)
                .OfType<Word>()
                .Select(w => w.Value)
                .Where(w => w.Length == length)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

        public static void RemoveWordsStartingWithConsonant(Text text, int length)
        {
            string consonants = "bcdfghjklmnpqrstvwxyzбвгджзйклмнпрстфхцчшщBCDFGHJKLMNPQRSTVWXYZБВГДЖЗЙКЛМНПРСТФХЦЧШЩ";
            foreach (var sentence in text.Sentences)
            {
                sentence.Elements.RemoveAll(e => e is Word w &&
                                                 w.Value.Length == length &&
                                                 consonants.Contains(w.Value[0]));
            }
        }

        public static void ReplaceWordsInSentence(Text text, int sentenceIndex, int targetLength, string replacement)
        {
            if (sentenceIndex < 0 || sentenceIndex >= text.Sentences.Count) return;

            var sentence = text.Sentences[sentenceIndex];
            for (int i = 0; i < sentence.Elements.Count; i++)
            {
                if (sentence.Elements[i] is Word w && w.Value.Length == targetLength)
                {
                    sentence.Elements[i] = new Word(replacement);
                }
            }
        }

        public static void RemoveStopWords(Text text, HashSet<string> stopWords)
        {
            foreach (var sentence in text.Sentences)
            {
                sentence.Elements.RemoveAll(e => e is Word w && stopWords.Contains(w.Value.ToLower()));
            }
        }
    }
}
