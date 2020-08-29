/* USAGE: 

GameObject countDown;
CountDown scriptCountDown;
void Start(){
  scriptCountDown = countDown.GetComponent<CountDown>();
}
void Update(){
  if (countDown.activeSelf)
    {
        scriptCountDown.startCountDown();
    }
}
*/


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    private enum countDowndStates {WAITING, FINISHED, INPROGRESS};
    private countDowndStates state;
    private Text textUI;
    private int actualNumber = 3;
    private int sizeFont = 300;
    private int actualSizeFont = 300;
    private string txtAdelante;

    public string TxtAdelante {
        get { return txtAdelante; }
        set { txtAdelante = value; }
    }

    void Start()
    {
        textUI = GetComponent<Text>();
        state = countDowndStates.WAITING;
    }

    IEnumerator showLastMessage()
    {

        while (actualNumber > 0)
        {
            textUI.fontSize = actualSizeFont;
            textUI.text = actualNumber.ToString();

            if (actualSizeFont <= 0)
            {
                actualNumber--;
                actualSizeFont = sizeFont;
            }
            else
            {
                actualSizeFont -= 5;
            }

            yield return new WaitForSeconds(3.0f);
        }

        textUI.fontSize = 100;
        textUI.text = txtAdelante;
        yield return new WaitForSeconds(3.0f);
        finish();
    }

    private void finish()
    {
        state = countDowndStates.FINISHED;
        gameObject.SetActive(false);
    }

    public void startCountDown()
    {
        if (state == countDowndStates.WAITING)
        {
            state = countDowndStates.INPROGRESS;
        }

        StartCoroutine(showLastMessage());
    }

    public bool isFinished()
    {
        return state == countDowndStates.FINISHED;
    }
}
