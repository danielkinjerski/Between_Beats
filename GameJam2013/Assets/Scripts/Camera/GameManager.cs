using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// Prefab References
    /// </summary>
    public GameObject PrefabPlayer, PrefabGoal, PrefabMainPulse, PrefabPlayerCam, PrefabGround, PrefabGameLight;

    /// <summary>
    /// Instance references
    /// </summary>
    private GameObject Player, Goal, MainPulse, PlayerCam, Ground, GameLight;

    /// <summary>
    /// Blocks and stuff
    /// </summary>
    private List<GameObject> obstacles = new List<GameObject>();

    /// <summary>
    /// Game state accessible by everything
    /// </summary>
    public static GameState gameState = GameState.OpeningWindow;

    /// <summary>
    /// Yeah scaleform, should replace with NGUI asap
    /// </summary>
	public ScaleformCamera scaleFormCamera;

    /// <summary>
    /// enable cheats, no death etc.
    /// </summary>
    public bool cheats, menuOpen;

    /// <summary>
    /// internal bools for not screwing up
    /// </summary>
    private bool toggle;

    /// <summary>
    /// levels completed
    /// </summary>
    public static int levelsCompleted = 0;

    #endregion

    #region Constructors

    private GameManager()
    {
		
    }	

    ~GameManager()
    {

    }

    #endregion

    #region Mono Inherit Functions

    /// <summary>
    /// We are first, so this is important
    /// </summary>
    void Awake () 
	{
        gameState = GameState.OpeningWindow;
        levelsCompleted = 0;
		menuOpen = true;
        PlayerCam = GameObject.Instantiate(PrefabPlayerCam, Vector3.zero, Quaternion.identity) as GameObject;
        scaleFormCamera = PlayerCam.GetComponent<ScaleformCamera>();
	}

    public void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            if (gameState != GameState.GameOver && gameState != GameState.OpeningWindow)
            {
                TogglePause();
            }
        }

        if (Input.GetButtonDown("Confirm"))
        {
            Debug.Log(gameState.ToString());
            switch (gameState)
            {
                case GameState.GameOver:

                case GameState.OpeningWindow:

                case GameState.Pause:
                    scaleFormCamera.hud.HandelConfirmPress(gameState.ToString());
                    break;
            }
        }
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

        //...LOADING...
        LoadNextLevel();
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

    void ManageObstacles()
    {
        if (Ground == null)
            Ground = GameObject.Instantiate(PrefabGround, Vector3.zero, Quaternion.identity) as GameObject;

        switch (levelsCompleted)
        {
            case 0:
                Debug.Log("Load first level.");
                break;
            case 1:
                Debug.Log("Get harder.");
                break;
            case 2:
                Debug.Log("Get difficult.");
                break;
            case 3:
                Debug.Log("UNBEATABLE.");
                break;
            default:
                Debug.Log("Reload first level.");
                break;
        }



    }

    void LoadNextLevel()
    {
        ChooseGoalLocation();
        LoadObstacles();
        SetUpMainPulse();
        ManageObstacles();

        // If this is the first game - initialize our player
        if (Player == null)
            Player = GameObject.Instantiate(PrefabPlayer, Vector3.zero+Vector3.up*2, Quaternion.identity) as GameObject;
        if (GameLight == null)
            GameLight = GameObject.Instantiate(PrefabGameLight, PrefabGameLight.transform.position,  PrefabGameLight.transform.rotation) as GameObject;
        PlayerCam.SendMessage("Initialize");
    }

    void ChooseGoalLocation()
    {
        Vector3 pos = Vector3.zero;
        
        Debug.Log("Doing good.");
        switch (levelsCompleted)
        {
            case 0:
                Debug.Log("Load first level.");
                pos = PrefabGoal.transform.position;
                break;
            case 1:
                Debug.Log("Get harder.");
                break;
            case 2:
                Debug.Log("Get difficult.");
                break;
            case 3:
                break;
            default:
                Debug.Log("UNBEATABLE.");
                break;
        }

        if (Goal == null)
        {
            Goal = GameObject.Instantiate(PrefabGoal, pos, Quaternion.identity) as GameObject;
            Goal.GetComponentInChildren<Goal>().manager = this.gameObject;
        }
    }

    void LoadObstacles()
    {
        switch (levelsCompleted)
        {
            case 0:
                Debug.Log("Lucky you - no obstacles.");
                break;
            case 1:
                Debug.Log("Get harder.");
                break;
            case 2:
                Debug.Log("Get difficult.");
                break;            
            default:
                Debug.Log("UNBEATABLE.");
                break;
        }
    }

    void SetUpMainPulse()
    {
        if (MainPulse == null)
            MainPulse = GameObject.Instantiate(PrefabMainPulse, Vector3.zero, Quaternion.identity) as GameObject;

        //MainPulse.SendMessage("Reload");

    }

    #endregion

    #region Game Messages

    void OnReachedLevelGoal()
    {
        Debug.Log("Doing good.");
        levelsCompleted++;
        LoadNextLevel();
    }

    #endregion


}
