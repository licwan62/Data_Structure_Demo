using System.Diagnostics;
using static System.Console;

namespace C1_Part_2_Sorting_Algorithms;

internal class MainProgramUI
{
    static DictionaryDS[] FilesLoad()
    {
        string path = @"..\..\..\random";
        FileInfo[] files = new DirectoryInfo(path).GetFiles();
        DictionaryDS[] dictionariesDS = new DictionaryDS[files.Length];
        int index = 0;
        foreach (var file in files)
        {
            DictionaryDS word_DS = new(file.FullName);
            StreamReader sr = new StreamReader(file.FullName);
            string line;
            while ((line = sr.ReadLine()) != null)
                word_DS.Insert(line);
            dictionariesDS[index] = word_DS;
            WriteLine($"Dictionary of {word_DS.Name} is successfully loaded");
            index++;
        }
        WriteLine($"Total {files.Length} flies successfully Loaded");
        WriteLine("----------------------------------------------------");

        return dictionariesDS;
    }
    static void Main(string[] args)
    {
        DictionaryDS[] word_DSs = FilesLoad();
        ArrayDS[] arrayDSs = new ArrayDS[word_DSs.Length];
        int index = 0;
        WriteLine("*** Generating Array ***");
        foreach (var word_DS in word_DSs)
        {
            Write($"Generating Array data comes from {word_DS.Name}...\t");
            ArrayDS arrayDS = new ArrayDS(word_DS.Number, word_DS.Count);
            arrayDS = word_DS.ToArray();
            Write($"Succeeded! {arrayDS.Name} contains {arrayDS.ArraySize} words\n");
            arrayDSs[index] = arrayDS;
            index++;
        }
        WriteLine($"All dictionaries transferred to arrays : {arrayDSs.Length} arrays generated");
        WriteLine("------------------------------------------------------------------------");
        WriteLine("*** Testing ***");
        Write("Press [y] to print the first array: ");
        while (true)
        {
            string input = ReadLine().Trim().ToLower();
            if (input == "y")
            {
                WriteLine(arrayDSs[1].Print());
                break;
            }
            else if (input == "n")
            {
                WriteLine("Not printing");
                break;
            }
            else
                Write("Invalid input, Press [y] to print the first array: ");
        }
        Write("Press [y] to print sort out array by selection: ");
        while (true)
        {
            string input = ReadLine().Trim().ToLower();
            if (input == "y")
            {
                WriteLine(arrayDSs[1].SelectionSort());
                break;
            }
            else if (input == "n")
            {
                WriteLine("Not printing");
                break;
            }
            else
                Write("Invalid input, Press [y] to print sort out array by selection: ");
        }
        Write("Press [y] to print sort out array above by merge: ");
        while (true)
        {
            string input = ReadLine().Trim().ToLower();
            if (input == "y")
            {
                WriteLine(arrayDSs[1].MergeSort());
                break;
            }
            else if (input == "n")
            {
                WriteLine("Not printing");
                break;
            }
            else
                Write("Invalid input, Press [y] to print sort out array above by merge: ");
        }

        WriteLine("*** Sorting each array in 2 methods ***");
        WriteLine("Please wait...");
        TimeDictionsryDS time_DS_selection = new("Selection");
        foreach (var arrayDS in arrayDSs)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            arrayDS.SelectionSort();
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            time_DS_selection.Insert(arrayDS.Number, timeSpan);
        }
        TimeDictionsryDS time_DS_merge = new("Merge");
        foreach (var arrayDS in arrayDSs)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            arrayDS.MergeSort();
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            time_DS_merge.Insert(arrayDS.Number, timeSpan);
        }
        WriteLine("Sorting complete");
        WriteLine("Press any key to print time elapsed table");
        Read();
        WriteLine(time_DS_selection.Print());
        WriteLine(time_DS_merge.Print());
    }

}
