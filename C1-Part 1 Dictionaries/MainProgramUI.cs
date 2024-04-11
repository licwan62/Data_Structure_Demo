using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Console;

namespace C1_Part_1_Dictionaries;

internal class MainProgramUI
{
    static (DictionaryDS[] word_DS, TimeDictionaryDS time_DS) FilesLoad()
    {
        // get files from specific directory
        FileInfo[] order_files = new DirectoryInfo(@"..\..\..\ordered").GetFiles();
        FileInfo[] random_files = new DirectoryInfo(@"..\..\..\random").GetFiles();
        // put files from two directories togeter for simplicity in 1 traverse
        FileInfo[] files = order_files.Concat(random_files).ToArray();
        // create array of dictionaries for each file
        DictionaryDS[] dictionariesDS = new DictionaryDS[files.Length];

        // dictionary to record time comsumed by each insertion
        TimeDictionaryDS timeInsertionDS = new();
        Stopwatch stopwatch = new Stopwatch();
        // use streamReader to split lines and load not null line in dictionary
        int index = 0;
        foreach (FileInfo file in files)
        {
            DictionaryDS word_DS = new(file.FullName);// dictionary to load words and lengths
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
    static void Insert(DictionaryDS word_DS, string word)
    {
        WriteLine(word_DS.Insert(word));
    }
    static void Find(DictionaryDS word_DS, string word)
    {
        WriteLine(word_DS.Find(word));
    }
    static void Delete(DictionaryDS word_DS, string word)
    {
        WriteLine(word_DS.Delete(word));
    }
    static void ToPrint(DictionaryDS word_DS, string word)
    {
        WriteLine(word_DS.Print());
    }
    static void Test(DictionaryDS word_DS)
    {
        WriteLine("------------------------");
        WriteLine($"Test operations done to {word_DS.Name}");
        Insert(word_DS, "Licheng");
        Insert(word_DS, "#Test");
        Insert(word_DS, "Licheng");
        Find(word_DS, "Licheng");
        Find(word_DS, "Licheng123");
        Delete(word_DS, "Licheng");
        Delete(word_DS, "Licheng");
        WriteLine("------------------------\n");
    }
    static string TestCode()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Insert(word_DS, \"Licheng\");");
        sb.AppendLine("Insert(word_DS, \"#Test\");");
        sb.AppendLine("Insert(word_DS, \"Licheng\");");
        sb.AppendLine("Find(word_DS, \"Lichen\");");
        sb.AppendLine("Find(word_DS, \"Licheng123\");");
        sb.AppendLine("Delete(word_DS, \"Licheng\");");
        sb.AppendLine("Delete(word_DS, \"Licheng\");");
        return sb.ToString();
    }
    /// <summary>
    /// put text files into array of dictionary
    /// show time table for each dictionary loaded
    /// test some code
    /// find a word in each dictionary
    /// shoe time table for each dictionary finding
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        // 1 put all text file into array of dictionary
        // Tuple is used to be assigned two objects, here to receive two objects returned by function
        Tuple<DictionaryDS[], TimeDictionaryDS> tuple = FilesLoad().ToTuple<DictionaryDS[], TimeDictionaryDS>();
        DictionaryDS[] word_DS_array = tuple.Item1;
        TimeDictionaryDS time_insertionDS = tuple.Item2;
        // 2 show time table for each dictionary loaded
        Write("Press any key to Print time table for each dictionary insertion...");
        ReadKey();
        WriteLine(time_insertionDS.Print());
        // 3 test some code
        WriteLine("***** Test Code *****\n" + TestCode() + "Test Code End -----");
        Write("Do test functions above to first dictionary? [y/n]: ");
        while (true)
        {
            string input = ReadLine().ToLower().Trim();
            if (input == string.Empty)
                Write("Empty input, type your answer [y/n]: ");
            else if (input == "y")
            {
                WriteLine("Test done");
                Test(word_DS_array[0]);
                break;
            }
            else if (input == "n")
            {
                WriteLine("Nothing test done");
                break;
            }
        }
        // 4 find a word in each dictionary
        TimeDictionaryDS time_findDS = new();
        string test_word = "nosuchword";
        Stopwatch stopwatch = new Stopwatch();
        WriteLine("[ Find the word in each dictionary then record their time during finding ]");
        WriteLine($"Using word: {test_word}");
        Write("Press any key to continue...");
        ReadKey();
        foreach (DictionaryDS word_DS in word_DS_array)
        {
            stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            Find(word_DS, test_word);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            time_findDS.Insert(word_DS.Type, word_DS.Number, timeSpan);
        }
        // 5 show time table for each dictionary finding
        WriteLine($"Print Time record for finding {test_word}");
        Write("Press any key to continue...");
        ReadKey();
        WriteLine(time_findDS.Print()); 
        
        ReadKey();

    }
}
