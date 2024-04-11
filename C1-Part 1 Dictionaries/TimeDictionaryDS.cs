using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace C1_Part_1_Dictionaries;

internal class TimeDictionaryDS
{
    TimeNode node;
    Dictionary<string, TimeNode> dictionary = new();
    public TimeDictionaryDS()
    {

    }
    private int InsertKey(string key, TimeNode node)
    {
        int count = dictionary.Count;
        if (dictionary.ContainsKey(key))
            return 0;
        else
        {
            dictionary.Add(key, node);
            return 1;
        }
    }
    public string Insert(string type, int number, TimeSpan timespan)
    {
        TimeNode node = new(type, number, timespan);
        string key = type + "_" + number;
        int feedback = InsertKey(key, node);
        switch (feedback)
        {
            case 0: return $"Failed to insert {key}, for {key} exists in Dictionary";
            case 1: return $"Sucessfully inserted {key} in Dictionary, which has {dictionary.Count} entries";
            default: return "InsetKey error";
        }
    }
    public string Print()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("*** Print for Time Consumption ***");
        sb.AppendLine($"{$"Type",-10} | {$"Number",-10} | {$"Time(ms)",-10}");// Title
        sb.AppendLine($"-----------+------------+-----------");
        foreach (KeyValuePair<string, TimeNode> entry in dictionary)
            sb.AppendLine($" {entry.Value.Type,-10} | {entry.Value.Number,-10} | {entry.Value.Time}");
        sb.AppendLine($"-----------+------------+-----------");
        sb.AppendLine($"{$"Type",-10} | {$"Number",-10} | {$"Time(ms)",-10}");// Title
        sb.AppendLine($"\nDictionary has totally {dictionary.Count} entries");
        sb.AppendLine("------------------------------------------------\n");
        return sb.ToString();
    }
}
