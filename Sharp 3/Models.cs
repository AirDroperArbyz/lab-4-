using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TextProcessor
{
    public interface ISentenceElement
    {
        string Value { get; set; }
    }

    public class Word : ISentenceElement
    {
        [XmlText]
        public string Value { get; set; }

        public Word() { }
        public Word(string value) => Value = value;

        public override string ToString() => Value;
    }

    public class Punctuation : ISentenceElement
    {
        [XmlText]
        public string Value { get; set; }

        public Punctuation() { }
        public Punctuation(string value) => Value = value;

        public override string ToString() => Value;
    }

    public class Sentence
    {
        [XmlElement("word", typeof(Word))]
        [XmlElement("punctuation", typeof(Punctuation))]
        public List<ISentenceElement> Elements { get; set; } = new List<ISentenceElement>();

        [XmlIgnore]
        public int WordCount => Elements.Count(e => e is Word);

        [XmlIgnore]
        public int Length => ToString().Length;

        [XmlIgnore]
        public bool IsQuestion => Elements.LastOrDefault()?.Value == "?";

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < Elements.Count; i++)
            {
                if (i > 0 && Elements[i] is Word) sb.Append(" ");
                sb.Append(Elements[i].Value);
            }
            return sb.ToString();
        }
    }

    [XmlRoot("text")]
    public class Text
    {
        [XmlElement("sentence")]
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();

        public override string ToString()
        {
            return string.Join(" ", Sentences);
        }
    }
}
