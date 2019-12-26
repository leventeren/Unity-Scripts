/// <summary>
/// Şans hesaplaması
/// 1-9 Ai için
/// 10-100 Ai şans faktörü hesaplaması
/// </summary>
/// <param name="value">Oyun zorluk derecesi</param>
public bool luckCalculator(int value)
{
    if (value==1)
    {
        bool[] uSoLucky = { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 2)
    {
        bool[] uSoLucky = { true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 3)
    {
        bool[] uSoLucky = { true, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 4)
    {
        bool[] uSoLucky = { true, false, true, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 5)
    {
        bool[] uSoLucky = { true, false, true, false, true, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 6)
    {
        bool[] uSoLucky = { true, false, true, false, true, false, true, false, true, false, true, false, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 7)
    {
        bool[] uSoLucky = { true, false, true, false, true, false, true, false, true, false, true, false, true, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 8)
    {
        bool[] uSoLucky = { true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value == 9)
    {
        bool[] uSoLucky = { true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value==10)
    {
        bool[] uSoLucky = { true, false, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value>10 && value<=20)
    {
        bool[] uSoLucky = { true, true, false, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 20 && value <= 30)
    {
        bool[] uSoLucky = { true, true, true, false, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 30 && value <= 40)
    {
        bool[] uSoLucky = { true, true, true, true, false, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 40 && value <= 50)
    {
        bool[] uSoLucky = { true, true, true, true, true, false, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 50 && value <= 60)
    {
        bool[] uSoLucky = { true, true, true, true, true, true, false, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 60 && value <= 70)
    {
        bool[] uSoLucky = { true, true, true, true, true, true, true, false, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 70 && value <= 80)
    {
        bool[] uSoLucky = { true, true, true, true, true, true, true, true, false, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 80 && value <= 90)
    {
        bool[] uSoLucky = { true, true, true, true, true, true, true, true, true, false };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    }
    else if (value > 90 && value <= 100)
    {
        bool[] uSoLucky = { true, true, true, true, true, true, true, true, true, true };
        return uSoLucky[UnityEngine.Random.Range(0, uSoLucky.Length)];
    } else
    {
        return false;
    }
}
