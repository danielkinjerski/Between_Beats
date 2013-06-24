using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {
	
	public Texture2D[] SplashScreens;
	public float defaultPlayLength = 3;
	
	private int currentMovie;
	private bool betweenFades;
	
	// Use this for initialization
	void Start () 
	{
		betweenFades = true;
		StartCoroutine(playSplash());
	}
	
	void OnGUI()
	{
		if(!betweenFades)
		{
			GUI.DrawTexture(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), SplashScreens[currentMovie]);
		}
	}
	
	IEnumerator playSplash()
	{
		for ( currentMovie = 0; currentMovie < SplashScreens.Length; currentMovie++) {
			
			betweenFades = false;
			//fade in
			Debug.Log("FADE IN");
			CameraFade.StartAlphaFade(Color.black, true, defaultPlayLength);
			while(GameObject.Find("CameraFade"))
			{
				yield return null;
			}
			
			//wait
						Debug.Log("wait");
			yield return new WaitForSeconds(defaultPlayLength);
			
			//fade out
						Debug.Log("FADE out");
			CameraFade.StartAlphaFade(Color.black, false, defaultPlayLength, 0, () => { betweenFades = true; });
			while(GameObject.Find("CameraFade"))
			{
				yield return null;
			}
		}
        Application.LoadLevel(1);
	}
}
