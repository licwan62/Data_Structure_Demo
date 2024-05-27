namespace Data_C3;

public class Util
{
    public static readonly int bad_int = int.MaxValue;
    /// <summary>
    /// check validation for user typed option
    /// </summary>
    /// <param name="data">option chosen for commitment</param>
    /// <param name="max">range for valid option number</param>
    /// <returns>bad_int: Empty input or value out of range</returns>
    public static int GetInt(string? data, int max)
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
    /// <summary>
    /// Get unique ASCII number converted from string
    /// </summary>
    /// <param name="data">string words</param>
    /// <returns>ASCII number of words</returns>
    public static int GetASCII(string data)
    {
        int result = 0;
        foreach (char c in data)
        {
            result = result * 256 + (int)c;
        }
        return result;
    }
}
