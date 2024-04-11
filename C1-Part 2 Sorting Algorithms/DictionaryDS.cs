using System.Text;
namespace C1_Part_2_Sorting_Algorithms;

internal class DictionaryDS
{
    Node node;
    Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
    public string Path { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public int Count
    {
        get { return dictionary.Count; }
    }
    public DictionaryDS(string pathName)
    {
        Path = pathName;
        Number = Util.FileName(pathName).number;
        Name = Number.ToString() + "_Words Dictionary";
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
            case 0: return $"{word} not existing in Dictionary";
            case 1: return $"{word} was found in Dictionary";
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
        sb.AppendLine($"*** Print for Dictionary of {this.Name} ***");
        foreach (KeyValuePair<string, Node> pair in dictionary)
            sb.AppendLine($" {pair.Key,-20} | {pair.Value.ToString(),-7}");
        sb.AppendLine($"--------------------------------");
        sb.AppendLine($" {$"Word",-20} | {$"Length",-7}");
        sb.AppendLine($"\nDictionary has totally {dictionary.Count} entries");
        sb.AppendLine("-------------------------------------------------");
        return sb.ToString();
    }
    public ArrayDS ToArray()
    {
        ArrayDS arrayDS = new(this.Number, dictionary.Count);
        foreach (KeyValuePair<string, Node> entry in dictionary)
        {
            arrayDS.Insert(entry.Key);
        }
        return arrayDS;
    }
}
