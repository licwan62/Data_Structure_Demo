namespace Data_C3;

internal class Node
{
    public string Data {  get; set; }
    public int Length { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node()
    {
        Data = "";
        Length = 0;
        Left = null;
        Right = null;
    }
    public Node(string data, int length)
    {
        Data = data;
        Length = length;
        Left = null;
        Right = null;
    }
    public override string ToString()
    {
        return Data;
    }
    public string ToPrint()
    {
        return string.Format("{0,-20}{1,-10}", Data, Length);
    }
}
