using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Console;
namespace Data_C3;

internal class Program
{
    static readonly string orderedPath = @"..\..\..\Ordered";
    static readonly string randomPath = @"..\..\..\Random";
    static readonly string inpGuide = "[Y/N]";

    // indicate file index and option name in main menu
    static int fileNum;
    static int[] words =
    {
        1000,5000,10000,15000,20000,25000,30000,35000,40000,45000,50000
    };
    static string[] fileNames = null!;

    // indicate decided type of file and tree at loading menu
    static bool Ordered;
    static bool balanced;
    static string Sequence { get => Ordered ? "Ordered" : "Random"; }
    static string TreeType { get => balanced ? "AVL" : "BST"; }

    // loaded tree before function test
    static BST_DS bst_DS = null!;
    static AVL_DS avl_DS = null!;

    // loaded trees after loading all files
    static BST_DS[] orderedBST_DSs = null!;
    static AVL_DS[] orderedAVL_DSs = null!;
    static BST_DS[] randomBST_DSs = null!;
    static AVL_DS[] randomAVL_DSs = null!;

    // file to be loaded that selected at loading menu
    static FileInfo fileToLoad = null!;

    // time recorded before printing time comparison
    static TimeSpan[] insertionTimes_orderedBST = null!;
    static TimeSpan[] insertionTimes_randomBST = null!;
    static TimeSpan[] insertionTimes_orderedAVL = null!;
    static TimeSpan[] insertionTimes_randomAVL = null!;
    static TimeSpan[] searchingTimes_orderedBST = null!;
    static TimeSpan[] searchingTimes_randomBST = null!;
    static TimeSpan[] searchingTimes_orderedAVL = null!;
    static TimeSpan[] searchingTimes_randomAVL = null!;
    static void Main(string[] args)
    {
        // generate options on main menu
        DirectoryInfo orderDirInfo = new DirectoryInfo(orderedPath);
        DirectoryInfo randomDirInfo = new DirectoryInfo(randomPath);
        fileNum = int.Min(orderDirInfo.GetFiles().Length, randomDirInfo.GetFiles().Length);
        fileNames = new string[fileNum];
        for (int i = 0; i < fileNum; i++)
        {
            fileNames[i] = words[i].ToString() + "_words";
        }
        // Main Menu: get tree and file type
        // Loading Menu: get specified file for loading
        // Function Menu: get function on the file
        GetStart();
    }

    #region Main Menu
    static void PrintMenu_main()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.Append("\u001b[33m");
        sb.AppendLine("=== Main Menu - select File Sequence and Tree Structure ===");
        sb.Append("\u001b[0m");
        sb.AppendLine("0 - Exist Program");
        sb.AppendLine("1 - Load Ordered File into BST");
        sb.AppendLine("2 - Load Random File into BST");
        sb.AppendLine("3 - Load Ordered File into AVL");
        sb.AppendLine("4 - Load Random File into AVL");
        sb.AppendLine("5 - Load all file and Get Time Report");
        WriteLine(sb.ToString());
    }
    /// <summary>
    /// Main Menu
    /// get order of file and tree type where file is loaded
    /// select option to assign corresponding bools to {Ordered} and {balanced}
    /// </summary>
    static void GetStart()
    {
        int maxNum = 5;// range for user type
        PrintMenu_main();
        while (true)
        {
            Write("- Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {// Enpty input or value out of range
                WriteWarning("INVALID NUMBER!");
            }
            else
            {
                bool confirmed = Start_confirm(value);
                if (confirmed)
                {
                    if (value == 5)
                    {
                        GetTimeReport();
                        // returned when back from time report menu
                        PrintMenu_main();
                    }
                    else
                    {// valid option chosen except for 5, get confirmed to continue or loop
                        GetFile();
                        // returned when back from loading munu
                        PrintMenu_main();
                    }
                }

            }
        }
    }
    static bool Start_confirm(int value)
    {
        int confirmed = 0;
        while (true)
        {
            if (value == 0)
            {
                Write("- Sure to Exist Program? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    Environment.Exit(0);
                }
            }
            else if (value == 1)
            {
                Write("- Sure to Load Ordered File into Binary Search Tree? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    Ordered = true;
                    balanced = false;
                    WriteWarning("Ordered file Loaded(BST)");
                }
            }
            else if (value == 2)
            {
                Write("- Sure to Load Random File into Binary Search Tree? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    Ordered = false;
                    balanced = false;
                    WriteWarning("Random file Loaded(BST)");
                }
            }
            else if (value == 3)
            {
                Write("- Sure to Load Ordered File into AVL Tree? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    Ordered = true;
                    balanced = true;
                    WriteWarning("Ordered file Loaded(AVL)");
                }
            }
            else if (value == 4)
            {
                Write("- Sure to Load Random File into AVL Tree? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    Ordered = false;
                    balanced = true;
                    WriteWarning("Random file Loaded(AVL)");
                }
            }
            else if (value == 5)
            {
                Write("- Sure to Print Time Record? This operation will take long time {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
            }
            // feedback and return(continue)
            switch (confirmed)
            {
                case -1:
                    WriteWarning("INVALID INPUT!");
                    break;// continue loop
                case 0:
                    WriteWarning("OPERATION CANCELLED!");
                    return false;
                case 1:
                    return true;
            }
        }
    }
    #endregion Main Menu

    #region Loading Menu
    static void PrintMenu_loading()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.Append("\u001b[33m");
        sb.AppendLine($"=== Loading Menu - Sequence: {Sequence}, Tree Structure: {TreeType} ===");
        sb.Append("\u001b[0m");
        sb.AppendLine("0 - Back to Main Menu");
        for (int i = 0; i < fileNum; i++)
        {
            sb.AppendLine($"{i + 1} - {fileNames[i]}");
        }
        WriteLine(sb.ToString());
    }
    static void GetFile()
    {
        // Initialize tree
        bst_DS = new BST_DS();
        avl_DS = new AVL_DS();
        DirectoryInfo dirInfo = new DirectoryInfo(orderedPath);
        int maxNum = fileNum;

        PrintMenu_loading();
        while (true)
        {
            Write("- Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {// not an option
                WriteWarning("INVALID INPUT!");
            }
            else if (value == 0)
            {// option to back
                return;
            }
            else
            {// option to load
                bool toLoad = Loading_confirm(value);
                if (toLoad)
                {// loading succeed
                    if (Ordered && balanced)
                    {// Ordered file on AVL
                        avl_DS = Load_AVL(orderedPath, value - 1);
                        avl_DS.Name = "AVL_" + fileNames[value - 1];
                    }
                    else if (Ordered)
                    {// Ordered file on BST
                        bst_DS = Load_BST(orderedPath, value - 1);
                        bst_DS.Name = "BST_" + fileNames[value - 1];
                    }
                    else if (balanced)
                    {// Random file on AVL
                        avl_DS = Load_AVL(randomPath, value - 1);
                        avl_DS.Name = "AVL_" + fileNames[value - 1];
                    }
                    else
                    {// Random file on BST
                        bst_DS = Load_BST(randomPath, value - 1);
                        bst_DS.Name = "BST_" + fileNames[value - 1];
                    }
                    WriteComplete("LOADING COMPLETED!");
                    Write("- Press any key to continue...");
                    ReadLine();
                    GetFunction();
                    // returned when back from Function Menu
                }
                else
                {// not loaded
                    WriteWarning("STOP LOADING!");
                    Write("- Press any key to continue...");
                    ReadLine();
                }
                PrintMenu_loading();
            }
        }
    }
    static bool Loading_confirm(int value)
    {
        while (true)
        {
            Write($"- Sure to Load {Sequence} {fileNames[value - 1]} on {TreeType}? {inpGuide} ");
            int confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {// get confirmed to load selected file 
                return true;
            }
            else if (confirmed == 0)
            {// confirmed not
                WriteWarning("STOP LOADING!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteWarning("INVALID INPUT!");
            }
        }
    }
    #endregion Loading Menu

    #region Funtion Menu
    static void PrintMenu_function()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.Append("\u001b[33m");
        sb.AppendLine($"=== Function Menu - Sequence: {Sequence}, File {fileToLoad.Name}, Current Tree Structure: {TreeType} ===");
        sb.Append("\u001b[0m");
        sb.AppendLine("0 - Back to Loading Menu");
        sb.AppendLine("1 - Print");
        sb.AppendLine("2 - Insert");
        sb.AppendLine("3 - Delete");
        sb.AppendLine("4 - Search");
        sb.AppendLine("5 - Function Test");
        WriteLine(sb.ToString());
    }
    static void GetFunction()
    {
        int maxNum = 5;

        PrintMenu_function();
        while (true)
        {
            Write("Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {
                WriteWarning("INVALID INPUT!");
            }
            else if (value == 0)
            {
                return;
            }
            else
            {
                bool confirmed;
                if (value == 1)
                {
                    if (confirmed = Print_confirm())
                    {
                        Print();
                    }
                }
                else if (value == 2)
                {
                    if (confirmed = Insert_confirm())
                    {
                        Insert();
                    }
                }
                else if (value == 3)
                {
                    if (confirmed = Delete_confirm())
                    {
                        Delete();
                    }
                }
                else if (value == 4)
                {
                    if (confirmed = Search_confirm())
                    {
                        Search();
                    }
                }
                else
                {
                    if (confirmed = FunctionTest_confirm())
                    {
                        FunctionTest();
                    }
                }
                Write("- Press any key to continue...");
                ReadLine();
                PrintMenu_function();
            }
        }
    }
    #endregion Function Menu

    #region Print
    static void PrintMenu_order()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.Append("\u001b[33m");
        sb.AppendLine($"=== Print Order - Sequence: {Sequence}, File {fileToLoad.Name}, Current Tree Structure: {TreeType} ===");
        sb.Append("\u001b[0m");
        sb.AppendLine("0 - Return to Function Menu");
        sb.AppendLine("1 - Inorder");
        sb.AppendLine("2 - Preorder");
        sb.AppendLine("3 - Postorder");
        WriteLine(sb.ToString());
    }
    static void Print()
    {
        int maxNum = 3;

        PrintMenu_order();
        while (true)
        {// 1 get print order
            Write("Enter Number: ");
            string? input = ReadLine();
            int orderNum = Util.GetInt(input, maxNum);
            if (orderNum == Util.bad_int)
            {
                WriteWarning("INVALID NUMBER!");
            }
            else if (orderNum == 0)
            {// option to back
                WriteWarning("STOP PRINTING!");
                return;
            }
            else
            {
                if (orderNum == 1)
                {
                    if (balanced)
                    {
                        WriteLine(avl_DS.InOrderPrint());
                    }
                    else
                    {
                        WriteLine(bst_DS.InOrderPrint());
                    }
                }
                else if (orderNum == 2)
                {
                    if (balanced)
                    {
                        WriteLine(avl_DS.PreOrderPrint());
                    }
                    else
                    {
                        WriteLine(bst_DS.PreOrderPrint());
                    }
                }
                else
                {
                    if (balanced)
                    {
                        WriteLine(avl_DS.PostOrderPrint());
                    }
                    else
                    {
                        WriteLine(bst_DS.PostOrderPrint());
                    }
                }
                WriteComplete("PRINTING COMPLETE!");
                return;
            }
        }
    }
    static bool Print_confirm()
    {
        while (true)
        {
            Write("- Sure to Print tree? (Current Tree: {0}) {1}", TreeType, inpGuide);
            int confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                return true;
            }
            else if (confirmed == 0)
            {
                WriteWarning("STOP PRINTING!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteWarning("INVALID INPUT!");
            }
        }
    }
    #endregion

    #region Insert
    static void Insert()
    {
        string? newWord;
        while (true)
        {
            // 1 get new word
            Write("- Enter your New Word: ");
            newWord = ReadLine();
            if (string.IsNullOrEmpty(newWord))
            {
                WriteWarning("Empty word!");
            }
            else
            {// Not Empty word
                if (balanced)
                {
                    WriteFeedback(avl_DS.Add(newWord));
                }
                else
                {
                    WriteFeedback(bst_DS.Add(newWord));
                }
                return;
            }
        }
    }
    static bool Insert_confirm()
    {
        int confirmed;
        while (true)
        {
            Write("- Sure to Insert? {0} ", inpGuide);
            confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                return true;
            }
            else if (confirmed == 0)
            {
                WriteWarning("STOP INSERTING!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteWarning("INVALID INPUT!");
            }
        }
    }
    #endregion

    #region Delete
    static void Delete()
    {
        string? deleteWord;
        while (true)
        {
            // 1 get new word
            Write("- Enter Word to delete: ");
            deleteWord = ReadLine();
            if (string.IsNullOrEmpty(deleteWord))
            {
                WriteWarning("Empty word!");
            }
            else
            {// Not Empty word
                if (balanced)
                {
                    WriteFeedback(avl_DS.Delete(deleteWord));
                }
                else
                {
                    WriteFeedback(bst_DS.Delete(deleteWord));
                }
                return;
            }
        }
    }
    static bool Delete_confirm()
    {
        int confirmed;
        while (true)
        {
            Write("- Sure to Delete? {0} ", inpGuide);
            confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                return true;
            }
            else if (confirmed == 0)
            {
                WriteWarning("STOP DELETING!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteWarning("INVALID INPUT!");
            }
        }
    }
    #endregion

    #region Search
    static void Search()
    {
        string? wordToSearch;
        while (true)
        {// 1 get new word
            Write("- Enter Word to Search: ");
            wordToSearch = ReadLine();
            if (string.IsNullOrEmpty(wordToSearch))
            {
                WriteWarning("Empty word!");
            }
            else
            {// Not Empty word
                if (balanced)
                {
                    WriteFeedback(avl_DS.Search(wordToSearch));
                }
                else
                {
                    WriteFeedback(bst_DS.Search(wordToSearch));
                }
                WriteComplete("SEARCHING COMPLETED!");
                return;
            }
        }
    }
    static bool Search_confirm()
    {
        int confirmed;
        while (true)
        {
            Write("- Sure to Search? {0} ", inpGuide);
            confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                return true;
            }
            else if (confirmed == 0)
            {
                WriteWarning("STOP SEARCHING!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteLine("INVALID INPUT!");
            }
        }
    }
    static void SearchNodeInALlTrees()
    {
        searchingTimes_orderedBST = new TimeSpan[fileNum];
        searchingTimes_orderedAVL = new TimeSpan[fileNum];
        searchingTimes_randomBST = new TimeSpan[fileNum];
        searchingTimes_randomAVL = new TimeSpan[fileNum];
        string? wordToSearch;
        while (true)
        {
            Write("- Enter Word to Search: ");
            wordToSearch = ReadLine();
            if (string.IsNullOrEmpty(wordToSearch))
            {
                WriteWarning("Empty word!");
            }
            else
            {// Not Empty word
             // record searching time for each Ordered BST
                for (int i = 0; i < fileNum; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    sw.Start();

                    WriteFeedback(orderedBST_DSs[i].Search(wordToSearch));

                    sw.Stop();
                    searchingTimes_orderedBST[i] = sw.Elapsed;

                    orderedBST_DSs[i].Name = "Ordered_BST" + fileNames[i];
                    WriteLine(orderedBST_DSs[i].Name + "  " + sw.Elapsed.TotalMicroseconds);
                }
                // record searching time for each Random BST
                for (int i = 0; i < fileNum; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    sw.Start();

                    WriteFeedback(randomBST_DSs[i].Search(wordToSearch));

                    sw.Stop();
                    searchingTimes_randomBST[i] = sw.Elapsed;

                    randomBST_DSs[i].Name = "Random_BST" + fileNames[i];
                    WriteLine(randomBST_DSs[i].Name + "  " + sw.Elapsed.TotalMicroseconds);
                }
                // record searching time for each Ordered AVL
                for (int i = 0; i < fileNum; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    sw.Start();

                    WriteFeedback(orderedAVL_DSs[i].Search(wordToSearch));

                    sw.Stop();
                    searchingTimes_orderedAVL[i] = sw.Elapsed;

                    orderedAVL_DSs[i].Name = "Ordered_AVL" + fileNames[i];
                    WriteLine(orderedAVL_DSs[i].Name + "  " + sw.Elapsed.TotalMicroseconds);
                }
                // record searching time for each Random AVL
                for (int i = 0; i < fileNum; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    sw.Start();

                    WriteFeedback(randomAVL_DSs[i].Search(wordToSearch));

                    sw.Stop();
                    searchingTimes_randomAVL[i] = sw.Elapsed;

                    randomAVL_DSs[i].Name = "Random_AVL" + fileNames[i];
                    WriteLine(randomAVL_DSs[i].Name + "  " + sw.Elapsed.TotalMicroseconds);
                }
                return;
            }
        }
    }
    #endregion

    #region Function Test
    static void FunctionTest()
    {
        string testWord = "Licheng";
        Write("- Press any key to Insert \"{0}\" into this {1}...", testWord, TreeType);
        ReadLine();

        if (balanced)
        {
            WriteFeedback(avl_DS.Add(testWord));
        }
        else
        {
            WriteFeedback(bst_DS.Add(testWord));
        }

        Write("- Press any key to Find {0}...", testWord);
        ReadLine();

        if (balanced)
        {
            WriteFeedback(avl_DS.Search(testWord));
        }
        else
        {
            WriteFeedback(bst_DS.Search(testWord));
        }

        Write("- Press any key to Delete {0}...", testWord);
        ReadLine();

        if (balanced)
        {
            WriteFeedback(avl_DS.Delete(testWord));
        }
        else
        {
            WriteFeedback(bst_DS.Delete(testWord));
        }

        Write("- Press any key to Find {0} ({1} has been Deleted)...", testWord, testWord);
        ReadLine();

        if (balanced)
        {
            WriteFeedback(avl_DS.Search(testWord));
        }
        else
        {
            WriteFeedback(bst_DS.Search(testWord));
        }
        WriteComplete("FUNCTION TEST COMPLETED!");
    }
    static bool FunctionTest_confirm()
    {
        while (true)
        {
            Write("- Sure to do Function Test? (Current File: {0}, Sequence: {1}) {2}", Sequence, fileToLoad.Name, inpGuide);
            int confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                return true;
            }
            else if (confirmed == 0)
            {
                WriteWarning("STOP FUNCTION TEST!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteWarning("INVALID INPUT!");
            }
        }
    }
    #endregion

    #region Load
    static BST_DS Load_BST(string path, int index)
    {
        BST_DS ds = new BST_DS();
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        if (dirInfo.Exists)
        {
            FileInfo[] fileInfos = dirInfo.GetFiles().OrderBy(f => f.Length).ToArray();
            fileToLoad = fileInfos[index];
            StreamReader sr = fileToLoad.OpenText();
            string? line;
            Write($"File: {fileToLoad.Name}, Sequence: {Sequence} is Loading on {TreeType}...... ");
            while ((line = sr.ReadLine()) != null)
            {
                ds.Add(line);
            }
            WriteComplete($"Complete! {TreeType} Contain {ds.Count} words");
        }
        else
        {
            WriteWarning($"{path} Not Exists");
        }
        return ds;
    }
    static AVL_DS Load_AVL(string path, int index)
    {
        AVL_DS ds = new AVL_DS();
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        if (dirInfo.Exists)
        {
            // get the file
            FileInfo[] fileInfos = dirInfo.GetFiles().OrderBy(f => f.Length).ToArray();
            fileToLoad = fileInfos[index];
            // add words into avl tree
            Write($"File: {fileToLoad.Name}, Sequence: {Sequence} is Loading on {TreeType}...... ");
            StreamReader sr = fileToLoad.OpenText();
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                ds.Add(line);
            }
            WriteComplete($"Complete! {TreeType} Contain {ds.Count} words");
        }
        else
        {
            WriteWarning($"{path} Not Exists");
        }
        return ds;
    }
    static void LoadAllFiles()
    {
        insertionTimes_orderedBST = new TimeSpan[fileNum];
        insertionTimes_randomBST = new TimeSpan[fileNum];
        insertionTimes_orderedAVL = new TimeSpan[fileNum];
        insertionTimes_randomAVL = new TimeSpan[fileNum];
        orderedBST_DSs = new BST_DS[fileNum];
        randomBST_DSs = new BST_DS[fileNum];
        orderedAVL_DSs = new AVL_DS[fileNum];
        randomAVL_DSs = new AVL_DS[fileNum];
        // get all Ordered file for BST
        Stopwatch sw = new Stopwatch();
        for (int i = 0; i < fileNum; i++)
        {
            sw.Start();
            Ordered = true;
            balanced = false;
            orderedBST_DSs[i] = Load_BST(orderedPath, i);
            sw.Stop();
            insertionTimes_orderedBST[i] = sw.Elapsed;
        }
        // get all Random file for BST
        for (int i = 0; i < fileNum; i++)
        {
            sw.Restart();
            Ordered = false;
            balanced = false;
            randomBST_DSs[i] = Load_BST(randomPath, i);
            sw.Stop();
            insertionTimes_randomBST[i] = sw.Elapsed;
        }
        // get all Ordered file for AVL
        for (int i = 0; i < fileNum; i++)
        {
            sw.Restart();
            Ordered = true;
            balanced = true;
            orderedAVL_DSs[i] = Load_AVL(orderedPath, i);
            sw.Stop();
            insertionTimes_orderedAVL[i] = sw.Elapsed;
        }
        // get all Random file for AVL
        for (int i = 0; i < fileNum; i++)
        {
            sw.Restart();
            Ordered = false;
            balanced = true;
            randomAVL_DSs[i] = Load_AVL(randomPath, i);
            sw.Stop();
            insertionTimes_randomAVL[i] = sw.Elapsed;
        }
    }
    #endregion

    #region TimeReport
    static void PrintMenu_timeReport()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== Time Report Menu ===");
        sb.AppendLine("0 - Back to Main Menu");
        sb.AppendLine("1 - Compare times for Inserting all words in trees");
        sb.AppendLine("2 - Compare times for Searching a word in trees");
        sb.AppendLine("=== Time Report Menu ===");
        WriteLine(sb.ToString());
    }
    static void GetTimeReport()
    {
        LoadAllFiles();
        WriteComplete("Complete loading all files on both BST and AVL");
        Write("- Press any key to continue...");
        ReadLine();
        PrintMenu_timeReport();
        int maxNum = 2;
        while (true)
        {
            Write("Enter a Number: ");
            int value = Util.GetInt(ReadLine(), maxNum);
            if (value == Util.bad_int)
            {
                WriteWarning("INVALID NUMBER!");
            }
            else if (value == 0)
            {
                return;
            }
            else
            {
                if (value == 1)
                {
                    Report_insertion();
                }
                else
                {
                    Report_searching();
                }
                Write("Press any key to continue...");
                ReadLine();

                PrintMenu_timeReport();
            }
        }
    }
    static void PrintMenu_report()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== Time Report Menu ===");
        sb.AppendLine("0 - Back to Previous Menu");
        sb.AppendLine("1 - Both on BST, Times for loading each Ordered file compare to Random file");
        sb.AppendLine("2 - Both on AVL, Times for loading each Ordered file compare to Random file");
        sb.AppendLine("3 - Both loading Ordered file, Times for loading on BST compare with on AVL");
        sb.AppendLine("4 - Both loading Random file, Times for loading on BST compare with on AVL");
        sb.AppendLine("=== Time Report Menu ===");
        WriteLine(sb.ToString());
    }
    static void Report_insertion()
    {
        PrintMenu_report();
        int maxNum = 4;
        while (true)
        {
            Write("Enter a number: ");
            int value = Util.GetInt(ReadLine(), maxNum);
            if (value == Util.bad_int)
            {
                WriteWarning("INVALID NUMBER!");
            }
            else if (value == 0)
            {
                return;
            }
            else
            {// valid number selected, then ask comfirmation
                bool toReport = Report_confirm(value);
                if (toReport)
                {
                    if (value == 1)
                    {
                        PrintTimeReport("orderedBST", "randomBST", insertionTimes_orderedBST, insertionTimes_randomBST);
                    }
                    else if (value == 2)
                    {
                        PrintTimeReport("orderedAVL", "randomAVL", insertionTimes_orderedAVL, insertionTimes_randomAVL);
                    }
                    else if (value == 3)
                    {
                        PrintTimeReport("orderedBST", "orderedAVL", insertionTimes_orderedBST, insertionTimes_orderedAVL);
                    }
                    else
                    {
                        PrintTimeReport("randomBST", "randomAVL", insertionTimes_randomBST, insertionTimes_randomAVL);
                    }
                    WriteComplete("TIME REPORT COMPLETED!");
                }
                else
                {
                    WriteWarning("STOP TIME REPORT!");
                }
                Write("- Press any key to continue...");
                ReadLine();
                PrintMenu_report();
            }
        }
    }
    static void Report_searching()
    {
        PrintMenu_report();
        int maxNum = 4;
        while (true)
        {
            Write("Enter a Number: ");
            int value = Util.GetInt(ReadLine(), maxNum);
            if (value == Util.bad_int)
            {
                WriteWarning("INVALID NUMBER!");
            }
            else if (value == 0)
            {
                return;
            }
            else
            {
                bool toReport = Report_confirm(value);
                if (toReport)
                {
                    SearchNodeInALlTrees();
                    if (value == 1)
                    {
                        PrintTimeReport("orderedBST", "randomBST", searchingTimes_orderedBST, searchingTimes_randomBST);
                    }
                    else if (value == 2)
                    {
                        PrintTimeReport("orderedAVL", "randomAVL", searchingTimes_orderedAVL, searchingTimes_randomAVL);
                    }
                    else if (value == 3)
                    {
                        PrintTimeReport("orderedBST", "orderedAVL", searchingTimes_orderedBST, searchingTimes_orderedAVL);
                    }
                    else
                    {
                        PrintTimeReport("randomBST", "randomAVL", searchingTimes_randomBST, searchingTimes_randomAVL);
                    }
                    WriteComplete("TIME REPORT COMPLETED!");
                }// finished or cancelled selected operation
                else
                {
                    WriteWarning("STOP TIME REPORT!");
                }
                Write("- Press any key to continue...");
                ReadLine();
                PrintMenu_report();
            }
        }
    }
    static bool Report_confirm(int value)
    {
        int confirmed = 0;
        while (true)
        {
            if (value == 1)
            {
                Write("- Sure to compare Ordered file with Random file, both of which was loaded on BST: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
            }
            else if (value == 2)
            {
                Write("- Sure to compare Ordered file with Random file, both of which was loaded on AVL: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
            }
            else if (value == 3)
            {
                Write("- Sure to compare file on BST with file on AVL where both Ordered file: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
            }
            else if (value == 4)
            {
                Write("- Sure to compare file on BST with file on AVL where both Random file: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
            }
            // feedback and return
            if (confirmed == 1)
            {
                return true;
            }
            else if (confirmed == 0)
            {
                WriteWarning("OPERATION CANCELLED!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteWarning("INVALID INPUT!");
            }
        }
    }
    static void PrintTimeReport(string left, string right, 
        TimeSpan[] times_left, TimeSpan[] times_right, 
        [CallerMemberName] string callerName = "")
    {
        string unit = callerName == "Report_insertion" ? "ms" : "µs";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"=== Print for Time({unit}) against Words ===");
        sb.AppendLine($"{"WORDS",-15}{left,-15}{right}");
        for (int i = 0; i < fileNum; i++)
        {
            if (callerName == "Report_insertion")
            {
                sb.AppendFormat("{0,-15}{1,-15:N1}{2:N1}\n",
                    words[i], times_left[i].TotalMilliseconds, times_right[i].TotalMilliseconds);
            }
            else
            {
                sb.AppendFormat("{0,-15}{1,-15:N1}{2:N1}\n",
                    words[i], times_left[i].TotalMicroseconds, times_right[i].TotalMicroseconds);
            }
        }
        Write(sb.ToString());
    }
    #endregion
    static void WriteComplete(string data)
    {
        ForegroundColor = ConsoleColor.Green;
        WriteLine(data);
        ResetColor();
    }
    static void WriteWarning(string data)
    {
        ForegroundColor = ConsoleColor.Red;
        WriteLine(data);
        ResetColor();
    }
    static void WriteFeedback(string data)
    {
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(data);
        ResetColor();
    }
}