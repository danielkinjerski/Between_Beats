using UnityEngine;
using System.Collections;
using Scaleform;

#region Enums
public enum GameState
{
    OpeningWindow = 1,
    PlayGame = 2,
    GameOver = 3,
    Pause = 4
}
#endregion

public class GameManager : MonoBehaviour
{

    #region Variables

    public GameObject Character;
    public static GameState gameState = GameState.OpeningWindow;
	public ScaleformCamera scaleFormCamera;
    private bool toggle;
    public bool cheats;
	public bool menuOpen;

    #endregion

    #region Constructors

    private GameManager()
    {
		
    }
	
	public void Update()
	{
		if (Input.GetButtonDown("Escape")) 
		{
			if(gameState != GameState.GameOver && gameState != GameState.OpeningWindow)
			{
				TogglePause(); 
			}
		}
		
		if (Input.GetButtonDown("Confirm")) 
		{
			Debug.Log(gameState.ToString());
			switch(gameState)
			{
			case GameState.GameOver:
				
			case GameState.OpeningWindow:
				
			case GameState.Pause:
				scaleFormCamera.hud.HandelConfirmPress(gameState.ToString());
				break;
			}
		}
	}

    ~GameManager()
    {

    }

    #endregion

    #region Mono Inherit Functions

    void Awake () 
	{
        //gameState = GameState.OpeningWindow;
        gameState = GameState.OpeningWindow;
		menuOpen = true;
		scaleFormCamera = Camera.mainCamera.GetComponent<ScaleformCamera>();
        
	}

    #endregion

    #region UI Events
    public void BackToMain()
    {
        Application.LoadLevel(Application.loadedLevel);
		scaleFormCamera.hud.OpenMainMenu();
		gameState = GameState.OpeningWindow;
		menuOpen = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Play()
    {
        gameState = GameState.PlayGame;
		scaleFormCamera.hud.OpenHUD();
		menuOpen = false;
    }
    void TogglePause()
    {
		if(gameState == GameState.Pause)
		{
			gameState = GameState.PlayGame;
			scaleFormCamera.hud.ClosePauseMenu();
			menuOpen = false;
		}
		else
		{
			gameState = GameState.Pause;
			scaleFormCamera.hud.PauseGame();
			menuOpen = true;
		}
       
    }
	
	public void ResumeGame()
	{
		gameState = GameState.PlayGame;
		menuOpen = false;
	}
    public void GameOver()
    {
        gameState = GameState.GameOver;
		scaleFormCamera.hud.OpenEndGameMenu();
		menuOpen = true;
    }
    #endregion

    #region Utilities

   

    #endregion


}
