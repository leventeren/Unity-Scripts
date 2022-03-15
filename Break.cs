public class Break : MonoBehaviour {
  public GameObject fractured;
  public float breakForce;
  
  void Update(){
    if (Input.GetKeyDown("f"))
      BreakTheThing();
  }
  
  public void BreakTheThing(){
    GameObject frac = Instantiate(fractured, transform.position, transform.rotation);

    foreach(Rigidbody rb in frac.GetComponentsInChildren<RigidBody>()){
      Vector3 force = (rb.transform.position - transform.position).normalized * breakForce;
      rb.AddForce(force);
    }
    Destroy(gameObject);
  }
}
