public int playerLives
{
    get
    {
        return _playerLives;
    }
    set
    {
        //updateUI();
        if (value == 0)
        {
            EndGame();
        }
        else
        {
            _playerLives = value;
        }
    }
}
private int _playerLives = 3;



//GameManager.instance.playerLives--;
