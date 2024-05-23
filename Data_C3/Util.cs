namespace Data_C3;

public class Util
{
    public static readonly int bad_int = int.MaxValue;
    public static int GetInt(string data, int max)
    {
        if (string.IsNullOrEmpty(data))
        {
            return bad_int;
        }
        else if (int.TryParse(data, out int value))
        {
            if (value <= max && value > -1)
            {
                return value;
            }
        }
        return bad_int;
    }
}
