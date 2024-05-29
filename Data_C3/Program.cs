using System.Text;
using static System.Console;
namespace Data_C3;

internal class Program
{
    static readonly string orderedPath = @"..\..\..\ordered";
    static readonly string randomPath = @"..\..\..\random";
    static readonly string inpGuide = "[Y/N]";
    static BST_DS? bst_DS = null;
    static AVL_DS? avl_DS = null;
    static bool ordered;// true when chosen file is ordered words
    static bool balanced;// true when loaded on AVL Tree
    static string[] names = {
        "1000-words", "5000-words", "10000-words", "15000-words", "20000-words",
        "25000-words","30000-words", "35000-words", "40000-words","45000-words",
        "50000-words"
    };
    static FileInfo fileToLoad = null;
    static string orderedType { get => ordered ? "Ordered" : "Random"; }
    static string treeType { get => balanced ? "AVL Tree" : "Binery Search Tree"; }
    static int index;
    static void Main(string[] args)
    {
        // Main Menu: get tree and file type
        // Loading Menu: get specified file for loading
        // Function Menu: get function on the file
        GetMethod();
    }
    static void WriteStopMSG(string operation)
    {
        WriteLine("Stopped {0}! Press any key to continue...", operation);
        ReadKey();
    }
    #region Main Menu
    static void PrintMenu_main()
    {
        Clear();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** Main Menu *****");
        sb.AppendLine("Choose File Order and Tree Structure");
        sb.AppendLine("Enter 0 - Exist Program");
        sb.AppendLine("Enter 1 - Load Ordered Text File into Binary Search Tree");
        sb.AppendLine("Enter 2 - Load Random Text File into Binary Search Tree");
        sb.AppendLine("Enter 3 - Load Ordered Text File into AVL Tree");
        sb.AppendLine("Enter 4 - Load Random Text File into AVL Tree");
        Write(sb.ToString());
    }

    /// <summary>
    /// select on Main Menu
    /// get ordered type of file and tree type where file is loaded
    /// select option to assign corresponding bools to {ordered} and {balanced}
    /// </summary>
    static void GetMethod()
    {
        int maxNum = 4;// range for user type
        while (true)
        {
            PrintMenu_main();
            Write("Enter number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {// Enpty input or value out of range
                WriteLine("Invalid Input, Enter number 0 ~ {0}", maxNum);
            }
            else
            {// valid option chosen
                ConfirmToGetMethod(value);
            }
        }
    }
    static void ConfirmToGetMethod(int value)
    {
        int confirmed;
        // option selected then get confirmed to operate or not
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
                else if (confirmed == 0)
                {
                    WriteStopMSG("Existing");
                    break;
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input {0} ", inpGuide);
                }
            }
            else if (value == 1)
            {
                Write("- Sure to Load Ordered Text File into Binary Search Tree? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    ordered = true;
                    balanced = false;
                    break;
                }
                else if (confirmed == 0)
                {
                    break;
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input {0} ", inpGuide);
                }
            }
            else if (value == 2)
            {
                Write("- Sure to Load Random Text File into Binary Search Tree? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    ordered = false;
                    balanced = false;
                    break;
                }
                else if (confirmed == 0)
                {
                    break;
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input!");
                }
            }
            else if (value == 3)
            {
                Write("- Sure to Load Ordered Text File into AVL Tree? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    ordered = true;
                    balanced = true;
                    break;
                }
                else if (confirmed == 0)
                {
                    break;
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input!");
                }
            }
            else if (value == 4)
            {
                Write("- Sure to Load Random Text File into AVL Tree? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
                {
                    ordered = false;
                    balanced = true;
                    break;
                }
                else if (confirmed == 0)
                {
                    break;
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input!");
                }
            }
        }
        // reachable when confirmed = 1 or 0
        if (confirmed == 1)
        {
            Write("Option {0} is chosen, Press any key to continue...", value);
            ReadKey();
            // go to Loading Menu
            GetFile();
            // returned when choosing to back
        }
        else if (confirmed == 0)
        {
            WriteStopMSG("Operating");
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
        sb.AppendLine("0  - Turn Back to Previous Menu");
        for (int i = 0; i < 11; i++)
        {
            sb.AppendLine($"{i + 1} - {names[i]}");
        }
        Write(sb.ToString());
    }
    static void GetFile()
    {
        int maxNum = 11;
        while (true)
        {
            PrintMenu_loading();
            // Initialize tree
            bst_DS = new BST_DS();
            avl_DS = new AVL_DS();

            Write("- Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {// invalid optional number
                WriteLine("Invalid number, Enter number 0 ~ {0}", maxNum);
            }
            else
            {// valid option number
                if (value == 0)
                {// option to back
                    return;
                }
                else
                {// option to load
                    ConfirmToGetFile(value);
                    // returned when confirmed not to load or loading failed
                }
            }
        }
    }
    static void ConfirmToGetFile(int value)
    {
        while (true)
        {
            Write($"- Sure to Load {orderedType} {names[value - 1]} on {treeType}? {inpGuide} ");
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
                break;
            }
            else if (confirmed == 0)
            {
                WriteLine("Loading is Cancelled!");
                ReadKey();
                return;
            }
            else if (confirmed == -1)
            {
                WriteLine("Invalid Input {0} ", inpGuide);// do loop to ask again
            }
        }// get confirmed and file loaded
        if (avl_DS != null || bst_DS != null)
        {// loading succeed
            Write("File Loading Complete, Press any key to continue Functional Selection...");
            ReadKey();
            GetFunction(); // returned when choosing back option in Function Menu
        }
        else
        {// loading failed
            Write("File Loading Failed, Press any key back to Loading Menu...");
            ReadKey();
            return;
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
        sb.AppendLine("4 - Find");
        Write(sb.ToString());
    }
    static void GetFunction()
    {
        int maxNum = 4;
        while (true)
        {
            PrintMenu_function();
            Write("Enter a number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {
                WriteLine("Invalid Number, Enter number 0 ~ {0}", maxNum);
            }
            else
            {
                switch (value)
                {
                    case 0: return;
                    case 1: Print(); break;
                    case 2: Insert(); break;
                    case 3: Delete(); break;
                    case 4: Find(); break;
                }
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
                WriteLine("Invalid Number, Enter number 0 ~ {0}", maxNum);
            }
            else
            {// valid number
                if (orderNum == 0)
                {
                    // return back
                    WriteStopMSG("Printing");
                    break;
                }
                else
                {
                    // 2 get confirmation
                    ConfirmToPrint(orderNum);
                    break;
                }
            }
        }
    }
    static void ConfirmToPrint(int orderNum)
    {
        int confirmed;
        while (true)
        {
            if (orderNum == 1)
            {// inorder
                Write("- Sure to Print the tree in InOrder? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
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
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input!");
                }
                if (confirmed == 1 || confirmed == 0)
                {
                    break;
                }
            }
            else if (orderNum == 2)
            {// preorder
                Write("- Sure to Print the tree in PreOrder? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
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
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input!");
                }
                if (confirmed == 1 || confirmed == 0)
                {
                    break;
                }
            }
            else if (orderNum == 3)
            {// postorder
                Write("- Sure to Print the tree in PostOrder? {0} ", inpGuide);
                confirmed = Util.IsSure(ReadLine());
                if (confirmed == 1)
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
                }
                else if (confirmed == -1)
                {
                    WriteLine("Invalid Input!");
                }
                if (confirmed == 1 || confirmed == 0)
                {
                    break;
                }
            }
        }
        // reachable when confirmed = 1 or 0
        if (confirmed == 1)
        {
            WriteLine("Printing Complete! Press any key to continue...");
            ReadKey();
        }
        else if (confirmed == 0)
        {
            WriteStopMSG("Printing");
        }
    }
    #endregion Print

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
                ConfirmToInsert(newWord);
                // returned when inserted or stopped
                break;
            }
        }
    }
    static void ConfirmToInsert(string newWord)
    {
        int confirmed;
        while (true)
        {
            Write("- Sure to Insert \"{0}\"? {1} ", newWord, inpGuide);
            confirmed = Util.IsSure(ReadLine());
            if (confirmed == 1)
            {
                // insert
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
                WriteLine("Insertion Complete! Press any key to Function Menu...");
                ReadKey();
                break;
            }
            else if (confirmed == 0)
            {
                WriteStopMSG("Inserting");
                break;
            }
            else if (confirmed == -1)
            {
                WriteLine("Invalid Input!");
            }
        }
    }
    #endregion Insert

    #region Delete
    static void Delete()
    {
        string? deleteWord;
        while (true)
        {
            // 1 get new word
            Write("- Enter your New Word: ");
            deleteWord = ReadLine();
            if (string.IsNullOrEmpty(deleteWord))
            {
                WriteLine("Empty word!");
            }
            else
            {// Not Empty word
             // 2 get confirmation
                ConfirmToDelete(deleteWord);
                // returned when inserted or stopped
                break;
            }
        }
    }
    static void ConfirmToDelete(string deleteWord)
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
                break;
            }
            else if (confirmed == 0)
            {
                WriteStopMSG("Deleting");
                break;
            }
            else if (confirmed == -1)
            {
                WriteLine("Invalid Input!");
            }
        }
    }
    #endregion Delete
    static void Find()
    {
        
    }
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
            Write($"{fileToLoad.Name} is Loading... ");
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
            Write($"{fileToLoad.Name} is Loading...");
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
}