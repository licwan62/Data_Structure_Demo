using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;
using static System.Console;
namespace Data_C2;
internal class Program
{
    private static DBLList? listToLoad;
    /// <summary>
    /// Load word files
    /// Print Loading feedback
    /// </summary>
    /// <param name="index">selected items index</param>
    /// <returns>loaded list</returns>
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
        Write("Loading... ");
        sw.Start();
        while ((line = sr.ReadLine()) != null)
        {
            list.AddAtEnd(line);
        }
        sw.Stop();
        ts = sw.Elapsed;
        WriteLine("Done");
        WriteLine($"{list.Counter} Words Loaded, Elapsed {String.Format("{0:N2}", ts.TotalMilliseconds)} ms");
        return list;
    }
    static bool ReportTime()
    {
        DirectoryInfo dir = new DirectoryInfo(@"..\..\..\WordFiles");
        FileInfo[] fileInfos = dir.GetFiles().OrderBy(f => f.Length).ToArray();
        int size = fileInfos.Length;
        DBLList[] lists = new DBLList[size];
        Write("- Print Time Report? (y) ");
        if (ReadLine()?.Trim().ToLower() == "y")
        {
            Clear();// load all files in directory
            WriteLine($"Start Loading {size} Files...");
            for (int index = 0; index < size; index++)
            {
                Write($"Loading {fileInfos[index].Name}... ");
                lists[index] = Load(index);
            }
            WriteLine("All {0} Files are Loaded\n", size);
            // insert word and record insertion time
            WriteLine("- Press any key to continue Inserting and Record time...");
            ReadKey();
            TimeSpan[] timeSpans = new TimeSpan[size];
            for (int index = 0; index < size; index++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                WriteLine(lists[index].AddAtEnd("Licheng"));
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
            WriteLine("- Press any key to Start Menu...");
            ReadKey();
            SetStartMenu();
            return true;
        }
        else
        {
            WriteLine("Stop printing Time Report");
            WriteLine("- Press any key to Start Menu");
            ReadKey();
            Clear();
            return false;
        }
    }

    #region Menu
    static string ShowMenu(int id)
    {
        StringBuilder sb = new StringBuilder();
        switch (id)
        {
            case 0:// start menu
                sb.AppendLine("== Start Menu ==");
                sb.AppendLine("1: Select a Words File");
                sb.AppendLine("2: Print Time Report");
                sb.AppendLine("0: Quit");
                return sb.ToString();
            case 1:// file load menu
                sb.AppendLine("== Select File ==");
                sb.AppendLine("1: 1000-words");
                sb.AppendLine("2: 5000-words");
                sb.AppendLine("3: 10000-words");
                sb.AppendLine("4: 15000-words");
                sb.AppendLine("5: 20000-words");
                sb.AppendLine("6: 25000-words");
                sb.AppendLine("7: 30000-words");
                sb.AppendLine("8: 35000-words");
                sb.AppendLine("9: 40000-words");
                sb.AppendLine("10: 45000-words");
                sb.AppendLine("11: 50000-words");
                sb.AppendLine("0: Back to Main Menu");
                return sb.ToString();
            case 2:// operation menu
                sb.AppendLine("== Operation ==");
                sb.AppendLine("1: Insert");
                sb.AppendLine("2: Delete");
                sb.AppendLine("3: Find");
                sb.AppendLine("4: Print");
                sb.AppendLine("5: Test functions");
                sb.AppendLine("0: Back to Select File");
                return sb.ToString();
            case 3: // insert method
                sb.AppendLine("== Insert Method");
                sb.AppendLine("1: Insert at Front");
                sb.AppendLine("2: Insert at End");
                sb.AppendLine("3: Insert Before");
                sb.AppendLine("4: Insert After");
                sb.AppendLine("0: Back to Operation");
                return sb.ToString();
            default: return "";
        }
    }
    static void SetStartMenu()
    {
        bool set = false;
        WriteLine(ShowMenu(0));
        while (!set)
        {
            Write("- Enter Main Option: ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim()))
            {
                WriteLine("Empty Input");
            }
            else if (int.TryParse(input, out int result))
            {
                switch (result)
                {
                    case 0:
                        set = Quit();
                        break;
                    case 1:
                        set = true;
                        Clear();
                        SetFileLoadMenu();
                        break;
                    case 2:
                        Clear();
                        set = ReportTime();
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
        listToLoad = new DBLList();
        bool set = false;
        WriteLine(ShowMenu(1));
        while (!set)
        {
            Write("- Select a File: ");
            string? input;
            if (string.IsNullOrEmpty(input = ReadLine()?.Trim()))
            {
                WriteLine("Empty Input");
            }
            else if (int.TryParse(input, out int result))
            {
                if (result == 0)
                {// back to start menu
                    set = true;
                    Clear();
                    SetStartMenu();
                }
                else if (result > 0 && result < 12)
                {
                    Write($"- To Load the No.{result} file? (y) ");
                    if (ReadLine()?.Trim().ToLower() == "y")
                    {// go to operation menu
                        set = true;
                        listToLoad = Load(result - 1);
                        WriteLine("- Press any key to commit operation");
                        ReadKey();
                        Clear();
                        SetOpMenu(listToLoad);
                    }
                    else
                    {// not enter "y", init
                        Clear();
                        SetFileLoadMenu();
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
    }
    /// <summary>
    /// FileLoadMenu selected, show OperationMenu
    /// </summary>
    /// <param name="list">list loaded</param>
    static void SetOpMenu(DBLList list)
    {
        bool set = false;
        WriteLine($"Current File {list.File.Name}");
        WriteLine(ShowMenu(2));
        while (!set)
        {
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
                    case 1: set = Insert(list); break;
                    case 2: set = Delete(list); break;
                    case 3: set = Find(list); break;
                    case 4: set = Print(list); break;
                    case 5: TestFunction(list); set = true; break;
                    case 0:
                        set = true;
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
        // Repeat showing OperationMenu
        WriteLine("Press any key to Operation Menu...");
        ReadKey();
        Clear();
        SetOpMenu(list);
    }
    #endregion Menu

    #region Functions
    static void TestFunction(DBLList list)
    {
        // test insertion
        WriteLine("- Press any key to Insert \"NewHead\" at the Front [AddAtFront Checked]");
        ReadKey();
        WriteLine(list.AddAtFront("NewHead"));// test insert at front
        WriteLine("- Press any key to Insert \"NewEnd\" at the End [AddAtEnd Checked]");
        ReadKey();
        WriteLine(list.AddAtEnd("NewEnd"));// test insert at end
        WriteLine("- Press any key to Insert \"BeforeNewEnd\" Before \"NewEnd\" [AddBefore Checked]");
        ReadKey();
        WriteLine(list.AddBefore("BeforeNewEnd", "NewEnd"));// test insert before
        WriteLine("- Press any key to Insert \"AfterNewHead\" After \"NewHead\" [AddAfter Checked]");
        ReadKey();
        WriteLine(list.AddAfter("AfterNewHead", "NewHead"));// test insert after
        // test find
        WriteLine("- Press any key to Find \"NewEnd\" that is inserted already [Find Checked]");
        ReadKey();
        WriteLine(list.Find("NewEnd"));
        // test print
        WriteLine("- Press any key to Print the list with 4 Insertion at Head and Tail");
        ReadKey();
        WriteLine(list.ToPrint());
        // test deletion
        WriteLine("- Press any key to Remove \"NewEnd\" [Remove Checked]");
        ReadKey();
        WriteLine(list.Remove("NewEnd"));
        WriteLine("- Press any key to Print the list Without \"NewEnd\" at End");
        ReadKey();
        WriteLine(list.ToPrint());
    }
    static bool Quit()
    {
        Write("- Quit the console? (y) ");
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
        bool stop = false;
        while (!inserted && !stop)
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
                    WriteLine(list.AddAtEnd(word));
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
                stop = true;
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
                    WriteLine(list.Remove(word));
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