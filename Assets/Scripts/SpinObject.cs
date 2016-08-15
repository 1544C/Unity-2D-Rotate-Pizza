using UnityEngine;
using System.Collections;

public class SpinObject : MonoBehaviour {

	public float glPlFltDeltaLimit;
	public float glPlFltDeltaReduce;
	public int glPlIntLapsBeforeStopping;
	public bool glPlBoolCanRotate {get; set;}
	public AudioClip glPlSpinSound;
	private float glPrFltDeltaRotation;
	private float glPrFltPreviousRotation;
	private float glPrFltCurrentRotation;
	private int glPrIntCurrentLaps;
	private float glPrFloatRotation;
	private float glPrFltQuarterRotation;
	private bool boolCountRotations;


	void Start()
	{
		glPrIntCurrentLaps = glPlIntLapsBeforeStopping;
		glPrFloatRotation = 0f;
		glPlBoolCanRotate = true;
		boolCountRotations = true;
	}

	// Update is called once per frame
	void Update () {
		RotateThis();
		CountRotations();
	}

	private void CountRotations()
	{
		if(boolCountRotations)
		{
			if(Mathf.Sign(glPrFltDeltaRotation) == 1)
			{
				glPrFloatRotation += glPrFltDeltaRotation;
			}
			
			if(glPrFloatRotation >= 360)
			{
				glPrFloatRotation -= 360;
				glPrIntCurrentLaps -= 1;
				if(glPrIntCurrentLaps <= 0)
				{
					glPlBoolCanRotate = false;
					StartCoroutine(EnableSpinForever(22));
				}
			}
		}
	}

	private void RotateThis()
	{
		if (Input.GetMouseButtonDown (0) && glPlBoolCanRotate) {

			// Get initial rotation of this game object
			glPrFltDeltaRotation = 0f;
			glPrFltPreviousRotation = angleBetweenPoints (transform.position, Camera.main.ScreenToWorldPoint (Input.mousePosition));
		} else if (Input.GetMouseButton (0) && glPlBoolCanRotate) {

			// Rotate along the mouse under Delta Rotation Limit
			glPrFltCurrentRotation = angleBetweenPoints (transform.position, Camera.main.ScreenToWorldPoint (Input.mousePosition));
			glPrFltDeltaRotation = Mathf.DeltaAngle (glPrFltCurrentRotation, glPrFltPreviousRotation);
			if (Mathf.Abs (glPrFltDeltaRotation) > glPlFltDeltaLimit) {
				glPrFltDeltaRotation = glPlFltDeltaLimit * Mathf.Sign (glPrFltDeltaRotation);
			}
			glPrFltPreviousRotation = glPrFltCurrentRotation;
			transform.Rotate (Vector3.back * Time.deltaTime, glPrFltDeltaRotation);
		} else {

			// Inertia
			transform.Rotate (Vector3.back * Time.deltaTime, glPrFltDeltaRotation);
			glPrFltDeltaRotation = Mathf.Lerp (glPrFltDeltaRotation, 0, glPlFltDeltaReduce * Time.deltaTime);
		}

		PlaySound();
	}

	private float angleBetweenPoints (Vector2 v2Position1, Vector2 v2Position2)
	{
		Vector2 v2FromLine = v2Position2 - v2Position1;
		Vector2 v2ToLine = new Vector2 (1, 0);
		
		float fltAngle = Vector2.Angle (v2FromLine, v2ToLine);

		// If rotation is more than 180
		Vector3 v3Cross = Vector3.Cross (v2FromLine, v2ToLine);
		if (v3Cross.z > 0) {
			fltAngle = 360f - fltAngle;
		}
		
		return fltAngle;
	}

	private IEnumerator EnableSpinForever(int intWaitSeconds)
	{
		yield return new WaitForSeconds(intWaitSeconds);
		glPlBoolCanRotate = true;
		boolCountRotations = false;
	}

	private void PlaySound()
	{
		glPrFltQuarterRotation += Mathf.Abs(glPrFltDeltaRotation);

		if(glPrFltQuarterRotation >= 90)
		{
			glPrFltQuarterRotation -= 90;
			SoundManager.instance.RandomizeSfxShot(glPlSpinSound);
		}
	}
}
