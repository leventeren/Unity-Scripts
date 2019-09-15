using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircularLoadingDash : MonoBehaviour {

    public Image circularSilder;            //Drag the circular image i.e Slider in our case
    public float time;                      //In how much time the progress bar will fill/empty

    public static bool resetTimer = false;
    public static bool dashAvailable = false;

    void Start()
    {
        circularSilder.fillAmount = 0f;      // Initally progress bar is empty
    }
    void Update()
    {
        circularSilder.fillAmount += Time.deltaTime / time;

        if (resetTimer)
        {
            dashAvailable = false;
            circularSilder.fillAmount = 0f;
            resetTimer = false;
        }

        if(circularSilder.fillAmount.Equals(1f))
        {
            dashAvailable = true;
        }
    }
}
