namespace Data_C2;

internal class Node
{
    public Node Next { get; set; }
    public Node Prev { get; set; }
    public string Data {  get; set; }
    public int Length
    {
        get
        {
            return Data.Length;
        }
    }
    public Node()
    {
        Next = null;
        Prev = null;
        Data = "";
    }
    public Node(string data)
    {
        Next = null;
        Prev = null;
        Data = data;
    }
    public string ToPrint()
    {
        return $"[Data] {Data,-20}[Length] {Length}";
    }
    public override string ToString()
    {
        return Data.ToString();
    }
}
