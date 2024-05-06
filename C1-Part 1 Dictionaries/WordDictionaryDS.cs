using System.Text;

namespace C1_Part_1_Dictionaries;

internal class WordDictionaryDS
{
    public string Path { get; set; }// path of text file
    public string Name { get; set; }// ex. ordered_1000
    public string Type { get; set; }// ex. ordered or random
    public int Number { get; set; }// world count that the text file include
    public int Count { get { return dictionary.Count; } }
    Node node;
    Dictionary<string, Node> dictionary;
    public WordDictionaryDS(string pathName)
    {
        dictionary = new Dictionary<string, Node>();
        Path = pathName;
        Type = Util.FileName(pathName).type;
        Number = Util.FileName(pathName).number;
        Name = Type + "_" + Number;
    }
    private int InsertKey(string key, Node node)
    {
        if (dictionary.ContainsKey(key))
            return 0;// key already exist
        else if (key.StartsWith("#"))
            return -1;// key starts with #
        else
        {
            dictionary.Add(key, node);
            return 1;
        }
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
    private int DeleteKey(string key)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary.Remove(key);
            return 1;
        }
        else return 0;
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
    private int FindKey(string key)
    {
        if (dictionary.ContainsKey(key))
            return 1;
        else
            return 0;
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
