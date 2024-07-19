namespace TopWords
{
    public class Program
    {
        public static string GetAllTextFromFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return new string(sr.ReadToEnd().Where(c => !char.IsPunctuation(c)).ToArray()).ToLower();
            }
        }
        public static List<string> StrToListOfWords(string text)
        {
            var list = new List<string>(text.Split(' ', '\n', '\t'));
            list.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            return list;
        }
        public static Dictionary<string, int> CreateDictionary(IList<string> list)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < list.Count; ++i)
            {
                if (!dict.TryAdd(list[i], 1))
                    dict[list[i]]++;
            }
            dict.Remove(string.Empty);
            return dict;
        }
        public static Dictionary<string, int> GetMaxWordsFromDictionary(IDictionary<string, int> dict, int wordsCount)
        {
            Dictionary<string, int> topWords = new Dictionary<string, int>();
            string str = string.Empty;
            KeyValuePair<string, int> keyValuePair;

            for (int i = 0; i < wordsCount; ++i)
            {
                keyValuePair = dict.MaxBy(x => x.Value);
                topWords.Add(keyValuePair.Key, keyValuePair.Value);
                dict.Remove(keyValuePair.Key);
            }
            return topWords;
        }
        public static void DisplayCollection<T>(IEnumerable<T> values)
        {
            foreach (var word in values)
                Console.WriteLine(word);
            return;
        }
        static void Main(string[] args)
        {
            int wordsCount;
            Console.WriteLine("Это программа для поиска наиболее встречаемых слов в тексте. Работает с текстовыми файлами" +
                "\nПрограмма посчитает количество слов и выведет самые популярные из них" +
                "\nСколько слов вывести в отчет?");
            do
            {
                wordsCount = 0;
                Console.WriteLine("Количество слов: ");
                if (Int32.TryParse(Console.ReadLine(), out wordsCount) == false || wordsCount <= 0)
                    Console.WriteLine("Не является числом или неверное значение.");
                else break;
            } while (true);

            string path = "";
            do
            {
                Console.WriteLine($"Путь к файлу, в котором {wordsCount} самых встречаемых слов будет выведено: ");
                path = Console.ReadLine();
                if (File.Exists(path)) break;
                else Console.WriteLine("Файл не найден. Попробуйте еще раз.");
            } while (true);

            string text = GetAllTextFromFile(path);

            List<string> list = StrToListOfWords(text);

            Dictionary<string, int> dict = CreateDictionary(list);

            Dictionary<string, int> topWords = GetMaxWordsFromDictionary(dict, wordsCount);

            DisplayCollection(topWords);
            Console.ReadKey();
        }
    }
}
