private float maxOffset = 3f;

void Update(){
  float noise = Mathf.PerlinNoise(transform.position.x / 10f + Time.time, transform.position.z /10f + Time.time);
  transform.Localscale = new Vector3(1f, noise * maxOffset, 1f);
}
