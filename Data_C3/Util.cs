namespace Data_C3;

public class Util
{
    public static readonly int bad_int = int.MaxValue;
    /// <summary>
    /// check if typed number is an valid option number
    /// </summary>
    /// <param name="data">typed number</param>
    /// <param name="max">valid number range from 0</param>
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
    /// get number indicate whether operation is confirmed
    /// </summary>
    /// <param name="input"></param>
    /// <returns>1 confirmed, 0 confirmed Not, -1 invalid</returns>
    public static int IsSure(string? input)
    {
        input = input.ToLower().Trim();
        if (string.IsNullOrEmpty(input) || input == "y" || input == "yes")
        {
            return 1;
        }
        else if (input == "no" || input == "n")
        {
            return 0;
        }
        return -1;
    }
}
