using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace C1_Part_2_Sorting_Algorithms;

internal class TimeDictionsryDS
{
    Dictionary<int, TimeSpan> dictionary = new();
    public string SortType { get; set; }
    public TimeDictionsryDS(string sortType)
    {
        SortType = sortType;
    }
    public int Insert(int number, TimeSpan time)
    {
        int count = dictionary.Count;
        if (dictionary.ContainsKey(number))
            return 0;
        else
        {
            dictionary.Add(number, time);
            return 1;
        }
    }
    public string Print()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"*** Print for Time Consumption [{this.SortType}] ***");
        sb.AppendLine($"{$"Sort Type",-10} | {$"Word Number",-10} | {$"Time (ms)",-10}");// Title
        sb.AppendLine($"-----------+------------+-----------");
        foreach (KeyValuePair<int, TimeSpan> entry in dictionary)
        {
            string time = string.Format("{0:N2}", entry.Value.TotalMilliseconds);
            sb.AppendLine($" {this.SortType,-10} | {entry.Key,-10} | {time}");
        }
        sb.AppendLine("--------------------------------------");
        return sb.ToString();
    }
}
