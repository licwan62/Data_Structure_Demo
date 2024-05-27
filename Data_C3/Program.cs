using System.Text;
using static System.Console;
namespace Data_C3;

internal class Program
{
    static readonly string orderedPath = @"..\..\..\ordered";
    static readonly string randomPath = @"..\..\..\random";
    static readonly string stopChar = "/";
    static readonly string stopInfo = $"(\"{stopChar}\" to stop)";
    static BST_DS? bst_DS = null;
    static AVL_DS? avl_DS = null;
    static bool ordered;
    static bool balanced;
    static string[] names = {
        "1000-words", "5000-words", "10000-words", "15000-words", "20000-words",
        "250000-words","30000-words", "35000-words", "40000-words","45000-words",
        "50000-words"
    };
    static string orderedType
    {
        get => ordered ? "Ordered" : "Random";
    }
    static string treeType
    {
        get => balanced ? "AVL Tree" : "Binery Search Tree";
    }
    static int index;
    static void Main(string[] args)
    {
        ShowMenu_types();
        GetTypes();
    }
    /// <summary>
    /// *** Type Selecting Menu ***
    /// </summary>
    static void ShowMenu_types()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** MENU *****\nChoose File Order and Tree Structure");
        sb.AppendLine("Enter 0 - Exist Program");
        sb.AppendLine("Enter 1 - Load Ordered Text File into Binary Search Tree");
        sb.AppendLine("Enter 2 - Load Random Text File into Binary Search Tree");
        sb.AppendLine("Enter 3 - Load Ordered Text File into AVL Tree");
        sb.AppendLine("Enter 4 - Load Random Text File into AVL Tree");
        Write(sb.ToString());
    }
    /// <summary>
    /// get ordered type of file and tree type where file is loaded
    /// when commitment confirmed -> Loading Menu
    /// </summary>
    static void GetTypes()
    {
        int maxNum = 4;// range for user type
        bool selected = false;// for true when get confirmed to load certain file
                              // then Go to Loading Menu
        while (!selected)
        {
            Write("Enter number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {// Enpty input or value out of range
                WriteLine("Invalid Input, Enter number 0 ~ {0}", maxNum);
            }
            else
            {// valid option chosen
                string? confirmed = "";// read from user
                // get user input to confirmed the commitment
                // assign ordered and balenced according to selection
                switch (value)
                {
                    case 0:
                        Write($"- Sure to Exist Program? {stopInfo}...");
                        if ((confirmed = ReadLine()) != stopChar)
                        {
                            Environment.Exit(0);
                        }
                        break;
                    case 1:
                        Write($"- Sure to Load Ordered Text File into Binary Search Tree? {stopInfo}...");
                        if ((confirmed = ReadLine()) != stopChar)
                        {
                            ordered = true;
                            balanced = false;
                        }
                        break;
                    case 2:
                        Write($"- Sure to Load Random Text File into Binary Search Tree? {stopInfo}...");
                        if ((confirmed = ReadLine()) != stopChar)
                        {
                            ordered = false;
                            balanced = false;
                        }
                        break;
                    case 3:
                        Write($"- Sure to Load Ordered Text File into AVL Tree? {stopInfo}...");
                        if ((confirmed = ReadLine()) != stopChar)
                        {
                            ordered = true;
                            balanced = true;
                        }
                        break;
                    case 4:
                        Write($"- Sure to Load Random Text File into AVL Tree? {stopInfo}...");
                        if ((confirmed = ReadLine()) != stopChar)
                        {
                            ordered = false;
                            balanced = true;
                        }
                        break;
                    default: break;
                }
                if (confirmed == stopChar)
                {
                    // redo Type Selecting Menu
                    ShowMenu_types();
                }
                else
                {
                    selected = true;
                    ShowMenu_loading();
                    GetFile();
                }
            }
        }
    }
    /// <summary>
    /// *** Loading Menu ***
    /// </summary>
    static void ShowMenu_loading()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"***** Loading Menu *****\nChoose Loading File");
        sb.AppendLine("Enter 0  - Turn Back to Previous Menu");
        for (int i = 0; i < 11; i++)
        {
            sb.AppendLine($"Enter {i + 1}  - {names[i]}");
        }
        Write(sb.ToString());
    }
    /// <summary>
    /// get the file to be loaded
    /// </summary>
    static void GetFile()
    {
        // Initialize tree
        bst_DS = null;
        avl_DS = null;
        int maxNum = 11;
        bool selected = false;
        while (!selected)
        {
            Write("Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {
                WriteLine("Invalid number, Enter number 0 ~ {0}", maxNum);
            }
            else
            {// loading file is chosen
                string? confirmed = "";
                // get user input to confirm commitment
                switch (value)
                {
                    case 0:
                        Write($"- Sure to Go back? {stopInfo}...");
                        if ((confirmed = ReadLine()) != stopChar)
                        {
                            ShowMenu_types();
                            GetTypes();
                            return;
                        }
                        break;
                    default:
                        Write($"Sure to Load {orderedType} {names[value - 1]} into {treeType}? {stopInfo}...");
                        if ((confirmed = ReadLine()) != stopChar)
                        {

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
                            }
                        }
                        break;
                }
                if (confirmed == stopChar)
                {// did not load, to show loading menu again
                    ShowMenu_loading();
                }
                else
                {// committed loading (case 0 get returned earlier)
                    if (avl_DS != null || bst_DS != null)
                    {
                        Write("Press any key to continue...");
                        ReadKey();
                        selected = true;// break menu loop
                        ShowMenu_function();
                        GetFunction();
                    }
                    else
                    {
                        Write($"Failed to Load {treeType}, Press any key to continue...");
                        ReadKey();
                        ShowMenu_loading();
                    }
                }
            }
        }
    }
    static void ShowMenu_function()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** Function Menu *****");
        sb.AppendLine("0 - Back to Loading Menu");
        sb.AppendLine($"1 - Print this {treeType}");
        sb.AppendLine("2 - Insert New Word");
        sb.AppendLine("3 - Delete a Word");
        sb.AppendLine("4 - Find a Word");
        Write(sb.ToString());
    }
    static void GetFunction()
    {
        int maxNum = 4;
        while (true)
        {
            Write("Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {
                WriteLine("Invalid number, Enter number 0 ~ {0}", maxNum);
            }
            else
            {
                bool stopped = false;
                switch (value)
                {
                    case 0:// back
                        Write("Sure to Back to Loading Menu? {0}...", stopInfo);
                        if (ReadLine() != stopChar)
                        {
                            ShowMenu_loading();
                            GetFile();
                        }
                        break;
                    case 1:// print
                        Print();
                        break;
                    case 2:// insert
                        stopped = !Insert();
                        break;
                    case 3:// delete
                        stopped = !Delete();
                        break;
                    case 4:
                        stopped = !Find();
                        break;
                }
                if (!stopped)
                {
                    Write("Press any key to continue...");
                    ReadKey();
                    ShowMenu_function(); 
                }
            }
        }
    }
    static void Print()
    {
        Write("Sure to Print the {0}? {1}...", treeType, stopInfo);
        if (ReadLine() != stopChar)
        {
            if (balanced)
            {
                if (avl_DS != null)
                {
                    Write(avl_DS.PreOrderPrint());
                }
            }
            else
            {
                if (bst_DS != null)
                {
                    Write(bst_DS.PreOrderPrint());
                }
            }
        }
        else
        {
            WriteLine("Stopped Printing!");
            ShowMenu_function();
        }
    }
    static bool Insert()
    {
        string? newWord;
        while (true)
        {
            // 1 get new word
            Write($"Enter your New Word {stopInfo}: ");
            if ((newWord = ReadLine()) != stopChar)
            {// new word entered
                if (string.IsNullOrEmpty(newWord))
                {
                    WriteLine("Empty new word, Enter again!");
                    continue;
                }
                // 2 get confirmed
                Write("Sure to Insert \"{0}\"? {1}...", newWord, stopInfo);
                if (ReadLine() != stopChar)
                {// confirmed to insert
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
                    // 3 ask if continue
                    Write("Continue adding new word? {0}...", stopInfo);
                    if (ReadLine() != stopChar)
                    {// continue adding new
                        ShowMenu_function();
                        continue;
                    }
                    else
                    {// not continued, back to function menu
                        ShowMenu_function();
                        return true;
                    }
                }
                else
                {// not confirmed to insert
                    WriteLine("Stopped Inserting!");
                    ShowMenu_function();
                    return false;
                }
            }
            else
            {// new word not entered
                ShowMenu_function();
                return false;
            }
        }
    }
    static bool Delete()
    {
        string? wordToDelete;
        while (true)
        {
            // 1 get word
            Write($"Enter your New Word {stopInfo}: ");
            if ((wordToDelete = ReadLine()) != stopChar)
            {// new word entered
                if (string.IsNullOrEmpty(wordToDelete))
                {
                    WriteLine("Empty word, Enter again!");
                    continue;
                }
                // 2 get confirmed
                Write("Sure to Delete \"{0}\"? {1}...", wordToDelete, stopInfo);
                if (ReadLine() != stopChar)
                {// confirmed to delete
                    if (balanced)
                    {
                        // delete in avl
                    }
                    else
                    {
                        // delete in bst
                    }
                    // 3 ask if continue
                    WriteLine("Continue Deleting word? {0}...", stopInfo);
                    if (ReadLine() != stopChar)
                    {// continue deleting another
                        ShowMenu_function();
                        continue;
                    }
                    else
                    {// not continued, back to function menu
                        ShowMenu_function();
                        return true;
                    }
                }
                else
                {// not confirmed to delete
                    WriteLine("Stopped Deleting!");
                    ShowMenu_function();
                    return false;
                }
            }
            else
            {// word not entered
                ShowMenu_function();
                return false;
            }
        }
    }
    static bool Find()
    {
        string? wordToFind;
        while (true)
        {
            // 1 get finding word
            Write($"Enter Searching word {stopInfo}: ");
            if ((wordToFind = ReadLine()) != stopChar)
            {// new word entered
                if (string.IsNullOrEmpty(wordToFind))
                {
                    WriteLine("Empty word, Enter again!");
                    continue;
                }
                // 2 get confirmed
                Write("Sure to Search \"{0}\"? {1}...", wordToFind, stopInfo);
                if (ReadLine() != stopChar)
                {// confirmed
                    if (balanced)
                    {
                        if (avl_DS != null)
                        {
                            WriteLine(avl_DS.Find(wordToFind));
                        }
                    }
                    else
                    {
                        if (bst_DS != null)
                        {
                            WriteLine(bst_DS.Find(wordToFind));
                        }
                    }
                    // 3 ask if continue
                    Write("Continue Searching word? {0}...", stopInfo);
                    if (ReadLine() != stopChar)
                    {// continue searching
                        ShowMenu_function();
                        continue;
                    }
                    else
                    {// not continued, back to function menu
                        ShowMenu_function();
                        return true;
                    }
                }
                else
                {// not confirmed
                    WriteLine("Stopped Searching!");
                    ShowMenu_function();
                    return false;
                }
            }
            else
            {// word not entered
                ShowMenu_function();
                return false;
            }
        }
    }
    static BST_DS Load_BST(string path, int index)
    {
        BST_DS ds = new BST_DS();
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        if (dirInfo.Exists)
        {
            FileInfo[] fileInfos = dirInfo.GetFiles().OrderBy(f => f.Length).ToArray();
            FileInfo fileInfo = fileInfos[index];
            ds.Name = orderedType + "_" + fileInfo.Name;
            StreamReader sr = fileInfo.OpenText();
            string? line;
            Write($"{fileInfo.Name} is Loading... ");
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
            FileInfo fileInfo = fileInfos[index];
            ds.Name = orderedType + "_" + fileInfo.Name;
            // add words into avl tree
            Write($"{fileInfo.Name} is Loading...");
            StreamReader sr = fileInfo.OpenText();
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
}