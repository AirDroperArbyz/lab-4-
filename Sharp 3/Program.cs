using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TextProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string sampleText = "Mary had a little lamb. А вы любите программирование на языке C#? " +
                                "Это очень интересный и полезный процесс! Кот сидел на красивом ковре. " +
                                "Who is Aesop and what did he write?";

            Console.WriteLine("--- ИСХОДНЫЙ ТЕКСТ ---");
            Console.WriteLine(sampleText);

            Text textObj = TextParser.Parse(sampleText);

            Console.WriteLine("\n1. Сортировка предложений по количеству слов:");
            foreach (var s in TextService.SortByWordCount(textObj))
                Console.WriteLine($"[{s.WordCount} слов]: {s}");

            Console.WriteLine("\n2. Сортировка предложений по общей длине текста:");
            foreach (var s in TextService.SortByLength(textObj))
                Console.WriteLine($"[{s.Length} симв.]: {s}");

            Console.WriteLine("\n3. Слова длиной 2 символа в вопросительных предложениях:");
            var questionWords = TextService.FindWordsInQuestions(textObj, 2);
            Console.WriteLine(string.Join(", ", questionWords));

            Console.WriteLine("\n5. В 0-м предложении заменяем слова длиной 4 ('lamb') на 'WOLF':");
            TextService.ReplaceWordsInSentence(textObj, 0, 4, "WOLF");
            Console.WriteLine(textObj.Sentences[0]);

            Console.WriteLine("\n4. Удаляем слова длиной 3 символа, начинающиеся с СОГЛАСНОЙ:");
            TextService.RemoveWordsStartingWithConsonant(textObj, 3);
            Console.WriteLine(textObj);

            Console.WriteLine("\n6. Удаление стоп-слов:");
            var stopWordsMock = new HashSet<string> { "на", "и", "а", "вы", "he", "is", "and" };
            TextService.RemoveStopWords(textObj, stopWordsMock);
            Console.WriteLine(textObj);

            Console.WriteLine("\n7. Экспорт текстового объекта в XML...");
            string xmlPath = "output_text.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(Text));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            using (StreamWriter writer = new StreamWriter(xmlPath, false, System.Text.Encoding.UTF8))
            {
                serializer.Serialize(writer, textObj, namespaces);
            }

            Console.WriteLine($"Успешно! XML-файл сохранен по пути: {Path.GetFullPath(xmlPath)}");
            Console.WriteLine("\nСодержимое сгенерированного XML:");
            Console.WriteLine(File.ReadAllText(xmlPath));

            Console.ReadKey();
        }
    }
}
