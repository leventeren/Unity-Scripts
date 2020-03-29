StartCoundownTimer();

public Text gameTime;
private float time;
private bool isGameOver = false;

//Start Count down of the timer
void StartCoundownTimer()
{
    if (gameTime != null)
    {
        time = 120;//2 minute
        gameTime.text = "Time Left: 2:00:000"; //set initial string to two minutes
        InvokeRepeating("UpdateTimer", 0.0f, 0.01f); //Invokes the method UpdateTimer in time seconds, then repeatedly every repeatRate seconds.
    }
}


void UpdateTimer()
{
    //when gameTime is exists and Game is not over 
    //update time text on the screen 
    if (gameTime != null && !isGameOver)
    {
        time -= Time.deltaTime;
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        string fraction = ((time * 100) % 100).ToString("000");
        gameTime.text = "Time Left: " + minutes + ":" + seconds + ":" + fraction;
    }
}
