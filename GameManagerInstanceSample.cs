public class GameManager : MonoBehaviour {

  #region Instance
  private static GameManager _instance;
  public static GameManager Instance => _instance;
  #endregion
  
  private void Awake() {
      if(_instance != null && _instance != this) Destroy(gameObject);
      _instance = this;
  }    
}

/*
#region Instance
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioManager>();
                if (_instance == null) {
                
                    _instance = new GameObject().AddComponent<AudioManager>();
                    _instance.name = "AudioManager";
                }
            }
            return _instance;
        }
    }
    #endregion
*/
