/* 1 */

function PauseWaitResume (pauseDelay : float) {
  Time.timeScale = .0000001;
  yield WaitForSeconds(pauseDelay * Time.timeScale);
  Time.timeScale = 1.0;
}

/* 2 */

void PauseAndResume()
 {
          Time.timeScale = 0;
          //Display Image here
          StartCoroutine(ResumeAfterNSeconds(3.0f));
 }
 
 float timer = 0;
 IEnumerator ResumeAfterNSeconds(float timePeriod)
 {
          yield return new WaitForEndOfFrame();
          timer += Time.unscaledDeltaTime;
          if(timer < timePeriod)
                     StartCoroutine(ResumeAfterNSeconds(3.0f));
          else
          {
                     Time.timeScale = 1; //Resume
                     timer = 0;
          }
 }
