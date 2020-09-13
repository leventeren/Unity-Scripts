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



############################################################

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Countdown : MonoBehaviour
{
    [Header("Settings")]                                
    [Tooltip("Set true to start counting (On Start event will be triggered the first time it is set to true)")]
    [SerializeField] private bool   _isCounting = true;                 // Is currently counting or not
    [Tooltip("Cooldown duration")]
    [SerializeField] private float  _duration   = 10;                   // Duration of the cooldown
    [Header("GUI")]
    [Tooltip("Text that can show the countdown")]
    [SerializeField] private TextMeshProUGUI _countdownText = null;     // Display text
    [Header("Countdown Events")]
    [Tooltip("Event that is triggered when countdown is started")]
    [SerializeField] private UnityEvent _onStart    = null;             // Event that is fired when countdown starts
    [Tooltip("Event that is triggered when countdown is finished")]
    [SerializeField] private UnityEvent _onFinish   = null;             // Event that is fired when countdown is over
    
    private float   _counter    = 0;        // Timer which counts for countdown         
    private bool    _isFinised  = false;    // Finish state
    private bool    _isStart    = false;    // Start state

     // =============================== GETTER/SETTERS ===================================
    public bool IsCounting
    {
        get{ return  _isCounting; }
        set{ _isCounting = value; }
    }

    // ================================== MONOBEHAVIOUR ====================================
    private void Start() 
    {
        _counter = _duration;       // Set timer
        _isStart = false;           // Countdown is not started
    }

    private void Update() 
    {
        // Check if timer can count
        if(_isCounting == true && _isFinised == false)
        {
            if(_isStart == false)
            {
                _isStart = true;
                _onStart.Invoke();
            }
            _counter -= Time.deltaTime;
            if(_countdownText) _countdownText.text = ((int)Mathf.Ceil(_counter)).ToString();
            if(_counter <= 0)
            {
                _isFinised = true;
                _onFinish.Invoke();
            }
        }    
    }

}
