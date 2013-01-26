using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
using Scaleform;

// Note that SFCamera is currently a Singleton as it creates a new SFMgr in Start().
public class ScaleformCamera : SFCamera {
	
	
	public UI_Scene_HUD				hud				= null;
	public Boolean gamePaused = false;

	// Initialization related notes:
	// We have three levels in the game- dummy level, mainmenu level and the main game level. The dummy level
	// is needed since even though unity claims all rendering related initialization is complete before
	// Start function is called, in our experience, it's really not. So if we initialize Scaleform rendering 
	// during MainMenu::Start, we see rendering artifacts. Therefore we introduce a dummy level which just transitions
	// to the mainmenu level. This removes rendering artifacts in the main menu. Now for loading up the main menu swf
	// itself, there are two choices. It can be done in the Start function after we initialize Scaleform Manager, or 
	// it can be done in OnLevelLoaded function. In our experience, the OnLevelLoaded can be called even before
	// MainMenu::Start is called, which is problematic since we are trying to load the mainmenu swf before the manager
	// has been initialized. Therefore, we choose to load the mainmenu swf during start after creation of SFManager. 
	// Another option could be to introduce another level in between the dummylevel and the mainmenu level which just
	// initializes the SFManager. This would be useful if the user intends the Manager to persist between successive
	// MainMEnu-Game-PauseMenu-MainMenu cycle. Currently, the manager (and hence Scaleform runtime) is destroyed 
	// after exit from pause menu and then reinitialized. 
	
	// Hides the Start function in the base SFCamera. Will be called every time the ScaleformCamera (Main Camera game object)
	// is created. 
	new public  IEnumerator Start()
	{
		base.Start();
		hud = null;
		CreateGameHud();
		hud.SetFocus(true);
		
		PauseGame();
		//hud.OpenEndGameMenu();
		yield return StartCoroutine("CallPluginAtEndOfFrames");
		
	}
	
	new public void Update()
	{
		base.Update();
		if(hud != null)
		{
			if(hud.pauseMenuOpen && !gamePaused)
			{
				hud.ClosePauseMenu();
			}
		}
		
	}
	

	private void CreateGameHud()
	{
		if (hud == null)
		{
			hud = new UI_Scene_HUD(SFMgr, CreateMovieCreationParams("GameJam2013.swf"));
		}
	}
	

	public void DestroyHUD()
	{
		if (hud != null)
		{
			SFMgr.DestroyMovie(hud);
			hud = null; // @TODO: Need some sort of replacement for this if it won't work.
		}
	}
	
	public void PauseGame()
	{
		gamePaused = true;
		hud.PauseGame();
	}
	
	public void ResumeGame()
	{
		gamePaused = false;
	}
	

}