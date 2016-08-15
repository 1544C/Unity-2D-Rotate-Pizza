using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Image imgFade;
	public GameObject imgBackground;
	public GameObject imgTheEnd;
	public GameObject goGameOverScreen;
	public AudioClip acEndSong;
	public static GameManager instance = null;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		} else if(instance != this)
		{
			Destroy(gameObject);
		}
	}

	public IEnumerator FadeAndChangeBackgound()
	{
		yield return new WaitForSeconds(3);
		StartCoroutine(Fade (0.005f));
		yield return new WaitForSeconds(5);
		imgBackground.SetActive(true);
		StartCoroutine(Fade (-0.005f));
		SoundManager.instance.musicSource.clip = acEndSong;
		SoundManager.instance.musicSource.Play();
		yield return new WaitForSeconds(20);
		imgTheEnd.SetActive(true);
		yield return new WaitForSeconds(5);
		goGameOverScreen.SetActive(true);
		yield return null;
	}
	private IEnumerator Fade(float fltFade)
	{
		Color color;
		for(int i = 0; i < 255; i++)
		{
			color = imgFade.color;
			color.a += fltFade;
			imgFade.color = color;
			yield return null;
		}
	}
}