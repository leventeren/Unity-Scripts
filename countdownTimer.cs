IEnumerator countdownTimer()
{
  yield return new WaitForSecondsRealtime(1);
  countdown.text = "2";
  yield return new WaitForSecondsRealtime(1);
  countdown.text = "1";
  yield return new WaitForSecondsRealtime(1);
  Time.timeScale = 1;
  countdown.gameObject.SetActive(false);
  countdown.text = "3";
}
