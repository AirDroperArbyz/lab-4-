using System;
using System.Text.RegularExpressions;

namespace TextProcessor
{
    public static class TextParser
    {
        public static Text Parse(string rawText)
        {
            var textObj = new Text();
            string[] rawSentences = Regex.Split(rawText, @"(?<=[.!?])\s+");

            foreach (var rawSentence in rawSentences)
            {
                if (string.IsNullOrWhiteSpace(rawSentence)) continue;

                var sentence = new Sentence();
                var tokens = Regex.Matches(rawSentence, @"[a-zA-Zа-яА-ЯёЁ0-9-]+|[.,!?;:]");

                foreach (Match token in tokens)
                {
                    string value = token.Value;
                    if (Regex.IsMatch(value, @"^[.,!?;:]$"))
                    {
                        sentence.Elements.Add(new Punctuation(value));
                    }
                    else
                    {
                        sentence.Elements.Add(new Word(value));
                    }
                }

                if (sentence.Elements.Count > 0)
                {
                    textObj.Sentences.Add(sentence);
                }
            }

            return textObj;
        }
    }
}
