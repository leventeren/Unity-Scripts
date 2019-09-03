using UnityEngine;

[ExecuteInEditMode]
public class LerpMoveDemo: MonoBehaviour
{
  [SerializeField]
  private Transform startCube;
  [SerializeField]
  private Transform endCube;
  [SerializeField]
  [Range(0f,1f)]
  private float lerpPct = 0.5f;
  
  private void Update(){
    transform.position = Vector3.Lerp(startCube.position, endCube.position, lerpPct);
    transform.rotation = Vector3.Lerp(startCube.rotation, endCube.rotation, lerpPct);
  }
}
