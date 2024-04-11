using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace C1_Part_1_Dictionaries;

internal class DictionaryDS
{
    public string Path { get; set; }
    public string Name { get; set; }
    public string Type {  get; set; }
    public int Number {  get; set; }
    public int Count
    {
        get { return dictionary.Count; }
    }
    Node node;
    Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
    public DictionaryDS(string pathName)
    {
        Path = pathName;
        Type = Util.FileName(pathName).type;
        Number = Util.FileName(pathName).number;
        Name = Type + "_" + Number;
    }
    private int InsertKey(string key, Node node)
    {
        int count = dictionary.Count;
        if (dictionary.ContainsKey(key))
            return 0;
        else if (key.StartsWith("#"))
            return -1;
        else
        {
            dictionary.Add(key, node);
            return 1;
        }
    }
    private int FindKey(string key)
    {
        if (dictionary.ContainsKey(key))
            return 1;
        else
            return 0;
    }
    private int DeleteKey(string key)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
            return 1;
        }
        else return 0;

    }
    public string Insert(string word)
    {
        Node node = new Node(word, word.Length);
        int feedback = InsertKey(word, node);
        switch (feedback)
        {
            case 0: return $"Failed to insert {word}, for {word} exists in Dictionary";
            case -1: return $"Failed to insert {word}, for {word} starts with char \"#\"";
            case 1: return $"Sucessfully inserted {word} in Dictionary, which has {this.Count} entries";
            default: return "InsetKey error";
        }
    }
    public string Find(string word)
    {
        switch (FindKey(word))
        {
            case 0: return $"Failed to find {word}\tfor {word} not existing in Dictionary";
            case 1: return $"Succeed to find {word}\t{word} was found in Dictionary";
            default: return "FindKey error";
        }
    }
    public string Delete(string word)
    {
        switch (DeleteKey(word))
        {
            case 0: return $"Failed to delete {word}, for {word} not existing in Dictionary";
            case 1: return $"Seccessfully deleted {word} in Dictionary, which has {this.Count} entries";
            default: return "DeleteKey error";
        }
    }
    public string Print()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("***** Print for Dictionary *****");
        sb.AppendLine($"{$"Word",-15} | {$"Length",-7}");
        sb.AppendLine($"-----------+--------");
        foreach (KeyValuePair<string, Node> pair in dictionary)
            sb.AppendLine($"{pair.Key,-15} | {pair.Value.ToString(),-7}");
        sb.AppendLine($"-----------+--------");
        sb.AppendLine($"{$"Word",-15} | {$"Length",-7}");
        sb.AppendLine($"\nDictionary has totally {this.Count} entries");
        sb.AppendLine("-------------------------------------------------");
        return sb.ToString();
    }
}
