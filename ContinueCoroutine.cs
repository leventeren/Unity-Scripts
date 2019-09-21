// example script for restarting Coroutine after gameobject was disabled (and continue from previous timer value)


using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContinueCoroutine : MonoBehaviour
{
    public float duration = 5f;

    Image image;

    bool isRunning = true;

    float timer = 0;
    float oldtimer = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // keep timer values on disable
    private void OnDisable()
    {
        oldtimer = timer;
        isRunning = false;
    }

    // start coroutine on enable
    private void OnEnable()
    {
        timer = oldtimer;
        isRunning = true;
        StartCoroutine(CustomUpdater());
    }

    // coroutine loop
    IEnumerator CustomUpdater()
    {
        while (true)
        {
            for (timer = oldtimer; timer < duration; timer += Time.deltaTime) // this works, since we keep reference to old timer
            // for (float timer = 0; timer < duration; timer += Time.deltaTime) // this doesnt work, because restarting coroutine would reset timer value
            {
                image.color = Color.Lerp(Color.red, Color.green, timer / duration);
                yield return 0;
            }
            oldtimer = 0;
        }
    }

}
