Target curTarget; //gameobject

[Header("Rotate Settings")]
private Vector3 targetPos;
public Transform partToRotate;
[Range(0, 1)]
[SerializeField] private float _turnSpeed;
public float turnSpeed
{
    get
    {
        return Mathf.Lerp(.1f, 10f, _turnSpeed);
    }
    set
    {
        _turnSpeed = value;
    }
}

void Update(){
  targets = GameObject.FindGameObjectsWithTag("Target");
  foreach (var target in targets) {
      var t = target.GetComponent<Target>();
      if (t) {
          curTarget = t;
          break;
      }
  }
}
    
public void LockOnTarget()
{
    targetPos = curTarget.transform.position;
    Vector3 dir = targetPos - transform.position;
    Quaternion lookRotation = Quaternion.LookRotation(dir);
    Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
    partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
}
