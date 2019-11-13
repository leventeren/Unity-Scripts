// Image Component have Unlit/Transparent material

public float speedModDiv;
public float dTime = 0;

void Update()
{
    dTime += Time.deltaTime / speedModDiv;
    GetComponent<Image>().materialForRendering.SetTextureOffset("_MainTex", new Vector2(dTime, 0));
}
