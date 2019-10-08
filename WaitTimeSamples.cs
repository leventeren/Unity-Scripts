1.With a coroutine and WaitForSeconds.

void Start()
{
    StartCoroutine(waiter());
}

IEnumerator waiter()
{
    //Rotate 90 deg
    transform.Rotate(new Vector3(90, 0, 0), Space.World);

    //Wait for 4 seconds
    yield return new WaitForSeconds(4);

    //Rotate 40 deg
    transform.Rotate(new Vector3(40, 0, 0), Space.World);

    //Wait for 2 seconds
    yield return new WaitForSeconds(2);

    //Rotate 20 deg
    transform.Rotate(new Vector3(20, 0, 0), Space.World);
}

############################################################

2.With a coroutine and WaitForSecondsRealtime

void Start()
{
    StartCoroutine(waiter());
}

IEnumerator waiter()
{
    //Rotate 90 deg
    transform.Rotate(new Vector3(90, 0, 0), Space.World);

    //Wait for 4 seconds
    yield return new WaitForSecondsRealtime(4);

    //Rotate 40 deg
    transform.Rotate(new Vector3(40, 0, 0), Space.World);

    //Wait for 2 seconds
    yield return new WaitForSecondsRealtime(2);

    //Rotate 20 deg
    transform.Rotate(new Vector3(20, 0, 0), Space.World);
}

#############################################################

3.With a coroutine and incrementing a variable every frame with Time.deltaTime.

bool quit = false;

void Start()
{
    StartCoroutine(waiter());
}

IEnumerator waiter()
{
    float counter = 0;
    //Rotate 90 deg
    transform.Rotate(new Vector3(90, 0, 0), Space.World);

    //Wait for 4 seconds
    float waitTime = 4;
    while (counter < waitTime)
    {
        //Increment Timer until counter >= waitTime
        counter += Time.deltaTime;
        Debug.Log("We have waited for: " + counter + " seconds");
        //Wait for a frame so that Unity doesn't freeze
        //Check if we want to quit this function
        if (quit)
        {
            //Quit function
            yield break;
        }
        yield return null;
    }

    //Rotate 40 deg
    transform.Rotate(new Vector3(40, 0, 0), Space.World);

    //Wait for 2 seconds
    waitTime = 2;
    //Reset counter
    counter = 0;
    while (counter < waitTime)
    {
        //Increment Timer until counter >= waitTime
        counter += Time.deltaTime;
        Debug.Log("We have waited for: " + counter + " seconds");
        //Check if we want to quit this function
        if (quit)
        {
            //Quit function
            yield break;
        }
        //Wait for a frame so that Unity doesn't freeze
        yield return null;
    }

    //Rotate 20 deg
    transform.Rotate(new Vector3(20, 0, 0), Space.World);
}

You can still simplify this by moving the while loop into another coroutine function and yielding it and also still be able to see it counting and even interrupt the counter.

bool quit = false;

void Start()
{
    StartCoroutine(waiter());
}

IEnumerator waiter()
{
    //Rotate 90 deg
    transform.Rotate(new Vector3(90, 0, 0), Space.World);

    //Wait for 4 seconds
    float waitTime = 4;
    yield return wait(waitTime);

    //Rotate 40 deg
    transform.Rotate(new Vector3(40, 0, 0), Space.World);

    //Wait for 2 seconds
    waitTime = 2;
    yield return wait(waitTime);

    //Rotate 20 deg
    transform.Rotate(new Vector3(20, 0, 0), Space.World);
}

IEnumerator wait(float waitTime)
{
    float counter = 0;

    while (counter < waitTime)
    {
        //Increment Timer until counter >= waitTime
        counter += Time.deltaTime;
        Debug.Log("We have waited for: " + counter + " seconds");
        if (quit)
        {
            //Quit function
            yield break;
        }
        //Wait for a frame so that Unity doesn't freeze
        yield return null;
    }
}

#############################################################################

4.With a coroutine and the WaitUntil function:

float playerScore = 0;
int nextScene = 0;

void Start()
{
    StartCoroutine(sceneLoader());
}

IEnumerator sceneLoader()
{
    Debug.Log("Waiting for Player score to be >=100 ");
    yield return new WaitUntil(() => playerScore >= 10);
    Debug.Log("Player score is >=100. Loading next Leve");

    //Increment and Load next scene
    nextScene++;
    SceneManager.LoadScene(nextScene);
}

#############################################################################

5.With a coroutine and the WaitWhile function.

void Start()
{
    StartCoroutine(inputWaiter());
}

IEnumerator inputWaiter()
{
    Debug.Log("Waiting for the Exit button to be pressed");
    yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Escape));
    Debug.Log("Exit button has been pressed. Leaving Application");

    //Exit program
    Quit();
}

void Quit()
{
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif
}

#############################################################################

6.With the Invoke function:

void Start()
{
    Invoke("feedDog", 5);
    Debug.Log("Will feed dog after 5 seconds");
}

void feedDog()
{
    Debug.Log("Now feeding Dog");
}

########################################

7.With the Update() function and Time.deltaTime

float timer = 0;
bool timerReached = false;

void Update()
{
    if (!timerReached)
        timer += Time.deltaTime;

    if (!timerReached && timer > 5)
    {
        Debug.Log("Done waiting");
        feedDog();

        //Set to false so that We don't run this again
        timerReached = true;
    }
}

void feedDog()
{
    Debug.Log("Now feeding Dog");
}

There are still other ways to wait in Unity but you should definitely know the ones mentioned above as that makes it easier to make games in Unity. When to use each one depends on the circumstances.

For your particular issue, this is the solution:

IEnumerator showTextFuntion()
{
    TextUI.text = "Welcome to Number Wizard!";
    yield return new WaitForSeconds(3f);
    TextUI.text = ("The highest number you can pick is " + max);
    yield return new WaitForSeconds(3f);
    TextUI.text = ("The lowest number you can pick is " + min);
}

And to call/start the coroutine function from your start or Update function, you call it with

StartCoroutine (showTextFuntion());

