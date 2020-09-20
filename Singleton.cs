#region static data
public static GameManager instance { get; private set; }
public void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }
    else
    {
        Destroy(gameObject);
    }
}
#endregion
