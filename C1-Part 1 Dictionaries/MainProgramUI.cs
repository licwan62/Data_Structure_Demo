using System.Diagnostics;
using System.Text;
using static System.Console;

namespace C1_Part_1_Dictionaries;

internal class MainProgramUI
{
    static FileInfo[] GetFiles()
    {
        FileInfo[] orderFiles = new DirectoryInfo(@"..\..\..\ordered").GetFiles().OrderBy(f => f.Length).ToArray();
        FileInfo[] randomFiles = new DirectoryInfo(@"..\..\..\random").GetFiles().OrderBy(f => f.Length).ToArray();
        FileInfo[] files = orderFiles.Concat(randomFiles).ToArray();
        return files;
    }
    static (WordDictionaryDS[] word_DS, TimeDictionaryDS time_DS) GetAllFiles()
    {
        FileInfo[] files = GetFiles();
        WordDictionaryDS[] dictionariesDS = new WordDictionaryDS[files.Length];

        // dictionary to record time comsumed by each insertion
        TimeDictionaryDS timeInsertionDS = new();
        Stopwatch stopwatch = new Stopwatch();
        // use streamReader to split lines and load not null line in dictionary
        int index = 0;
        foreach (FileInfo file in files)
        {
            WordDictionaryDS word_DS = new(file.FullName);// dictionary to load words and lengths
            StreamReader sr = file.OpenText();
            string line;
            stopwatch = new Stopwatch();
            stopwatch.Start();// start record insertion
            // insert each line from text file into dictionary
            while ((line = sr.ReadLine()) != null)
                word_DS.Insert(line);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            // Util is addtional class funcion returns designed name and type of files
            // put entry with file type, inserted words sum, elapsed time into dictionary of time record
            timeInsertionDS.Insert(word_DS.Type, word_DS.Number, timeSpan);
            dictionariesDS[index] = word_DS;
            index++;
            WriteLine($"Dictionary of {word_DS.Name} is successfully loaded");
        }
        WriteLine($"Total {files.Length} flies successfully Loaded");
        WriteLine("----------------------------------------------------\n");
        return (dictionariesDS, timeInsertionDS);
    }
    static void ReportTimeComplexity()
    {
        // 1 put all text files into array of word dictionary
        // Tuple is used to be assigned two objects
        Tuple<WordDictionaryDS[], TimeDictionaryDS> tuple = GetAllFiles()
            .ToTuple<WordDictionaryDS[], TimeDictionaryDS>();
        WordDictionaryDS[] word_DS_array = tuple.Item1;
        TimeDictionaryDS time_insertionDS = tuple.Item2;
        // 2 show time dictionary for each dictionary loaded
        Write("Press any key to Print time table for each dictionary insertion...");
        ReadKey();
        WriteLine(time_insertionDS.Print());
    }
    static (FileInfo, WordDictionaryDS) GetFile(int index)
    {
        FileInfo[] files = GetFiles();
        FileInfo file = files[index];
        WordDictionaryDS wordDS = new WordDictionaryDS(file.FullName);
        if (File.Exists(file.FullName))
        {
            StreamReader sr = new StreamReader(file.FullName);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                wordDS.Insert(line);
            }
        }
        return (file, wordDS);
    }
    static void Menu_function(WordDictionaryDS wordDS)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("== Function Menu ==");
        sb.AppendLine("0 - Insert word");
        sb.AppendLine("1 - Find word");
        sb.AppendLine("2 - Delete word");
        sb.AppendLine("3 - Print");
        sb.AppendLine("4 - Return");
        WriteLine(sb.ToString());
        while (true)
        {
            Write("Enter number: ");
            string input = ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                WriteLine("-Empty input");
            }
            else if (int.TryParse(input, out int value))
            {
                switch (value)
                {
                    case 0:
                        Insert(wordDS); break;
                    case 1:
                        Find(wordDS); break;
                    case 2:
                        Delete(wordDS); break;
                    case 3:
                        Print(wordDS); break;
                    case 4:
                        Write("Really to return to Load menu? [Enter Y to continue] : ");
                        if (ReadLine().ToLower() == "y")
                            Menu_loading();
                        break;

                }
            }
        }
    }
    static void Menu_loading()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("== Choose the file to load in ==");
        int words = 1000;
        for (int i = 0; i < 22; i++)
        {
            if (i < 11)
                sb.AppendLine($"{i} - Random {words}-words.txt");
            else
                sb.AppendLine($"{i} - Sequential {words}-words.txt");
            if (words == 1000)
            {
                words += 4000;
            }
            else if (words == 50000)
            {
                words = 1000;
            }
            else
            {
                words += 5000;
            }
        }
        sb.AppendLine("22 - Return to Main Menu");
        WriteLine(sb.ToString());
        while (true)
        {
            Write("Enter number: ");
            string input = ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                continue;
            }
            else if (int.TryParse(input, out int value))
            {
                if (value < 0 || value > 22)
                {
                    WriteLine("-Invalid number");
                }
                else if (value == 22)
                {
                    Write("Really to return to main menu? [Enter Y to continue] : ");
                    if (ReadLine().ToLower() == "y")
                    {
                        WriteLine("-Returned");
                        Menu_main();
                    }
                    else
                    {
                        WriteLine("-Canceled");
                    }
                }
                else
                {
                    WriteLine("-Loading");
                    Tuple<FileInfo, WordDictionaryDS> tuple = GetFile(value).ToTuple();
                    FileInfo file = tuple.Item1;
                    WordDictionaryDS wordDS = tuple.Item2;
                    if (file != null)
                    {
                        WriteLine($"{file.Name} is loaded");
                        Menu_function(wordDS);
                    }
                }
            }
            else
            {
                WriteLine("-Invalid input, enter command number");
            }
        }
    }
    /// <summary>
    /// Menu_main -> Menu_loading -> Menu_function
    /// </summary>
    static void Menu_main()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("== Main Menu ==");
        sb.AppendLine("0 - Load and manage a text file");
        sb.AppendLine("1 - Report time complexity of all files");
        sb.AppendLine("2 - Exit");
        WriteLine(sb.ToString());
        while (true)
        {
            Write("Enter number: ");
            string input = ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                WriteLine("-Empty input");
                continue;
            }
            else if (int.TryParse(input, out int value))
            {
                switch (value)
                {
                    case 0:
                        Write("Really to load file? [Enter Y to continue] : ");
                        if (ReadLine().ToLower() == "y")
                        {
                            WriteLine("-Loading");
                            Menu_loading();
                        }
                        else WriteLine("-Canceled");
                        break;
                    case 1:
                        Write("Really to report time complexity? This operation will load all the files exist [Enter Y to continue] ");
                        if (ReadLine().ToLower() == "y")
                        {
                            WriteLine("-Operating");
                            ReportTimeComplexity();
                        }
                        else WriteLine("-Canceled");
                        break;
                    case 2:
                        Write("Really to exit? [Enter Y to continue] : ");
                        if (ReadLine().ToLower() == "y")
                        {
                            WriteLine("-Exit");
                            return;
                        }
                        else
                        {
                            WriteLine("-Canceled");
                        }
                        break;
                    default: WriteLine("-Invalid command number"); break;
                }
            }
            else
            {
                WriteLine("-Invalid input, enter command number");
            }
        }
    }
    static void Insert(WordDictionaryDS wordDS)
    {
        Write("Really to insert word into selected file? [Enter Y to continue] : ");
        if (ReadLine().ToLower() == "y")
        {
            while (true)
            {
                Write("Enter the word to be insert: ");
                string insertString = ReadLine();
                if (string.IsNullOrEmpty(insertString))
                {
                    WriteLine("Empty input");
                }
                else
                {
                    Write($"Really to insert \"{insertString}\"? [Enter Y to continue] : ");
                    if (ReadLine().ToLower() == "y")
                    {
                        WriteLine(wordDS.Insert(insertString));
                        Menu_function(wordDS);
                        break;
                    }
                    else
                    {
                        WriteLine("-Canceled");
                    }
                }
            }
        }
        else
        {
            WriteLine("-Canceled");
        }
    }
    static void Find(WordDictionaryDS wordDS)
    {
        Write("Really to find word in selected file? [Enter Y to continue] : ");
        if (ReadLine().ToLower() == "y")
        {
            while (true)
            {
                Write("Enter the word to search: ");
                string searchString = ReadLine();
                if (string.IsNullOrEmpty(searchString))
                {
                    WriteLine("Empty input");
                }
                else
                {
                    Write($"Really to search \"{searchString}\"? [Enter Y to continue] : ");
                    if (ReadLine().ToLower() == "y")
                    {
                        WriteLine(wordDS.Find(searchString));
                        Menu_function(wordDS);
                        break;
                    }
                    else
                    {
                        WriteLine("-Canceled");
                    }
                }
            }
        }
        else
        {
            WriteLine("-Canceled");
        }
    }
    static void Delete(WordDictionaryDS wordDS)
    {
        Write("Really to remove word from selected file? [Enter Y to continue] : ");
        if (ReadLine().ToLower() == "y")
        {
            while (true)
            {
                Write("Enter the word to be removed: ");
                string deleteString = ReadLine();
                if (string.IsNullOrEmpty(deleteString))
                {
                    WriteLine("Empty input");
                }
                else
                {
                    WriteLine($"Really to search \"{deleteString}\"? [Enter Y to continue] : ");
                    if (ReadLine().ToLower() == "y")
                    {
                        WriteLine(wordDS.Delete(deleteString));
                        Menu_function(wordDS);
                        break;
                    }
                    else
                    {
                        WriteLine("-Canceled");
                    }
                }
            }
        }
        else
        {
            WriteLine("-Canceled");
        }
    }
    static void Print(WordDictionaryDS wordDS)
    {
        Write("Really to print selected file? [Enter Y to continue] : ");
        if (ReadLine().ToLower() == "y")
        {
            Write(wordDS.Print());
            Menu_function(wordDS);
        }
        else
        {
            WriteLine("-Canceled");
        }
    }
    static void Main(string[] args)
    {
        Menu_main();

        /*TimeDictionaryDS time_findDS = new();
        string test_word = "nosuchword";
        Stopwatch stopwatch = new Stopwatch();
        WriteLine("[ Find the word in each dictionary then record their time during finding ]");
        WriteLine($"Using word: {test_word}");
        Write("Press any key to continue...");
        ReadKey();
        foreach (WordDictionaryDS word_DS in word_DS_array)
        {
            stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            Find(word_DS, test_word);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            time_findDS.Insert(word_DS.Type, word_DS.Number, timeSpan);
        }
        ReadKey();*/
    }
}
