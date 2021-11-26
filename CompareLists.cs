public static bool CompareLists<T>(List<T> aListA, List<T> aListB)
{
    if (aListA == null || aListB == null || aListA.Count != aListB.Count)
        return false;
    if (aListA.Count == 0)
        return true;
    Dictionary<T, int> lookUp = new Dictionary<T, int>();
    for (int i = 0; i < aListA.Count; i++)
    {
        int count = 0;
        if (!lookUp.TryGetValue(aListA[i], out count))
        {
            lookUp.Add(aListA[i], 1);
            continue;
        }
        lookUp[aListA[i]] = count + 1;
    }
    for (int i = 0; i < aListB.Count; i++)
    {
        int count = 0;
        if (!lookUp.TryGetValue(aListB[i], out count))
        {
            return false;
        }
        count--;
        if (count <= 0)
            lookUp.Remove(aListB[i]);
        else
            lookUp[aListB[i]] = count;
    }
    return lookUp.Count == 0;
}
