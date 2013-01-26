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

    #endregion

    #region Constructors

    private GameManager()
    {
		
    }
	
	public void Update()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			if(gameState != GameState.GameOver && gameState != GameState.OpeningWindow)
			{
				TogglePause(); 
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
        Debug.Log("Direct to play, just change to OpeningWindow.");
        gameState = GameState.PlayGame;
		scaleFormCamera = Camera.mainCamera.GetComponent<ScaleformCamera>();
        Debug.Log("Disable all menus");
	}

    #endregion

    #region UI Events
    void BackToMain()
    {
        Application.LoadLevel(Application.loadedLevel);
		scaleFormCamera.hud.OpenMainMenu();
    }
    void Quit()
    {
        Application.Quit();
    }
    void Play()
    {
        gameState = GameState.PlayGame;
    }
    void TogglePause()
    {
		if(gameState == GameState.Pause)
		{
			gameState = GameState.PlayGame;
		}
		else
		{
			gameState = GameState.Pause;
			scaleFormCamera.hud.PauseGame();
		}
       
    }
	
	public void ResumeGame()
	{
		gameState = GameState.PlayGame;
	}
    void GameOver()
    {
        gameState = GameState.GameOver;
		scaleFormCamera.hud.OpenEndGameMenu();
    }
    #endregion

    #region Utilities

   

    #endregion


}
