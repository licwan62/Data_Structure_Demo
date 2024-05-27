namespace Data_C3;

internal class Node
{
    public int Key { get; set; }
    public string Data {  get; set; }
    public int Length { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node()
    {
        Key = 0;
        Length = 0;
        Data = "";
        Left = null;
        Right = null;
    }
    public Node(string data, int length)
    {
        Key = Util.GetASCII(data);
        Length = length;
        Data = data;
        Left = null;
        Right = null;
    }
    public override string ToString()
    {
        return Data;
    }
    public string ToPrint()
    {
        return string.Format("{0,-20}{1,-10}{2,-10}", Data, Length, Key);
    }
}
