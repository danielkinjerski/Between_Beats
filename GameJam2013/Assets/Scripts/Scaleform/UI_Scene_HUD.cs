using System;
using UnityEngine;
using System.Runtime.InteropServices;
using Scaleform;
using Scaleform.GFx;

public class UI_Scene_HUD : Movie
{
	protected Value hudMovie = null;
	
	
	
	// Required to implement this constructor.
	public UI_Scene_HUD(SFManager sfmgr, SFMovieCreationParams cp):
		base(sfmgr, cp)
	{
		if (MovieID != -1)
		{
		//	SetBackgroundAlpha(1);
		}
		
		// 
	}
	
	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
	public void OnRegisterSWFCallback(Value movieRef)
	{
		Debug.Log("UI_Scene_MainMenu::OnRegisterSWFCallback()");
		hudMovie = movieRef;
		Console.WriteLine("mainmenu type = " + hudMovie.type);
		Init();
	}
	
	public void Init()
	{
		 //
	}

	// Callback from the content that provides a reference to the MainMenu object once it's fully loaded.
	public void RegisterMovies(Value movieRef)
	{
		hudMovie = movieRef;
	}

	// Callback from the content to launch the game when the "close" animation has finished playing.
	public void OnStartGameCallback()
	{
		
		sfMgr.DestroyMovie(this);
		//ScaleformCamera sfCamera = Component.FindObjectOfType( typeof(ScaleformCamera) ) as ScaleformCamera;
		//sfCamera.mainMenu = null;
		//sfCamera.OnLevelLoadStart();
	}

	// Callback from the content to launch the game when the "close" animation has finished playing.
	public void OnExitGameCallback()
	{
		Console.WriteLine("In OnExitGameCallback");
		sfMgr.DestroyMovie(this);
		// Application.Quit() is Ignored in the editor!
		Application.Quit();
		// Application.LoadLevelAsync("Level");
		// Destroy(this); // NFM: Do our Value references need to be cleared? How do we cleanup a movie?
	}
	
}