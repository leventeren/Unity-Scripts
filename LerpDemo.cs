using UnityEngine;

[ExecuteInEditMode]
public class LerpDemo: MonoBehaviour{

  [SerializeField]
  private float start = 0;
  [SerializeField]
  private float end = 100;
  [SerializeField]
  [Range(0f,1f)]
  private float lerpPct = 0.5f;
  public float finalValue;
  
  private void Update(){
    finalValue = Mathf.Lerp(start, end, lerpPct);
  }
}
