namespace Data_C3;

internal class Node
{
    public int Key { get; set; }
    public string Data {  get; set; }
    public int Length { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public Node(string data)
    {
        Key = 0;
        Length = 0;
        Data = data;
        Left = null;
        Right = null;
    }
    public Node(int key, string data, int length)
    {
        Key = key;
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
        return string.Format("{0,-20}{1,-10}", Data, Length);
    }
}
