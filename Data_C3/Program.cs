using System.Text;
using static System.Console;
namespace Data_C3;

internal class Program
{
    static readonly string orderedPath = @"..\..\..\ordered";
    static readonly string randomPath = @"..\..\..\random";
    static readonly string stopInput = "/";
    static readonly string stopInfo = $"(press \"{stopInput}\" to stop)";
    static BST_DS bst_DS;
    static AVL_DS avl_DS;
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
        ShowMenu_selectType();
        SelectMenu_selectType();
    }
    /// <summary>
    /// show menu for File type and tree type where file is loaded
    /// </summary>
    static void ShowMenu_selectType()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** MENU *****\nLoading Method and File Order");
        sb.AppendLine("Enter 0 - Exist Program");
        sb.AppendLine("Enter 1 - Load Ordered Text File into Binary Search Tree");
        sb.AppendLine("Enter 2 - Load Random Text File into Binary Search Tree");
        sb.AppendLine("Enter 3 - Load Ordered Text File into AVL Tree");
        sb.AppendLine("Enter 4 - Load Random Text File into AVL Tree");
        Write(sb.ToString());
    }
    /// <summary>
    /// get ordered type of file and tree type where file is loaded
    /// </summary>
    static void SelectMenu_selectType()
    {
        int maxNum = 4;
        bool selected = false;// 
        while (!selected)
        {
            Write("Enter number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {// wrong input
                WriteLine("Invalid Input, Enter number 0 ~ {0}", maxNum);
            }
            else
            {// option is chosen
                string? confirm = "";
                // get user input to confirm commitment
                switch (value)
                {
                    case 0:
                        Write($"- Sure to Exist Program? {stopInfo}...");
                        if ((confirm = ReadLine()) != stopInput)
                        {
                            Environment.Exit(0);
                        }
                        break;
                    case 1:
                        Write($"- Sure to Load Ordered Text File into Binary Search Tree? {stopInfo}...");
                        if ((confirm = ReadLine()) != stopInput)
                        {
                            ordered = true;
                            balanced = false;
                        }
                        break;
                    case 2:
                        Write($"- Sure to Load Random Text File into Binary Search Tree? {stopInfo}...");
                        if ((confirm = ReadLine()) != stopInput)
                        {
                            ordered = false;
                            balanced = false;
                        }
                        break;
                    case 3:
                        Write($"- Sure to Load Ordered Text File into AVL Tree? {stopInfo}...");
                        if ((confirm = ReadLine()) != stopInput)
                        {
                            ordered = true;
                            balanced = true;
                        }
                        break;
                    case 4:
                        Write($"- Sure to Load Random Text File into AVL Tree? {stopInfo}...");
                        if ((confirm = ReadLine()) != stopInput)
                        {
                            ordered = false;
                            balanced = true;
                        }
                        break;
                    default: break;
                }
                if (confirm == stopInput)
                {
                    Clear();
                    ShowMenu_selectType();
                    // loop to get selected in menu
                }
                else
                {
                    Clear();
                    selected = true;// break menu loop
                    ShowMenu_loading();
                    SelectMenu_loading();
                }
            }
        }
    }
    /// <summary>
    /// show list of files to get loaded
    /// </summary>
    static void ShowMenu_loading()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"***** MENU *****\nSelect a {orderedType} File to Load on {treeType}");
        sb.AppendLine("Enter 0  - Turn Back to previous MENU");
        for (int i = 0; i < 11; i++)
        {
            sb.AppendLine($"Enter {i + 1}  - {names[i]}");
        }
        Write(sb.ToString());
    }
    /// <summary>
    /// get corresponding file to get loaded
    /// </summary>
    static void SelectMenu_loading()
    {
        int maxNum = 11;
        bool selected = false;
        while (!selected)
        {
            Write("Enter number: ");
            string? input = ReadLine();
            int value = Util.GetInt(input, maxNum);
            if (value == Util.bad_int)
            {
                WriteLine("Invalid Input, Enter number 0 ~ {0}", maxNum);
            }
            else
            {// loading file is chosen
                string? confirm = "";
                // get user input to confirm commitment
                switch (value)
                {
                    case 0:
                        Write($"- Sure to Go back? {stopInfo}...");
                        if ((confirm = ReadLine()) != stopInput)
                        {
                            Clear();
                            ShowMenu_selectType();
                            SelectMenu_selectType();
                            return;
                        }
                        break;
                    default:
                        Write($"Sure to Load {orderedType} {names[value - 1]} into {treeType}? {stopInfo}...");
                        if ((confirm = ReadLine()) != stopInput)
                        {

                            if (ordered && balanced)
                            {
                                // ordered file load on avl
                            }
                            else if (ordered)
                            {
                                bst_DS = Load_BST(orderedPath, value - 1);
                            }
                            else if (balanced)
                            {
                                // random file load on avl
                            }
                            else
                            {
                                bst_DS = Load_BST(randomPath, value - 1);
                            }
                        }
                        break;
                }
                if (confirm == stopInput)
                {
                    Clear();
                    ShowMenu_selectType();
                    // loop to get selected in menu
                }
                else
                {// did not stop and committed loading (case 0 get returned earlier)
                    WriteLine("Press any key to Continue committing functions...");
                    ReadKey();
                    Clear();
                    selected = true;// break menu loop
                    ShowMenu_function();
                    SelectMenu_function();
                }
            }
        }
    }
    static void ShowMenu_function()
    {

    }
    static void SelectMenu_function()
    {

    }
    static BST_DS Load_BST(string path, int index)
    {
        BST_DS ds = new BST_DS();
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (directoryInfo.Exists)
        {
            FileInfo[] fileInfos = directoryInfo.GetFiles().OrderBy(f => f.Length).ToArray();
            FileInfo fileInfo = fileInfos[index];
            string seq = path == orderedPath ? "Ordered_" : "Random_";
            ds.Name = seq + fileInfo.Name;
            StreamReader sr = fileInfo.OpenText();
            string? line;
            Write($"{fileInfo.Name} is Loading ... ");
            while ((line = sr.ReadLine()) != null)
            {
                ds.Add(line);
            }
            WriteLine($"Complete! Binary_Search_Tree Contain {ds.Count} words");
        }
        else
        {
            WriteLine($"{path} Not Existed");
        }
        return ds;
    }
}