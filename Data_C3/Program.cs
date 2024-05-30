using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using static System.Console;
namespace Data_C3;

internal class Program
{
    static readonly string orderedPath = @"..\..\..\ordered";
    static readonly string randomPath = @"..\..\..\random";
    static readonly string inpGuide = "[Y/N]";

    // indicate file index and option name in main menu
    static int fileNum;
    static int[] words =
    {
        1000,5000,10000,15000,20000,25000,30000,35000,40000,45000,50000
    };
    static string[] fileNames = null!;

    // indicate decided type of file and tree at loading menu
    static bool ordered = false;
    static bool balanced = false;
    static string orderedType { get => ordered ? "Ordered" : "Random"; }
    static string treeType { get => balanced ? "AVL" : "BST"; }

    // loaded tree before function test
    static BST_DS bst_DS = null!;
    static AVL_DS avl_DS = null!;

    // file to be loaded that selected at loading menu
    static FileInfo fileToLoad = null!;

    // time recorded before printing time comparison
    static TimeSpan[] ordered_BST = null!;
    static TimeSpan[] random_BST = null!;
    static TimeSpan[] ordered_AVL = null!;
    static TimeSpan[] random_AVL = null!;
    static void Main(string[] args)
    {
        // generate options on main menu
        DirectoryInfo orderDirInfo = new DirectoryInfo(orderedPath);
        DirectoryInfo randomDirInfo = new DirectoryInfo(randomPath);
        fileNum = int.Min(orderDirInfo.GetFiles().Length, randomDirInfo.GetFiles().Length);
        fileNames = new string[fileNum];
        for (int i = 0; i < fileNum; i++)
        {
            fileNames[i] = words[i].ToString() + "-words";
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
        sb.AppendLine("***** Main Menu *****");
        sb.AppendLine("Choose File Order and Tree Structure");
        sb.AppendLine("0 - Exist Program");
        sb.AppendLine("1 - Load ORDERED File into BST");
        sb.AppendLine("2 - Load RANDOM File into BST");
        sb.AppendLine("3 - Load ORDERED File into AVL");
        sb.AppendLine("4 - Load RANDOM File into AVL");
        sb.AppendLine("5 - Load all file and Get Time Report");
        Write(sb.ToString());
    }
    /// <summary>
    /// select on Main Menu
    /// get ordered type of file and tree type where file is loaded
    /// select option to assign corresponding bools to {ordered} and {balanced}
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
                WriteLine("Invalid Input, - Enter a new number 0 ~ {0}", maxNum);
            }
            else if (value == 5)
            {
                bool confirmed = Check_main(value);
                if (confirmed)
                {
                    LoadAllFiles();

                    WriteLine("Complete loading all files of both ordered and random on both AVL and BST");
                    Write("Press any key to continue...");
                    ReadKey();

                    GetTimeReport();
                    // returned when back from time report menu
                    PrintMenu_main();
                }
                else
                {
                    Write("Press any key to continue...");
                    ReadKey();
                    PrintMenu_main();
                }
            }
            else
            {// valid option chosen except for 5, get confirmed to continue or loop
                bool confirmed = Check_main(value);
                if (confirmed)
                {
                    GetFile();
                    // returned when back from loading munu
                    PrintMenu_main();
                }
                else
                {
                    Write("Press any key to continue...");
                    ReadKey();
                    PrintMenu_main();
                }
            }
        }
    }
    static bool Check_main(int value)
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
                    ordered = true;
                    balanced = false;
                    WriteLine("RANDOM file Loaded(AVL)");
                }
            }
            else if (value == 2)
            {
                Write("- Sure to Load Random File into Binary Search Tree? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    ordered = false;
                    balanced = false;
                    WriteLine("RANDOM file Loaded(AVL)");
                }
            }
            else if (value == 3)
            {
                Write("- Sure to Load Ordered File into AVL Tree? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    ordered = true;
                    balanced = true;
                    WriteLine("RANDOM file Loaded(AVL)");
                }
            }
            else if (value == 4)
            {
                Write("- Sure to Load Random File into AVL Tree? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    ordered = false;
                    balanced = true;
                    WriteLine("RANDOM file Loaded(AVL)");
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
                    WriteLine("INVALID INPUT!");
                    break;// continue loop
                case 0:
                    WriteLine("OPERATION CANCELLED!");
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
        sb.AppendLine($"***** Loading Menu *****");
        sb.AppendLine($"Current File Order: {orderedType}, Current Tree Structure: {treeType}");
        sb.AppendLine("Choose the File to get loaded on the Tree");
        sb.AppendLine("0  - Back to Main Menu");
        for (int i = 0; i < fileNum; i++)
        {
            sb.AppendLine($"{i + 1} - {fileNames[i]}");
        }
        Write(sb.ToString());
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
                WriteLine("INVALID INPUT! - Enter a number 0 ~ {0}", maxNum);
            }
            else if (value == 0)
            {// option to back
                return;
            }
            else
            {// option to load
                bool confirmed = Check_loading(value);
                if (confirmed)
                {// loading succeed
                    Write("- Press any key to Function Menu...");
                    ReadKey();

                    GetFunction();
                    // returned when back from Function Menu

                    PrintMenu_loading();
                }
                else
                {// not loaded
                    WriteLine("- Press any key to continue...");
                    ReadKey();
                    PrintMenu_loading();
                }
            }
        }
    }
    static bool Check_loading(int value)
    {
        while (true)
        {
            Write($"- Sure to Load {orderedType} {fileNames[value - 1]} on {treeType}? {inpGuide} ");
            int confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {// get confirmed to load selected file 
                if (ordered && balanced)
                {// ordered file on avl
                    avl_DS = Load_AVL(orderedPath, value - 1);
                }
                else if (ordered)
                {// ordered file on bst
                    bst_DS = Load_BST(orderedPath, value - 1);
                }
                else if (balanced)
                {// random file on avl
                    avl_DS = Load_AVL(randomPath, value - 1);
                }
                else
                {// random file on avl
                    bst_DS = Load_BST(randomPath, value - 1);
                }// corresponding file is loaded
                WriteLine("File Loading Complete!");
                return true;
            }
            else if (confirmed == 0)
            {// confirmed not
                WriteLine("STOP LOADING!");
                return false;
            }
            else if (confirmed == -1)
            {
                WriteLine("INVALID INPUT!");
            }
        }
    }
    #endregion Loading Menu

    #region Funtion Menu
    static void PrintMenu_function()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** Function Menu *****");
        sb.AppendLine($"Current File {orderedType}_{fileToLoad.Name}, Current Tree Structure: {treeType}");
        sb.AppendLine("0 - Back to Loading Menu");
        sb.AppendLine("1 - Print");
        sb.AppendLine("2 - Insert");
        sb.AppendLine("3 - Delete");
        sb.AppendLine("4 - Search");
        Write(sb.ToString());
    }
    static void GetFunction()
    {
        int maxNum = 4;

        PrintMenu_function();
        while (true)
        {
            Write("Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {
                WriteLine("INVALID INPUT! - Enter a number 0 ~ {0}", maxNum);
            }
            else if (value == 0)
            {
                return;
            }
            else
            {
                switch (value)
                {
                    case 1: Print(); break;
                    case 2: Insert(); break;
                    case 3: Delete(); break;
                    case 4: Search(); break;
                }
                // get function again
                WriteLine("Press any key to continue...");
                ReadKey();
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
        sb.AppendLine("***** Print Order *****");
        sb.AppendLine($"Current File {orderedType}_{fileToLoad.Name}, Current Tree Structure: {treeType}");
        sb.AppendLine("0 - Return to Function Menu");
        sb.AppendLine("1 - Inorder");
        sb.AppendLine("2 - Preorder");
        sb.AppendLine("3 - Postorder");
        Write(sb.ToString());
    }
    /// <summary>
    /// 1 get print order
    /// 2 get confirmation
    /// </summary>
    static void Print()
    {
        int maxNum = 3;

        PrintMenu_order();
        while (true)
        {// 1 get print order
            Write("Enter Print Order: ");
            string? input = ReadLine();
            int orderNum = Util.GetInt(input, maxNum);
            if (orderNum == Util.bad_int)
            {
                WriteLine("INVALID NUMBER! - Enter a number 0 ~ {0}", maxNum);
            }
            else if (orderNum == 0)
            {// option to back
                WriteLine("STOP PRINTING!");
                return;
            }
            else
            {// 2 get confirmation
                Check_print(orderNum);
                // returned whether printed or cancelled
                return;
            }
        }
    }
    static void Check_print(int orderNum)
    {
        int confirmed = 0;
        while (true)
        {
            if (orderNum == 1)
            {// inorder
                Write("- Sure to Print the tree in InOrder? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    if (balanced)
                    {
                        if (avl_DS != null)
                            Write(avl_DS.InOrderPrint());
                    }
                    else
                    {
                        if (bst_DS != null)
                            Write(bst_DS.InOrderPrint());
                    }
                    return;
                }
            }
            else if (orderNum == 2)
            {// preorder
                Write("- Sure to Print the tree in PreOrder? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    if (balanced)
                    {
                        if (avl_DS != null)
                            Write(avl_DS.PreOrderPrint());
                    }
                    else
                    {
                        if (bst_DS != null)
                            Write(bst_DS.PreOrderPrint());
                    }
                    return;
                }
            }
            else if (orderNum == 3)
            {// postorder
                Write("- Sure to Print the tree in PostOrder? {0} ", inpGuide);
                if ((confirmed = Util.IsSure(ReadLine())) == 1)
                {
                    if (balanced)
                    {
                        if (avl_DS != null)
                            Write(avl_DS.PostOrderPrint());
                    }
                    else
                    {
                        if (bst_DS != null)
                            Write(bst_DS.PostOrderPrint());
                    }
                    return;
                }
            }
            if (confirmed == 0)
            {
                WriteLine("STOP PRINTING!");
                return;
            }
            else if (confirmed == -1)
            {
                WriteLine("INVALID INPUT!");
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
                WriteLine("Empty word!");
            }
            else
            {// Not Empty word
             // 2 get confirmation
                Check_insert(newWord);
                // returned when inserted or stopped
                return;
            }
        }
    }
    static void Check_insert(string newWord)
    {
        int confirmed;
        while (true)
        {
            Write("- Sure to Insert \"{0}\"? {1} ", newWord, inpGuide);
            confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                if (balanced)
                {
                    if (avl_DS != null)
                        WriteLine(avl_DS.Add(newWord));
                }
                else
                {
                    if (bst_DS != null)
                        WriteLine(bst_DS.Add(newWord));
                }
                return;
            }
            else if (confirmed == 0)
            {
                WriteLine("STOP INSERTING!");
                return;
            }
            else if (confirmed == -1)
            {
                WriteLine("INVALID INPUT!");
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
                WriteLine("Empty word!");
            }
            else
            {// Not Empty word
             // 2 get confirmation
                Check_delete(deleteWord);
                // returned when inserted or stopped
                return;
            }
        }
    }
    static void Check_delete(string deleteWord)
    {
        int confirmed;
        while (true)
        {
            Write("- Sure to Delete \"{0}\"? {1} ", deleteWord, inpGuide);
            confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                // insert
                if (balanced)
                {
                    if (avl_DS != null)
                        WriteLine(avl_DS.Delete(deleteWord));
                }
                else
                {
                    if (bst_DS != null)
                        WriteLine(bst_DS.Delete(deleteWord));
                }
                WriteLine("Deleting Complete! Press any key to Function Menu...");
                ReadKey();
                return;
            }
            else if (confirmed == 0)
            {
                WriteLine("STOP DELETING!");
                return;
            }
            else if (confirmed == -1)
            {
                WriteLine("INVALID INPUT!");
            }
        }
    }
    #endregion

    #region Search
    static void Search()
    {
        string? wordToSearch;
        while (true)
        {
            // 1 get new word
            Write("- Enter Word to Search: ");
            wordToSearch = ReadLine();
            if (string.IsNullOrEmpty(wordToSearch))
            {
                WriteLine("Empty word!");
            }
            else
            {// Not Empty word
             // 2 get confirmation
                Check_search(wordToSearch);
                // returned when inserted or stopped
                return;
            }
        }
    }
    static void Check_search(string wordToSearch)
    {
        int confirmed;
        while (true)
        {
            Write("- Sure to Search for \"{0}\"? {1} ", wordToSearch, inpGuide);
            confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                if (balanced)
                {
                    if (avl_DS != null)
                        WriteLine(avl_DS.Search(wordToSearch));
                }
                else
                {
                    if (bst_DS != null)
                        WriteLine(bst_DS.Search(wordToSearch));
                }
                WriteLine("Searching Complete! Press any key to Function Menu...");
                ReadKey();
                return;
            }
            else if (confirmed == 0)
            {
                WriteLine("STOP SEARCHING!");
                return;
            }
            else if (confirmed == -1)
            {
                WriteLine("INVALID INPUT!");
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
            ds.Name = orderedType + "_" + fileToLoad.Name;
            StreamReader sr = fileToLoad.OpenText();
            string? line;
            Write($"{orderedType}_{fileToLoad.Name} is Loading on {treeType}... ");
            while ((line = sr.ReadLine()) != null)
            {
                ds.Add(line);
            }
            WriteLine($"Complete! {treeType} Contain {ds.Count} words");
        }
        else
        {
            WriteLine($"{path} Not Exists");
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
            ds.Name = orderedType + "_" + fileToLoad.Name;
            // add words into avl tree
            Write($"{orderedType}_{fileToLoad.Name} is Loading on {treeType}...");
            StreamReader sr = fileToLoad.OpenText();
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                ds.Add(line);
            }
            WriteLine($"Complete! {treeType} Contain {ds.Count} words");
        }
        else
        {
            WriteLine($"{path} Not Exists");
        }
        return ds;
    }
    /// <summary>
    /// Loading both RANDOM and ORDERED files both on BST and AVL
    /// loaded then select on Comparison Menu for printing compared time data
    /// </summary>
    static void LoadAllFiles()
    {
        Stopwatch stopwatch = new Stopwatch();
        ordered_BST = new TimeSpan[fileNum];
        random_BST = new TimeSpan[fileNum];
        ordered_AVL = new TimeSpan[fileNum];
        random_AVL = new TimeSpan[fileNum];
        // get all ordered file for BST
        for (int i = 0; i < fileNum; i++)
        {
            stopwatch.Start();

            ordered = true;
            balanced = false;
            bst_DS = Load_BST(orderedPath, i);

            stopwatch.Stop();
            ordered_BST[i] = stopwatch.Elapsed;
        }
        // get all random file for BST
        for (int i = 0; i < fileNum; i++)
        {
            stopwatch.Start();

            ordered = false;
            balanced = false;
            bst_DS = Load_BST(randomPath, i);

            stopwatch.Stop();
            random_BST[i] = stopwatch.Elapsed;
        }
        // get all ordered file for AVL
        for (int i = 0; i < fileNum; i++)
        {
            stopwatch.Start();

            ordered = true;
            balanced = true;
            avl_DS = Load_AVL(orderedPath, i);

            stopwatch.Stop();
            ordered_AVL[i] = stopwatch.Elapsed;
        }
        // get all random file for AVL
        for (int i = 0; i < fileNum; i++)
        {
            stopwatch.Start();

            ordered = false;
            balanced = true;
            avl_DS = Load_AVL(randomPath, i);

            stopwatch.Stop();
            random_AVL[i] = stopwatch.Elapsed;
        }
    }
    #endregion

    #region TimeReport
    static void PrintMenu_timeReport()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** Comparison Menu *****");
        sb.AppendLine("0 - Back to Main Menu");
        sb.AppendLine("1 - Both on BST, Times for loading each ORDERED file compare to RANDOM file");
        sb.AppendLine("2 - Both on AVL, Times for loading each ORDERED file compare to RANDOM file");
        sb.AppendLine("3 - Both loading ORDERED file, Times for loading on BST compare with on AVL");
        sb.AppendLine("4 - Both loading RANDOM file, Times for loading on BST compare with on AVL");
        Write(sb.ToString());
    }
    static void GetTimeReport()
    {
        PrintMenu_timeReport();
        int maxNum = 4;
        while (true)
        {
            Write("Enter a number: ");
            int value = Util.GetInt(ReadLine(), maxNum);
            if (value == Util.bad_int)
            {
                Write("INVALID NUMBER! - Enter a new number 0 ~ {0}: ", maxNum);
            }
            else if (value == 0)
            {
                return;
            }
            else
            {// valid number selected, then ask comfirmation
                Check_timeReport(value);

                WriteLine("Press any key to continue...");
                ReadKey();

                PrintMenu_timeReport();
            }
        }
    }
    static void Check_timeReport(int value)
    {
        int confirmed = 0;
        while (true)
        {
            if (value == 1)
            {
                Write("- Sure to compare ORDERED file with RANDOM file, both of which was loaded on BST: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    PrintTimeComparison("ordered_BST", "random_BST", ordered_BST, random_BST);
                    return;
                }
            }
            else if (value == 2)
            {
                Write("- Sure to compare ORDERED file with RANDOM file, both of which was loaded on AVL: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    PrintTimeComparison("ordered_AVL", "random_AVL", ordered_AVL, random_AVL);
                    return;
                }
            }
            else if (value == 3)
            {
                Write("- Sure to compare file on BST with file on AVL where both ORDERED file: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    PrintTimeComparison("ordered_BST", "ordered_AVL", ordered_BST, ordered_AVL);
                    return;
                }
            }
            else if (value == 4)
            {
                Write("- Sure to compare file on BST with file on AVL where both RANDOM file: {0}", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    PrintTimeComparison("random_BST", "random_AVL", random_BST, random_AVL);
                    return;
                }
            }
            // feedback and return
            if (confirmed == 0)
            {
                WriteLine("OPERATION CANCELLED!");
                return;
            }
            else if (confirmed == -1)
            {
                WriteLine("INVALID INPUT!");
            }
        }
    }
    static void PrintTimeComparison(string left, string right, TimeSpan[] times_left, TimeSpan[] times_right)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** Print Loading Time against Words *****");
        sb.AppendLine($"{"WORDS",-15}{left,-15}{right}");
        for (int i = 0; i < fileNum; i++)
        {
            sb.AppendFormat("{0,-15}{1,-15:N2}{2:N2}\n",
                words[i], times_left[i].TotalSeconds, times_right[i].TotalSeconds);
        }
        Write(sb.ToString());
    }
}
#endregion