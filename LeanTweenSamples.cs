using UnityEngine;
using System.Collections;
using DentedPixel;

public class GeneralBasics2d : MonoBehaviour {

	public Texture2D dudeTexture;
	public GameObject prefabParticles;

	#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)

	void Start () {
		// Setup
		GameObject avatarRotate = createSpriteDude( "avatarRotate", new Vector3(-2.51208f,10.7119f,-14.37754f));
		GameObject avatarScale = createSpriteDude( "avatarScale", new Vector3(2.51208f,10.2119f,-14.37754f));
		GameObject avatarMove = createSpriteDude( "avatarMove", new Vector3(-3.1208f,7.100643f,-14.37754f));
	
		// Rotate Example
		LeanTween.rotateAround( avatarRotate, Vector3.forward, -360f, 5f);

		// Scale Example
		LeanTween.scale( avatarScale, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
		LeanTween.moveX( avatarScale, avatarScale.transform.position.x + 1f, 5f).setEase(LeanTweenType.easeOutBounce); // Simultaneously target many different tweens on the same object 

		// Move Example
		LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(1.7f, 0f, 0f), 2f).setEase(LeanTweenType.easeInQuad);

		// Delay
		LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(2f, -1f, 0f), 2f).setDelay(3f);

		// Chain properties (delay, easing with a set repeating of type ping pong)
		LeanTween.scale( avatarScale, new Vector3(0.2f, 0.2f, 0.2f), 1f).setDelay(7f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong(3);

		// Call methods after a certain time period
		LeanTween.delayedCall(gameObject, 0.2f, advancedExamples);
	}
	
	GameObject createSpriteDude( string name, Vector3 pos, bool hasParticles = true ){
		GameObject go = new GameObject(name);
		SpriteRenderer ren = go.AddComponent<SpriteRenderer>();
		go.GetComponent<SpriteRenderer>().color = new Color(0f,181f/255f,1f);
		ren.sprite = Sprite.Create( dudeTexture, new Rect(0.0f,0.0f,256.0f,256.0f), new Vector2(0.5f,0f), 256f);
		go.transform.position = pos;

		if(hasParticles){
			GameObject particles = (GameObject)GameObject.Instantiate(prefabParticles, Vector3.zero, prefabParticles.transform.rotation );
			particles.transform.parent = go.transform;
			particles.transform.localPosition = prefabParticles.transform.position;
		}
		return go;
	}

	// Advanced Examples
	// It might be best to master the basics first, but this is included to tease the many possibilies LeanTween provides.

	void advancedExamples(){
		LeanTween.delayedCall(gameObject, 14f, ()=>{
			for(int i=0; i < 10; i++){
				// Instantiate Container
				GameObject rotator = new GameObject("rotator"+i);
				rotator.transform.position = new Vector3(2.71208f,7.100643f,-12.37754f);

				// Instantiate Avatar
				GameObject dude = createSpriteDude( "dude"+i, new Vector3(-2.51208f,7.100643f,-14.37754f), false);//(GameObject)GameObject.Instantiate(prefabAvatar, Vector3.zero, prefabAvatar.transform.rotation );
				dude.transform.parent = rotator.transform;
				dude.transform.localPosition = new Vector3(0f,0.5f,0.5f*i);

				// Scale, pop-in
				dude.transform.localScale = new Vector3(0f,0f,0f);
				LeanTween.scale(dude, new Vector3(0.65f,0.65f,0.65f), 1f).setDelay(i*0.2f).setEase(LeanTweenType.easeOutBack);

				// Color like the rainbow
				float period = LeanTween.tau/10*i;
				float red   = Mathf.Sin(period + LeanTween.tau*0f/3f) * 0.5f + 0.5f;
	  			float green = Mathf.Sin(period + LeanTween.tau*1f/3f) * 0.5f + 0.5f;
	  			float blue  = Mathf.Sin(period + LeanTween.tau*2f/3f) * 0.5f + 0.5f;
				Color rainbowColor = new Color(red, green, blue);
				LeanTween.color(dude, rainbowColor, 0.3f).setDelay(1.2f + i*0.4f);
				
				// Push into the wheel
				LeanTween.moveLocalZ(dude, -2f, 0.3f).setDelay(1.2f + i*0.4f).setEase(LeanTweenType.easeSpring).setOnComplete(
					()=>{
						LeanTween.rotateAround(rotator, Vector3.forward, -1080f, 12f);
					}
				);

				// Jump Up and back down
				LeanTween.moveLocalY(dude,1.17f,1.2f).setDelay(5f + i*0.2f).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad);
			
				// Alpha Out, and destroy
				LeanTween.alpha(dude, 0f, 0.6f).setDelay(9.2f + i*0.4f).setDestroyOnComplete(true).setOnComplete(
					()=>{
						Destroy( rotator ); // destroying parent as well
					}
				);	
			}

		}).setOnCompleteOnStart(true).setRepeat(-1); // Have the OnComplete play in the beginning and have the whole group repeat endlessly
	}

	#endif
}




public class Following : MonoBehaviour {

    public Transform planet;

    public Transform followArrow;

    public Transform dude1;
    public Transform dude2;
    public Transform dude3;
    public Transform dude4;
    public Transform dude5;

    public Transform dude1Title;
    public Transform dude2Title;
    public Transform dude3Title;
    public Transform dude4Title;
    public Transform dude5Title;

    private Color dude1ColorVelocity;

    private Vector3 velocityPos;

    private void Start()
    {
        followArrow.gameObject.LeanDelayedCall(3f, moveArrow).setOnStart(moveArrow).setRepeat(-1);

        // Follow Local Y Position of Arrow
        LeanTween.followDamp(dude1, followArrow, LeanProp.localY, 1.1f);
        LeanTween.followSpring(dude2, followArrow, LeanProp.localY, 1.1f);
        LeanTween.followBounceOut(dude3, followArrow, LeanProp.localY, 1.1f);
        LeanTween.followSpring(dude4, followArrow, LeanProp.localY, 1.1f, -1f, 1.5f, 0.8f);
        LeanTween.followLinear(dude5, followArrow, LeanProp.localY, 50f);

        // Follow Arrow color
        LeanTween.followDamp(dude1, followArrow, LeanProp.color, 1.1f);
        LeanTween.followSpring(dude2, followArrow, LeanProp.color, 1.1f);
        LeanTween.followBounceOut(dude3, followArrow, LeanProp.color, 1.1f);
        LeanTween.followSpring(dude4, followArrow, LeanProp.color, 1.1f, -1f, 1.5f, 0.8f);
        LeanTween.followLinear(dude5, followArrow, LeanProp.color, 0.5f);

        // Follow Arrow scale
        LeanTween.followDamp(dude1, followArrow, LeanProp.scale, 1.1f);
        LeanTween.followSpring(dude2, followArrow, LeanProp.scale, 1.1f);
        LeanTween.followBounceOut(dude3, followArrow, LeanProp.scale, 1.1f);
        LeanTween.followSpring(dude4, followArrow, LeanProp.scale, 1.1f, -1f, 1.5f, 0.8f);
        LeanTween.followLinear(dude5, followArrow, LeanProp.scale, 5f);

        // Titles
        var titleOffset = new Vector3(0.0f, -20f, -18f);
        LeanTween.followDamp(dude1Title, dude1, LeanProp.localPosition, 0.6f).setOffset(titleOffset);
        LeanTween.followSpring(dude2Title, dude2, LeanProp.localPosition, 0.6f).setOffset(titleOffset);
        LeanTween.followBounceOut(dude3Title, dude3, LeanProp.localPosition, 0.6f).setOffset(titleOffset);
        LeanTween.followSpring(dude4Title, dude4, LeanProp.localPosition, 0.6f, -1f, 1.5f, 0.8f).setOffset(titleOffset);
        LeanTween.followLinear(dude5Title, dude5, LeanProp.localPosition, 30f).setOffset(titleOffset);

        // Rotate Planet
        var localPos = Camera.main.transform.InverseTransformPoint(planet.transform.position);
        LeanTween.rotateAround(Camera.main.gameObject, Vector3.left, 360f, 300f).setPoint(localPos).setRepeat(-1);
    }

    private float fromY;
    private float velocityY;
    private Vector3 fromVec3;
    private Vector3 velocityVec3;
    private Color fromColor;
    private Color velocityColor;

    private void Update()
    {
        // Use the smooth methods to follow variables in which ever manner you wish!
        fromY = LeanSmooth.spring(fromY, followArrow.localPosition.y, ref velocityY, 1.1f);
        fromVec3 = LeanSmooth.spring(fromVec3, dude5Title.localPosition, ref velocityVec3, 1.1f);
        fromColor = LeanSmooth.spring(fromColor, dude1.GetComponent<Renderer>().material.color, ref velocityColor, 1.1f);
        Debug.Log("Smoothed y:" + fromY + " vec3:" + fromVec3 + " color:" + fromColor);
    }

	private void moveArrow()
    {
        LeanTween.moveLocalY(followArrow.gameObject, Random.Range(-100f, 100f), 0f);

        var randomCol = new Color(Random.value, Random.value, Random.value);
        LeanTween.color(followArrow.gameObject, randomCol, 0f);

        var randomVal = Random.Range(5f, 10f);
        followArrow.localScale = Vector3.one * randomVal;
    }
}



public class GeneralAdvancedTechniques : MonoBehaviour {

	public GameObject avatarRecursive;
	public GameObject avatar2dRecursive;
	public RectTransform wingPersonPanel;
	public RectTransform textField;

	public GameObject avatarMove;
	public Transform[] movePts;
	public GameObject[] avatarSpeed;
	public GameObject[] avatarSpeed2;

	private Vector3[] circleSm = new Vector3[]{ new Vector3(16f,0f,0f), new Vector3(14.56907f,8.009418f,0f), new Vector3(15.96541f,4.638379f,0f), new Vector3(11.31371f,11.31371f,0f), new Vector3(11.31371f,11.31371f,0f), new Vector3(4.638379f,15.96541f,0f), new Vector3(8.009416f,14.56908f,0f), new Vector3(-6.993822E-07f,16f,0f), new Vector3(-6.993822E-07f,16f,0f), new Vector3(-8.009419f,14.56907f,0f), new Vector3(-4.63838f,15.9654f,0f), new Vector3(-11.31371f,11.31371f,0f), new Vector3(-11.31371f,11.31371f,0f), new Vector3(-15.9654f,4.63838f,0f), new Vector3(-14.56908f,8.009415f,0f), new Vector3(-16f,-1.398764E-06f,0f), new Vector3(-16f,-1.398764E-06f,0f), new Vector3(-14.56907f,-8.009418f,0f), new Vector3(-15.9654f,-4.638382f,0f), new Vector3(-11.31371f,-11.31371f,0f), new Vector3(-11.31371f,-11.31371f,0f), new Vector3(-4.638381f,-15.9654f,0f), new Vector3(-8.009413f,-14.56908f,0f), new Vector3(1.907981E-07f,-16f,0f), new Vector3(1.907981E-07f,-16f,0f), new Vector3(8.00942f,-14.56907f,0f), new Vector3(4.638381f,-15.9654f,0f), new Vector3(11.31371f,-11.3137f,0f), new Vector3(11.31371f,-11.3137f,0f), new Vector3(15.96541f,-4.638378f,0f), new Vector3(14.56907f,-8.009418f,0f), new Vector3(16f,2.797529E-06f,0f) };
	private Vector3[] circleLrg = new Vector3[]{ new Vector3(25f,0f,0f), new Vector3(22.76418f,12.51472f,0f), new Vector3(24.94595f,7.247467f,0f), new Vector3(17.67767f,17.67767f,0f), new Vector3(17.67767f,17.67767f,0f), new Vector3(7.247467f,24.94595f,0f), new Vector3(12.51471f,22.76418f,0f), new Vector3(-1.092785E-06f,25f,0f), new Vector3(-1.092785E-06f,25f,0f), new Vector3(-12.51472f,22.76418f,0f), new Vector3(-7.247468f,24.94594f,0f), new Vector3(-17.67767f,17.67767f,0f), new Vector3(-17.67767f,17.67767f,0f), new Vector3(-24.94594f,7.247468f,0f), new Vector3(-22.76418f,12.51471f,0f), new Vector3(-25f,-2.185569E-06f,0f), new Vector3(-25f,-2.185569E-06f,0f), new Vector3(-22.76418f,-12.51472f,0f), new Vector3(-24.94594f,-7.247472f,0f), new Vector3(-17.67767f,-17.67767f,0f), new Vector3(-17.67767f,-17.67767f,0f), new Vector3(-7.247469f,-24.94594f,0f), new Vector3(-12.51471f,-22.76418f,0f), new Vector3(2.98122E-07f,-25f,0f), new Vector3(2.98122E-07f,-25f,0f), new Vector3(12.51472f,-22.76418f,0f), new Vector3(7.24747f,-24.94594f,0f), new Vector3(17.67768f,-17.67766f,0f), new Vector3(17.67768f,-17.67766f,0f), new Vector3(24.94595f,-7.247465f,0f), new Vector3(22.76418f,-12.51472f,0f), new Vector3(25f,4.371139E-06f,0f) };

	// Use this for initialization
	void Start () {
		// Recursion - Set a objects value and have it recursively effect it's children
		LeanTween.alpha( avatarRecursive, 0f, 1f).setRecursive(true).setLoopPingPong();
		LeanTween.alpha( avatar2dRecursive, 0f, 1f).setRecursive(true).setLoopPingPong();
		LeanTween.alpha( wingPersonPanel, 0f, 1f).setRecursive(true).setLoopPingPong();

		// Destroy on Complete - 

		// Chaining tweens together

		// setOnCompleteOnRepeat


		// Move to path of transforms that are moving themselves
		LeanTween.value( avatarMove, 0f, (float)movePts.Length-1, 5f).setOnUpdate((float val)=>{
			int first = (int)Mathf.Floor(val);
			int next = first < movePts.Length-1 ? first + 1 : first;
			float diff = val - (float)first;
			// Debug.Log("val:"+val+" first:"+first+" next:"+next);
			Vector3 diffPos = (movePts[next].position-movePts[first].position);
			avatarMove.transform.position = movePts[first].position + diffPos*diff;
		}).setEase(LeanTweenType.easeInOutExpo).setLoopPingPong();

		// move the pts
		for(int i = 0; i < movePts.Length; i++)
			LeanTween.moveY( movePts[i].gameObject, movePts[i].position.y + 1.5f, 1f).setDelay(((float)i)*0.2f).setLoopPingPong();


		// move objects at a constant speed
		for(int i = 0; i < avatarSpeed.Length; i++)
			LeanTween.moveLocalZ( avatarSpeed[i], (i+1)*5f, 1f).setSpeed(6f).setEase(LeanTweenType.easeInOutExpo).setLoopPingPong(); // any time you set the speed it overrides the time value
	
		// move around a circle at a constant speed
		for(int i = 0; i < avatarSpeed2.Length; i++){
			LeanTween.moveLocal( avatarSpeed2[i], i == 0 ? circleSm : circleLrg, 1f).setSpeed(20f).setRepeat(-1);
		}
			
	}
	
}





public class GeneralBasic : MonoBehaviour {

	public GameObject prefabAvatar;

	void Start () {
		// Setup
		GameObject avatarRotate = GameObject.Find("AvatarRotate");
		GameObject avatarScale = GameObject.Find("AvatarScale");
		GameObject avatarMove = GameObject.Find("AvatarMove");

		// Rotate Example
		LeanTween.rotateAround( avatarRotate, Vector3.forward, 360f, 5f);

		// Scale Example
		LeanTween.scale( avatarScale, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
		LeanTween.moveX( avatarScale, avatarScale.transform.position.x + 5f, 5f).setEase(LeanTweenType.easeOutBounce); // Simultaneously target many different tweens on the same object 

		// Move Example
		LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(-9f, 0f, 1f), 2f).setEase(LeanTweenType.easeInQuad);

		// Delay
		LeanTween.move( avatarMove, avatarMove.transform.position + new Vector3(-6f, 0f, 1f), 2f).setDelay(3f);

		// Chain properties (delay, easing with a set repeating of type ping pong)
		LeanTween.scale( avatarScale, new Vector3(0.2f, 0.2f, 0.2f), 1f).setDelay(7f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong( 3 );
	
		// Call methods after a certain time period
		LeanTween.delayedCall(gameObject, 0.2f, advancedExamples);

	}

	// Advanced Examples
	// It might be best to master the basics first, but this is included to tease the many possibilies LeanTween provides.

	void advancedExamples(){
		LeanTween.delayedCall(gameObject, 14f, ()=>{
			for(int i=0; i < 10; i++){
				// Instantiate Container
				GameObject rotator = new GameObject("rotator"+i);
				rotator.transform.position = new Vector3(10.2f,2.85f,0f);

				// Instantiate Avatar
				GameObject dude = (GameObject)GameObject.Instantiate(prefabAvatar, Vector3.zero, prefabAvatar.transform.rotation );
				dude.transform.parent = rotator.transform;
				dude.transform.localPosition = new Vector3(0f,1.5f,2.5f*i);

				// Scale, pop-in
				dude.transform.localScale = new Vector3(0f,0f,0f);
				LeanTween.scale(dude, new Vector3(0.65f,0.65f,0.65f), 1f).setDelay(i*0.2f).setEase(LeanTweenType.easeOutBack);

				// Color like the rainbow
				float period = LeanTween.tau/10*i;
				float red   = Mathf.Sin(period + LeanTween.tau*0f/3f) * 0.5f + 0.5f;
	  			float green = Mathf.Sin(period + LeanTween.tau*1f/3f) * 0.5f + 0.5f;
	  			float blue  = Mathf.Sin(period + LeanTween.tau*2f/3f) * 0.5f + 0.5f;
				Color rainbowColor = new Color(red, green, blue);
				LeanTween.color(dude, rainbowColor, 0.3f).setDelay(1.2f + i*0.4f);
				
				// Push into the wheel
				LeanTween.moveZ(dude, 0f, 0.3f).setDelay(1.2f + i*0.4f).setEase(LeanTweenType.easeSpring).setOnComplete(
					()=>{
						LeanTween.rotateAround(rotator, Vector3.forward, -1080f, 12f);
					}
				);

				// Jump Up and back down
				LeanTween.moveLocalY(dude,4f,1.2f).setDelay(5f + i*0.2f).setLoopPingPong(1).setEase(LeanTweenType.easeInOutQuad);
			
				// Alpha Out, and destroy
				LeanTween.alpha(dude, 0f, 0.6f).setDelay(9.2f + i*0.4f).setDestroyOnComplete(true).setOnComplete(
					()=>{
						Destroy( rotator ); // destroying parent as well
					}
				);	
			}

		}).setOnCompleteOnStart(true).setRepeat(-1); // Have the OnComplete play in the beginning and have the whole group repeat endlessly
	}
}



public class GeneralCameraShake : MonoBehaviour {

	private GameObject avatarBig;
	private float jumpIter = 9.5f;
	private AudioClip boomAudioClip;

	// Use this for initialization
	void Start () {
		avatarBig = GameObject.Find("AvatarBig");

		AnimationCurve volumeCurve = new AnimationCurve( new Keyframe(8.130963E-06f, 0.06526042f, 0f, -1f), new Keyframe(0.0007692695f, 2.449077f, 9.078861f, 9.078861f), new Keyframe(0.01541314f, 0.9343268f, -40f, -40f), new Keyframe(0.05169491f, 0.03835937f, -0.08621139f, -0.08621139f));
		AnimationCurve frequencyCurve = new AnimationCurve( new Keyframe(0f, 0.003005181f, 0f, 0f), new Keyframe(0.01507768f, 0.002227979f, 0f, 0f));
		boomAudioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato( new Vector3[]{ new Vector3(0.1f,0f,0f)} ));
		

		bigGuyJump();	
	}

	void bigGuyJump(){
		float height = Mathf.PerlinNoise(jumpIter, 0f)*10f;
		height = height*height * 0.3f;
		// Debug.Log("height:"+height+" jumpIter:"+jumpIter);

		LeanTween.moveY(avatarBig, height, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete( ()=>{
			LeanTween.moveY(avatarBig, 0f, 0.27f).setEase(LeanTweenType.easeInQuad).setOnComplete( ()=>{
				LeanTween.cancel(gameObject);

				/**************
				* Camera Shake
				**************/
				
				float shakeAmt = height*0.2f; // the degrees to shake the camera
				float shakePeriodTime = 0.42f; // The period of each shake
				float dropOffTime = 1.6f; // How long it takes the shaking to settle down to nothing
				LTDescr shakeTween = LeanTween.rotateAroundLocal( gameObject, Vector3.right, shakeAmt, shakePeriodTime)
				.setEase( LeanTweenType.easeShake ) // this is a special ease that is good for shaking
				.setLoopClamp()
				.setRepeat(-1);

				// Slow the camera shake down to zero
				LeanTween.value(gameObject, shakeAmt, 0f, dropOffTime).setOnUpdate( 
					(float val)=>{
						shakeTween.setTo(Vector3.right*val);
					}
				).setEase(LeanTweenType.easeOutQuad);


				/********************
				* Shake scene objects
				********************/

				// Make the boxes jump from the big stomping
				GameObject[] boxes = GameObject.FindGameObjectsWithTag("Respawn"); // I just arbitrarily tagged the boxes with this since it was available in the scene
		        foreach (GameObject box in boxes) {
		            box.GetComponent<Rigidbody>().AddForce(Vector3.up * 100 * height);
		        }

		        // Make the lamps spin from the big stomping
		        GameObject[] lamps = GameObject.FindGameObjectsWithTag("GameController"); // I just arbitrarily tagged the lamps with this since it was available in the scene
		        foreach (GameObject lamp in lamps) {
		        	float z = lamp.transform.eulerAngles.z;
		        	z = z > 0.0f && z < 180f ? 1 : -1; // push the lamps in whatever direction they are currently swinging
		            lamp.GetComponent<Rigidbody>().AddForce(new Vector3(z, 0f, 0f ) * 15 * height);
		        }

		        // Play BOOM!
		        LeanAudio.play(boomAudioClip, transform.position, height*0.2f); // Like this sound? : http://leanaudioplay.dentedpixel.com/?d=a:fvb:8,0,0.003005181,0,0,0.01507768,0.002227979,0,0,8~8,8.130963E-06,0.06526042,0,-1,0.0007692695,2.449077,9.078861,9.078861,0.01541314,0.9343268,-40,-40,0.05169491,0.03835937,-0.08621139,-0.08621139,8~0.1,0,0,~44100
		        
		        // Have the jump happen again 2 seconds from now
		        LeanTween.delayedCall(2f, bigGuyJump);
			});
		});
		jumpIter += 5.2f;
	}

}
#endif



public class GeneralEasingTypes : MonoBehaviour {

	public float lineDrawScale = 10f;
	public AnimationCurve animationCurve;

	private string[] easeTypes = new string[]{
		"EaseLinear","EaseAnimationCurve","EaseSpring",
		"EaseInQuad","EaseOutQuad","EaseInOutQuad",
		"EaseInCubic","EaseOutCubic","EaseInOutCubic",
		"EaseInQuart","EaseOutQuart","EaseInOutQuart",
		"EaseInQuint","EaseOutQuint","EaseInOutQuint",
		"EaseInSine","EaseOutSine","EaseInOutSine",
		"EaseInExpo","EaseOutExpo","EaseInOutExpo",
		"EaseInCirc","EaseOutCirc","EaseInOutCirc",
		"EaseInBounce","EaseOutBounce","EaseInOutBounce",
		"EaseInBack","EaseOutBack","EaseInOutBack",
		"EaseInElastic","EaseOutElastic","EaseInOutElastic",
        "EasePunch","EaseShake",
	};

	void Start () {

		demoEaseTypes();
	}

	private void demoEaseTypes(){
		for(int i = 0; i < easeTypes.Length; i++){
			string easeName = easeTypes[i];
			Transform obj1 = GameObject.Find(easeName).transform.Find("Line");
			float obj1val = 0f;
			LTDescr lt = LeanTween.value( obj1.gameObject, 0f, 1f, 5f).setOnUpdate( (float val)=>{
				Vector3 vec = obj1.localPosition;
				vec.x = obj1val*lineDrawScale;
				vec.y = val*lineDrawScale;

				obj1.localPosition = vec;

				obj1val += Time.deltaTime/5f;
				if(obj1val>1f)
					obj1val = 0f;
			});
			if(easeName.IndexOf("AnimationCurve")>=0){
				lt.setEase(animationCurve);
            }else{
				MethodInfo theMethod = lt.GetType().GetMethod("set"+easeName);
				theMethod.Invoke(lt, null);
			}

			if (easeName.IndexOf("EasePunch") >= 0) {
				lt.setScale(1f);
			} else if (easeName.IndexOf("EaseOutBounce") >= 0) {
				lt.setOvershoot(2f);
			}
		}

		LeanTween.delayedCall(gameObject, 10f, resetLines);
		LeanTween.delayedCall(gameObject, 10.1f, demoEaseTypes);
	}

	private void resetLines(){
		for(int i = 0; i < easeTypes.Length; i++){
			Transform obj1 = GameObject.Find(easeTypes[i]).transform.Find("Line");
			obj1.localPosition = new Vector3(0f,0f,0f);
		}
	}

}



public class GeneralEventsListeners : MonoBehaviour {

	Vector3 towardsRotation;
	float turnForLength = 0.5f;
	float turnForIter = 0f;
	Color fromColor;

	// It's best to make this a public enum that you use throughout your project, so every class can have access to it
	public enum MyEvents{ 
		CHANGE_COLOR,
		JUMP,
		LENGTH
	}

	void Awake(){
		LeanTween.LISTENERS_MAX = 100; // This is the maximum of event listeners you will have added as listeners
		LeanTween.EVENTS_MAX = (int)MyEvents.LENGTH; // The maximum amount of events you will be dispatching

		fromColor = GetComponent<Renderer>().material.color;
	}

	void Start () {
		// Adding Listeners, it's best to use an enum so your listeners are more descriptive but you could use an int like 0,1,2,...
		LeanTween.addListener(gameObject, (int)MyEvents.CHANGE_COLOR, changeColor);
		LeanTween.addListener(gameObject, (int)MyEvents.JUMP, jumpUp);
	}

	// ****** Event Listening Methods

	void jumpUp( LTEvent e ){
		GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300f);
	}

	void changeColor( LTEvent e ){
		Transform tran = (Transform)e.data;
		float distance = Vector3.Distance( tran.position, transform.position);
		Color to = new Color(Random.Range(0f,1f),0f,Random.Range(0f,1f));
		LeanTween.value( gameObject, fromColor, to, 0.8f ).setLoopPingPong(1).setDelay(distance*0.05f).setOnUpdate(
			(Color col)=>{
				GetComponent<Renderer>().material.color = col;
			}
		);
	}

	// ****** Physics / AI Stuff

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.layer!=2)
			towardsRotation = new Vector3(0f, Random.Range(-180, 180), 0f);
    }

     void OnCollisionStay(Collision collision) {
     	if(collision.gameObject.layer!=2){
     		turnForIter = 0f;
	    	turnForLength = Random.Range(0.5f, 1.5f);
	    }
     }

	void FixedUpdate(){
		if(turnForIter < turnForLength){
			GetComponent<Rigidbody>().MoveRotation( GetComponent<Rigidbody>().rotation * Quaternion.Euler(towardsRotation * Time.deltaTime ) );
			turnForIter += Time.deltaTime;
		}

		GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4.5f);
	}

	// ****** Key and clicking detection

	void OnMouseDown(){
		if(Input.GetKey( KeyCode.J )){ // Are you also pressing the "j" key while clicking
			LeanTween.dispatchEvent((int)MyEvents.JUMP);
		}else{
			LeanTween.dispatchEvent((int)MyEvents.CHANGE_COLOR, transform); // with every dispatched event, you can include an object (retrieve this object with the *.data var in LTEvent)
		}
	}
}
#endif





public class GeneralSequencer : MonoBehaviour {

	public GameObject avatar1;

    public GameObject star;

	public GameObject dustCloudPrefab;

	public float speedScale = 1f;

	public void Start(){

		// Jump up
		var seq = LeanTween.sequence();


		seq.append( LeanTween.moveY( avatar1, avatar1.transform.localPosition.y + 6f, 1f).setEaseOutQuad() );

        // Power up star, use insert when you want to branch off from the regular sequence (this does not push back the delay of other subsequent tweens)
        seq.insert( LeanTween.alpha(star, 0f, 1f) );
        seq.insert( LeanTween.scale( star, Vector3.one * 3f, 1f) );

		// Rotate 360
		seq.append( LeanTween.rotateAround( avatar1, Vector3.forward, 360f, 0.6f ).setEaseInBack() );

		// Return to ground
		seq.append( LeanTween.moveY( avatar1, avatar1.transform.localPosition.y, 1f).setEaseInQuad() );

		// Kick off spiraling clouds - Example of appending a callback method
		seq.append(() => {
			for(int i = 0; i < 50f; i++){
				GameObject cloud = Instantiate(dustCloudPrefab) as GameObject;
				cloud.transform.parent = avatar1.transform;
				cloud.transform.localPosition = new Vector3(Random.Range(-2f,2f),0f,0f);
				cloud.transform.eulerAngles = new Vector3(0f,0f,Random.Range(0,360f));

				var range = new Vector3(cloud.transform.localPosition.x, Random.Range(2f,4f), Random.Range(-10f,10f));

				// Tweens not in a sequence, because we want them all to animate at the same time
				LeanTween.moveLocal(cloud, range, 3f*speedScale).setEaseOutCirc();
				LeanTween.rotateAround(cloud, Vector3.forward, 360f*2, 3f*speedScale).setEaseOutCirc();
				LeanTween.alpha(cloud, 0f, 3f*speedScale).setEaseOutCirc().setDestroyOnComplete(true);
			}
		});

		// You can speed up or slow down the sequence of events
		seq.setScale(speedScale);

        // seq.reverse(); // not working yet

        // Testing canceling sequence after a bit of time
        //LeanTween.delayedCall(3f, () =>
        //{
        //    LeanTween.cancel(seq.id);
        //});
	}
}





public class GeneralSimpleUI : MonoBehaviour {
	#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_0_1 && !UNITY_4_1 && !UNITY_4_2 && !UNITY_4_3 && !UNITY_4_5

	public RectTransform button;

	void Start () {
		Debug.Log("For better examples see the 4.6_Examples folder!");
		if(button==null){
			Debug.LogError("Button not assigned! Create a new button via Hierarchy->Create->UI->Button. Then assign it to the button variable");
			return;
		}
		
		// Tweening various values in a block callback style
		LeanTween.value(button.gameObject, button.anchoredPosition, new Vector2(200f,100f), 1f ).setOnUpdate( 
			(Vector2 val)=>{
				button.anchoredPosition = val;
			}
		);

		LeanTween.value(gameObject, 1f, 0.5f, 1f ).setOnUpdate( 
			(float volume)=>{
				Debug.Log("volume:"+volume);
			}
		);

		LeanTween.value(gameObject, gameObject.transform.position, gameObject.transform.position + new Vector3(0,1f,0), 1f ).setOnUpdate( 
			(Vector3 val)=>{
				gameObject.transform.position = val;
			}
		);

		LeanTween.value(gameObject, Color.red, Color.green, 1f ).setOnUpdate( 
			(Color val)=>{
				UnityEngine.UI.Image image = (UnityEngine.UI.Image)button.gameObject.GetComponent( typeof(UnityEngine.UI.Image) );
				image.color = val;
			}
		);

		// Tweening Using Unity's new Canvas GUI System
		LeanTween.move(button, new Vector3(200f,-100f,0f), 1f).setDelay(1f);
		LeanTween.rotateAround(button, Vector3.forward, 90f, 1f).setDelay(2f);
		LeanTween.scale(button, button.localScale*2f, 1f).setDelay(3f);
		LeanTween.rotateAround(button, Vector3.forward, -90f, 1f).setDelay(4f).setEase(LeanTweenType.easeInOutElastic);
	}

	#else
	void Start(){
		Debug.LogError("Unity 4.6+ is required to use the new UI");
	}
	
	#endif
}




public class GeneralUISpace : MonoBehaviour {

	public RectTransform mainWindow;
	public RectTransform mainParagraphText;
	public RectTransform mainTitleText;
	public RectTransform mainButton1;
	public RectTransform mainButton2;

	public RectTransform pauseRing1;
	public RectTransform pauseRing2;
	public RectTransform pauseWindow;

	public RectTransform chatWindow;
	public RectTransform chatRect;
	public Sprite[] chatSprites;
	public RectTransform chatBar1;
	public RectTransform chatBar2;
	public UnityEngine.UI.Text chatText;

	public RectTransform rawImageRect;

	void Start () {
		// Time.timeScale = 1f/4f;
		
		// *********** Main Window **********
		// Scale the whole window in
		mainWindow.localScale = Vector3.zero;
		LeanTween.scale( mainWindow, new Vector3(1f,1f,1f), 0.6f).setEase(LeanTweenType.easeOutBack);
		LeanTween.alphaCanvas( mainWindow.GetComponent<CanvasGroup>(), 0f, 1f).setDelay(2f).setLoopPingPong().setRepeat(2);

		// Fade the main paragraph in while moving upwards
		mainParagraphText.anchoredPosition3D += new Vector3(0f,-10f,0f);
		LeanTween.textAlpha( mainParagraphText, 0f, 0.6f).setFrom(0f).setDelay(0f);
		LeanTween.textAlpha( mainParagraphText, 1f, 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.move( mainParagraphText, mainParagraphText.anchoredPosition3D + new Vector3(0f,10f,0f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);

		// Flash text to purple and back
		LeanTween.textColor( mainTitleText, new Color(133f/255f,145f/255f,223f/255f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f).setLoopPingPong().setRepeat(-1);

		// Fade button in
		LeanTween.textAlpha(mainButton2, 1f, 2f ).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		LeanTween.alpha(mainButton2, 1f, 2f ).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);

		// Pop size of button
		LeanTween.size(mainButton1, mainButton1.sizeDelta * 1.1f, 0.5f).setDelay(3f).setEaseInOutCirc().setRepeat(6).setLoopPingPong();


		// *********** Pause Button **********
		// Drop pause button in
		pauseWindow.anchoredPosition3D += new Vector3(0f,200f,0f);
		LeanTween.moveY( pauseWindow, pauseWindow.anchoredPosition3D.y + -200f, 0.6f).setEase(LeanTweenType.easeOutSine).setDelay(0.6f);

		// Punch Pause Symbol
		RectTransform pauseText = pauseWindow.Find("PauseText").GetComponent<RectTransform>();
		LeanTween.moveZ( pauseText, pauseText.anchoredPosition3D.z - 80f, 1.5f).setEase(LeanTweenType.punch).setDelay(2.0f);

		// Rotate rings around in opposite directions
		LeanTween.rotateAroundLocal(pauseRing1, Vector3.forward, 360f, 12f).setRepeat(-1);
		LeanTween.rotateAroundLocal(pauseRing2, Vector3.forward, -360f, 22f).setRepeat(-1);
		

		// *********** Chat Window **********
		// Flip the chat window in
		chatWindow.RotateAround(chatWindow.position, Vector3.up, -180f);
		LeanTween.rotateAround(chatWindow, Vector3.up, 180f, 2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.2f);

		// Play a series of sprites on the window on repeat endlessly
		LeanTween.play(chatRect, chatSprites).setLoopPingPong();

		// Animate the bar up and down while changing the color to red-ish
		LeanTween.color( chatBar2, new Color(248f/255f,67f/255f,108f/255f, 0.5f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong().setDelay(1.2f);
		LeanTween.scale( chatBar2, new Vector2(1f,0.7f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong();

		// Write in paragraph text
		string origText = chatText.text;
		chatText.text = "";
		LeanTween.value(gameObject, 0, (float)origText.Length, 6f).setEase(LeanTweenType.easeOutQuad).setOnUpdate( (float val)=>{
			chatText.text = origText.Substring( 0, Mathf.RoundToInt( val ) );
		}).setLoopClamp().setDelay(2.0f);

		// Raw Image
		LeanTween.alpha(rawImageRect,0f,1f).setLoopPingPong();
	}

}
