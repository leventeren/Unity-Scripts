// Benchmark:
// int.Parse("400")     123.07 ns
// IntParseFast("400")    2.87 ns
public static int IntParseFast(string value)
{
    int result = 0;
    for (int i = 0; i < value.Length; i++)
    {
        char letter = value[i];
        result = 10 * result + (letter - 48);
    }
    return result;
}

public static int IntParseFast(char value)
{
    int result = 0;
    result = 10 * result + (value - 48);
    return result;
}
