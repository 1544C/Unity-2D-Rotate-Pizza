using UnityEngine;
using System.Collections;


public class Rotor : MonoBehaviour {

	public float glPlFloatRotationSpeed;
	public int glPlIntSecsBeforePulling;
	public int glPlIntSecsAfterPulling;
	public Transform glPlTfsBlades;
	public SpinObject glPlSpinObject;
	public AudioClip glPlACHelicopter;
	private states glPlCurrentState = states.Waiting;
	private Animator animator;

	private enum states
	{
		Waiting,
		Ready,
		PullingOut,
		PulledOut
	}

	void Start()
	{
		glPlCurrentState = states.Waiting;
		animator = gameObject.GetComponentInParent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(glPlCurrentState)
		{
			case states.Waiting:
				CheckHoldingObject();
				break;
			case states.Ready:
				StartCoroutine(PullOutRotor());
				break;
			case states.PulledOut:
				RotateBlades();
				break;
		}
	}

	private IEnumerator PullOutRotor()
	{
		glPlCurrentState = states.PullingOut;
		yield return new WaitForSeconds (glPlIntSecsBeforePulling);
		SoundManager.instance.musicSource.Stop();
		Vector3 v3EndPosition;
		Vector3 v3MoveTowards;

		v3EndPosition = transform.position + new Vector3(0f,2f,0f);

		float fltSqrDistanceLeft = (transform.position - v3EndPosition).sqrMagnitude;
		while(fltSqrDistanceLeft > float.Epsilon)
		{
			v3MoveTowards = Vector3.MoveTowards(transform.position,
			                                    v3EndPosition,
			                                    Time.deltaTime );
			transform.position = v3MoveTowards;
			fltSqrDistanceLeft = (transform.position - v3EndPosition).sqrMagnitude;
			yield return null;
		}

		yield return new WaitForSeconds (glPlIntSecsAfterPulling);
		glPlCurrentState = states.PulledOut;
		SoundManager.instance.PlaySingle(glPlACHelicopter);
		animator.SetBool("FlyingAway",true);
		StartCoroutine(GameManager.instance.FadeAndChangeBackgound());
	}

	private void RotateBlades()
	{
		glPlTfsBlades.Rotate(Vector3.up * Time.deltaTime,glPlFloatRotationSpeed);
	}

	private void CheckHoldingObject()
	{
		if(!glPlSpinObject.glPlBoolCanRotate)
		{
			glPlCurrentState = states.Ready;
		}
	}
}