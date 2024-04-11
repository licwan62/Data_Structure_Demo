using System;

namespace C1_Part_1_Dictionaries;

internal class TimeNode
{
    public string Type {  get; set; }
    public int Number {  get; set; }
    public string Time {  get; set; }
    public TimeNode(string type, int number, TimeSpan timespan)
    {
        Type = type;
        Number = number;
        Time = string.Format("{0:N3}", timespan.TotalMilliseconds);
    }
}
