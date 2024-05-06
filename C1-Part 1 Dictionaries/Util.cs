namespace C1_Part_1_Dictionaries;

public class Util
{
    /// <summary>
    /// example
    /// C:\Users\hzwlc\source\repos\Data Structures\C1-Part 1 Dictionaries\random\10000-words.txt
    /// return ("random", 10000)
    /// </summary>
    /// <param name="path">full path of file name</param>
    /// <returns></returns>
    public static (string type, int number) FileName(string path)
    {
        int start = 0, end;
        end = path.LastIndexOf("\\");
        string type = path.Substring(0, end);
        type = type.Substring(type.LastIndexOf("\\") + 1);

        start = path.LastIndexOf("\\") + 1;
        end = path.LastIndexOf("-") - 1;
        int number = int.Parse(path.Substring(start, end - start + 1));
        return (type, number);
    }
}
