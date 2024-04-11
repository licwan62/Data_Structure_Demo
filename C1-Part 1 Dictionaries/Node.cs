namespace C1_Part_1_Dictionaries;

internal class Node
{
    public string Word { get; set; }
    public int Length { get; set; }
    public Node(string word, int length)
    {
        Word = word;
        Length = length;
    }
    public override string ToString()
    {
        return this.Length.ToString();
    }
}