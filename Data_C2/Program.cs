﻿using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;
using static System.Console;
namespace Data_C2;
internal class Program
{
    private static DBLList? listToLoad = new DBLList();
    static DBLList Load(int index)
    {
        DirectoryInfo dir = new DirectoryInfo(@"..\..\..\WordFiles");
        FileInfo[] fileInfos = dir.GetFiles().OrderBy(f => f.Length).ToArray();
        FileInfo fileToLoad = fileInfos[index];
        DBLList list = new DBLList(fileToLoad);
        TimeSpan ts = new TimeSpan();
        Stopwatch sw = new Stopwatch();
        StreamReader sr = fileToLoad.OpenText();
        string? line;
        sw.Start();
        while ((line = sr.ReadLine()) != null)
        {
            list.Insert(line);
        }
        sw.Stop();
        ts = sw.Elapsed;
        WriteLine($"{list.Counter} Words Loaded, Elapsed {String.Format("{0:N2}", ts.TotalMilliseconds)} ms");
        return list;
    }
    static void ReportTime()
    {
        DirectoryInfo dir = new DirectoryInfo(@"..\..\..\WordFiles");
        FileInfo[] fileInfos = dir.GetFiles().OrderBy(f => f.Length).ToArray();
        int size = fileInfos.Length;
        DBLList[] lists = new DBLList[size];
        // load all files in directory
        WriteLine($"Start Loading {size} Files...");
        for (int index = 0; index < size; index++)
        {
            Write($"Loading {fileInfos[index].Name}... ");
            lists[index] = Load(index);
        }
        WriteLine("All {0} Files are Loaded\n", size);
        // insert word and record insertion time
        WriteLine("- Press any key to continue Inserting certain word and Record time...");
        ReadKey();
        TimeSpan[] timeSpans = new TimeSpan[size];
        for (int index = 0; index < size; index++)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            WriteLine(lists[index].Insert("Licheng"));
            sw.Stop();
            timeSpans[index] = sw.Elapsed;
        }
        // print table of word and time
        WriteLine("- Press any key to Print Report...");
        ReadKey();
        WriteLine("== Time Report ==");
        WriteLine("{0, -10}{1}", "Words", "Time");
        for (int index = 0; index < size; index++)
        {
            WriteLine($"{lists[index].Counter,-10}{String.Format("{0:N2}", timeSpans[index].TotalMilliseconds)}");
        }
        // back to start menu
        WriteLine("- Press any key to continue...");
        ReadKey();
        SetStartMenu();
    }

    #region Menu
    static void ShowMenu(int id)
    {
        switch (id)
        {
            case 0:// start menu
                WriteLine("== Start Menu ==");
                WriteLine("1: Select a Words File");
                WriteLine("2: Print Time Report");
                WriteLine("0: Quit");
                break;
            case 1:// file load menu
                WriteLine("== Select a File ==");
                WriteLine("1: 1000-words");
                WriteLine("2: 5000-words");
                WriteLine("3: 10000-words");
                WriteLine("4: 15000-words");
                WriteLine("5: 20000-words");
                WriteLine("6: 25000-words");
                WriteLine("7: 30000-words");
                WriteLine("8: 35000-words");
                WriteLine("9: 40000-words");
                WriteLine("10: 45000-words");
                WriteLine("11: 50000-words");
                WriteLine("0: Back to Main Menu");
                break;
            case 2:// operation menu
                WriteLine("== Select a Operation ==");
                WriteLine("1: Insert");
                WriteLine("2: Delete");
                WriteLine("3: Find");
                WriteLine("4: Print");
                WriteLine("5: Test functions");
                WriteLine("0: Back to File Selection");
                break;
        }
    }
    static void SetStartMenu()
    {
        bool SetOption0 = false;
        while (!SetOption0)
        {
            ShowMenu(0);
            Write("- Enter Main Option: ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim()))
            {
                WriteLine("Empty Input");
                continue;
            }
            else if (int.TryParse(input, out int result))
            {
                switch (result)
                {
                    case 0:
                        SetOption0 = Quit();
                        break;
                    case 1:
                        SetFileLoadMenu();
                        SetOption0 = true;
                        break;
                    case 2:
                        Write("- Print Time Report, continue? (y) ");
                        if (ReadLine()?.Trim().ToLower() == "y")
                        {
                            ReportTime();
                            SetOption0 = true;
                        }
                        else
                        {
                            WriteLine("Stop printing Time Report");
                        }
                        break;
                    default:
                        WriteLine("Invalid Number");
                        break;
                }
            }
            else
            {
                WriteLine("Invalid Number");
            }
        }
    }
    static void SetFileLoadMenu()
    {
        listToLoad = null;
        bool setOption1 = false;
        while (!setOption1)
        {
            ShowMenu(1);
            Write("- Select a File: ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim()))
            {
                WriteLine("Empty Input");
            }
            else if (int.TryParse(input, out int result))
            {
                if (result == 0)
                {
                    Write("- Back to Main Menu, continue? (y) ");
                    if (ReadLine()?.Trim().ToLower() == "y")
                    {
                        Clear();
                        SetStartMenu();
                        setOption1 = true;
                    }
                }
                else if (result > 0 && result < 12)
                {
                    Write($"- To Load the No.{result} file? (y) ");
                    if (ReadLine()?.Trim().ToLower() == "y")
                    {
                        Write("Loading... ");
                        listToLoad = Load(result - 1);
                        SetOpMenu(listToLoad);
                        setOption1 = true;
                    }
                    else
                    {
                        WriteLine("Stop Loading");
                    }
                }
                else
                {
                    WriteLine("Invalid Number");
                }
            }
            else
            {
                WriteLine("Invalid Input");
            }
        }
        WriteLine("Press any key to continue...");
        ReadKey();
    }
    static void SetOpMenu(DBLList list)
    {
        bool SetOption2 = false;
        while (!SetOption2)
        {
            ShowMenu(2);
            WriteLine($"Current File {list.File.Name}");
            Write("- Select an Function: ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim().ToLower()))
            {
                WriteLine("Empty Input");
            }
            else if (int.TryParse(input, out int result))
            {
                switch (result)
                {
                    case 1: SetOption2 = Insert(list); break;
                    case 2: SetOption2 = Delete(list); break;
                    case 3: SetOption2 = Find(list); break;
                    case 4: SetOption2 = Print(list); break;
                    case 5: SetOption2 = TestFunction(list); break;
                    case 0:
                        Clear();
                        SetFileLoadMenu();
                        break;
                    default: WriteLine("Invalid Number"); break;
                }
            }
            else
            {
                WriteLine("Invalid Input");
            }
        }
        WriteLine("Press any key to continue...");
        ReadKey();
        SetOpMenu(list);
    }
    #endregion Menu// Start Menu -> FileLoad Menu -> Op Menu

    #region Functions
    static bool TestFunction(DBLList list)
    {
        WriteLine("Press any key to continue Test");
        WriteLine("Test include Insertion, Finding, Deletion of \"Test123\" or \"#Test123\"");
        ReadKey();
        WriteLine(list.Insert("Test123"));
        WriteLine(list.Insert("Test123"));
        WriteLine(list.Insert("#Test123"));
        WriteLine(list.Find("Test123"));
        WriteLine(list.Delete("Test123"));
        WriteLine(list.Find("Test123"));
        WriteLine(list.Delete("Test123"));
        WriteLine("Press any key to Print the List");
        ReadKey();
        WriteLine(list.ToPrint());
        return true;
    }
    static bool Quit()
    {
        Write("- Quit the console, continue? (y) ");
        string? input;
        if (string.IsNullOrEmpty(input = ReadLine()?.Trim().ToLower()))
        {
            WriteLine("Empty Input");
            return false;
        }
        else if (input == "y")
        {
            Environment.Exit(0);
            return true;
        }
        else
        {
            WriteLine("Stop Quiting");
            return false;
        }
    }
    static bool Insert(DBLList list)
    {
        bool inserted = false;
        while (!inserted)
        {// confirm insertion
            Write($"- Insert a new word in List? (y: yes, #: stop) ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim().ToLower()))
            {
                WriteLine("Empty Input");
            }
            else if (input == "y")
            {// get word to insert
                Write("- New word: ");
                string? word;
                if (!String.IsNullOrEmpty(word = ReadLine()))
                {
                    WriteLine(list.Insert(word));
                    inserted = true;
                }
                else
                {
                    WriteLine("Empty Word, No Insertion");
                }
            }
            else if (input == "#")
            {
                WriteLine("Stop Insertion");
                return inserted;
            }
        }
        return inserted;
    }
    static bool Delete(DBLList list)
    {
        bool deleted = false;
        while (!deleted)
        {
            Write($"- Delete a word in List? (y: yes, #: stop) ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim().ToLower()))
            {
                WriteLine("Empty Input");
            }
            else if (input == "y")
            {
                Write("- Word to Delete: ");
                string? word;
                if (!String.IsNullOrEmpty(word = ReadLine()))
                {
                    WriteLine(list.Delete(word));
                    deleted = true;
                }
                else
                {
                    WriteLine("Empty Word, No Deletion");
                }
            }
            else if (input == "#")
            {
                WriteLine("Stop Deletion");
                return deleted;
            }
        }
        return deleted;
    }
    static bool Find(DBLList list)
    {
        bool found = false;
        while (!found)
        {
            Write($"- Find a word in List? (y: yes, #: stop) ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim().ToLower()))
            {
                WriteLine("Empty Input");
            }
            else if (input == "y")
            {
                Write("- Word to Find: ");
                string? word;
                if (!String.IsNullOrEmpty(word = ReadLine()))
                {
                    WriteLine(list.Find(word));
                    found = true;
                }
                else
                {
                    WriteLine("Empty Word, No Finding");
                }
            }
            else if (input == "#")
            {
                WriteLine("Stop Finding");
                return found;
            }
        }
        return found;
    }

    static bool Print(DBLList list)
    {
        bool printed = false;
        while (!printed)
        {
            Write($"- Print Selected List? (y: yes, #: stop) ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim().ToLower()))
            {
                WriteLine("Empty List");
            }
            else if (input == "#")
            {
                WriteLine("Stop Printing");
                return printed;
            }
            else
            {
                WriteLine(list.ToPrint());
                printed = true;
            }
        }
        return printed;
    }
    #endregion Functions
    static void Main(string[] args)
    {
        SetStartMenu();
    }
}