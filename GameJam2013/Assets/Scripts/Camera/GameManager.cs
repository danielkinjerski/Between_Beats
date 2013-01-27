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
    Transitioning = 4,
    Pause = 5
}
#endregion

public class GameManager : MonoBehaviour
{

    #region Variables

    /// <summary>
    /// Prefab References
    /// </summary>
    public GameObject PrefabPlayer, PrefabGoal, PrefabMainPulse, PrefabPlayerCam, PrefabGround, PrefabGameLight, PrefabLevelBlock;

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
	
			
	/// we store all our sounds in an array, then we can play them
	/// at any time, any where, by sending a message to gameManager
	/// to playOneShot and pass along the name of the audio we want
	public AudioClip[] audioClips;
	Dictionary<string,int> audioLookUp;

    float obstacleDistance = 30.0f;
    float goalDistance = 30.0f;
    float pulseSpeed = 1f;

    #endregion

    #region Constructors

    private GameManager()
    {
		
    }	

    void OnDestroy()
    {
        Object.Destroy(Player);
        Object.Destroy(Goal);
        Object.Destroy(MainPulse);
        Object.Destroy(PlayerCam);
        Object.Destroy(Ground);
        Object.Destroy(GameLight);
        scaleFormCamera.hud.OnExitGameCallback();

        levelsCompleted = 0;
        gameState = GameState.OpeningWindow;
    }

    #endregion

    #region Mono Inherit Functions

    /// <summary>
    /// We are first, so this is important
    /// </summary>
    void Awake () 
	{
        gameState = GameState.OpeningWindow;
        Random.seed = (int)Time.time;
        levelsCompleted = 0;
		menuOpen = true;
        PlayerCam = GameObject.Instantiate(PrefabPlayerCam, PrefabPlayerCam.transform.position, PrefabPlayerCam.transform.rotation) as GameObject;
        scaleFormCamera = PlayerCam.GetComponent<ScaleformCamera>();
				
		//build our sound look up table
		int counter = 0;
		audioLookUp = new Dictionary<string, int>();
		foreach(AudioClip clip in audioClips)
		{
			print (string.Format(">>>> Added {0} at index {1}", clip.ToString(), counter));
			audioLookUp.Add(clip.name, counter++);
		}
		
		
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
		//scaleFormCamera.hud.OpenHUD();
		menuOpen = false;

        //...LOADING...
        LoadNextLevel();
    }
    void TogglePause()
    {
		if(gameState == GameState.Pause)
		{
            gameState = GameState.Transitioning;
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

    public void NewLevelText()
    {
        scaleFormCamera.hud.OpenHUD();
    }

    #endregion

    #region Methods
	void playOneShot(string audioToPlay, float volume = 1)
	{
		print(string.Format("attempt to play the {0} audio clip", audioToPlay));
		if(PlayerCam != null)
		{
			if (audioLookUp.ContainsKey(audioToPlay))
			{
			    PlayerCam.audio.PlayOneShot(audioClips[audioLookUp[audioToPlay]], volume);
			}
			else
			{
				Debug.LogWarning(string.Format("No key found in audioLookUp matching {0}", audioToPlay));
			}
		}
	}

    void LoadNextLevel()
    {
        LoadObstacles();
        SetGoalLocation();
        SetUpMainPulse();
        NewLevelText();

        if (GameLight == null)
            GameLight = GameObject.Instantiate(PrefabGameLight, PrefabGameLight.transform.position,  PrefabGameLight.transform.rotation) as GameObject;

        gameState = GameState.PlayGame;
    }

    void SetGoalLocation()
    {
        Vector3 pos = Vector3.zero;
        
        Debug.Log("NEW GOAL.");
        switch (levelsCompleted)
        {
            case 0:
                break;
            case 1:
            case 2:
            case 3:
                goalDistance = 30;
                break;
            case 4:
            case 5:
            case 6:
                goalDistance = 50;
                break;
            default:
                goalDistance = 60;
                break;
        }

        if (Goal == null)
        {
            Goal = GameObject.Instantiate(PrefabGoal, PrefabGoal.transform.position, Quaternion.identity) as GameObject;
            Goal.GetComponentInChildren<Goal>().manager = this.gameObject;
            return;
        }


        Debug.Log("placing goal");
        for (int i = 0; i < obstacles.Count; i++)
        {
            PlaceGoal();
        }
    }

    void LoadObstacles()
    {
        if (Ground == null)
        {
            Ground = GameObject.Instantiate(PrefabGround, Vector3.zero, Quaternion.identity) as GameObject;
        }

        int numOfObstacles = 0;
        switch (levelsCompleted)
        {
            case 0:
                Debug.Log("Lucky you - no obstacles.");
                break;
            case 1:
            case 2:
            case 3:
                numOfObstacles = 5;
                obstacleDistance = 20;
                break;
            case 4:
            case 5:
            case 6:
                numOfObstacles = 10;
                obstacleDistance = 30;
                break;
            default:
                numOfObstacles = 15;
                obstacleDistance = 40;
                pulseSpeed += .02f;
                break;
        }

        if (obstacles.Count < numOfObstacles)
        {
            while (obstacles.Count < numOfObstacles)
            {
                obstacles.Add(GameObject.Instantiate(PrefabLevelBlock) as GameObject);
            }

        }
        else if (obstacles.Count > numOfObstacles)
        {
            int range = obstacles.Count - numOfObstacles;
            obstacles.RemoveRange(obstacles.Count - 1 - range, range);
        }


        for (int i = 0; i < obstacles.Count; i++)
        {
            PlaceBlock(i);
        }

    }

    void SetUpMainPulse()
    {
        if (MainPulse == null)
        {
            MainPulse = GameObject.Instantiate(PrefabMainPulse, PrefabMainPulse.transform.position, PrefabMainPulse.transform.rotation) as GameObject;
            return;
        }
    }

    #endregion

    #region Utilities

    void PlaceGoal()
    {
        Vector3 dir = new Vector3(Random.insideUnitCircle.x * goalDistance, 0, Random.insideUnitCircle.y * goalDistance);        
        dir.Normalize();
        Vector3 pos = dir * goalDistance;

        Goal.transform.position = pos;
    }

    

    private void PlaceBlock(int i)
    {
        Vector3 dir = new Vector3(Random.insideUnitCircle.x * obstacleDistance, 0, Random.insideUnitCircle.y * obstacleDistance);
        dir.Normalize();
        Vector3 pos = dir * obstacleDistance;

        obstacles[i].transform.position = pos;

        for (int j = 0; j < obstacles.Count; j++)
        {
            if(obstacles[i].collider.bounds.Intersects(obstacles[j].collider.bounds) && i != j) 
            {
                PlaceBlock(i);
            }
        }
    }

    


    #endregion

    #region Game Messages

    void OnReachedLevelGoal()
    {
        if (gameState == GameState.Transitioning)
            return;
        gameState = GameState.Transitioning;
        levelsCompleted++;
        PlayerCam.SendMessage("StopCam");
        PlayerCam.transform.position = PrefabPlayerCam.transform.position;
        LoadNextLevel();
        Debug.Log("RUINING LIVES");
        MainPulse.GetComponentInChildren<LoopScale>().Initialize(pulseSpeed);
        Player.SetActive(false);

    }

    public void LoadPlayer()
    {
        // If this is the first game - initialize our player
        if (Player == null)
        {
            Player = GameObject.Instantiate(PrefabPlayer, Vector3.zero + Vector3.up * 2, Quaternion.identity) as GameObject;
            // grab reference
            PlayerCam.SendMessage("Initialize");
        }
        // else respawn our guy
        else
        {
            if (!Player.activeInHierarchy)
            {
                Player.SetActive(true);
            }
            Player.transform.position = PrefabPlayer.transform.position;
            PlayerCam.SendMessage("GoCam");
        }

        MainPulse.GetComponentInChildren<LoopScale>().UnlockPulse();
        Goal.GetComponentInChildren<Goal>().enabled = true;
    }

    public void Death()
    {
        //Debug.Log("DEATH");
        //Debug.Log("DEATH"); Debug.Log("DEATH"); Debug.Log("DEATH"); Debug.Log("DEATH"); Debug.Log("DEATH"); Debug.Log("DEATH"); Debug.Log("DEATH"); Debug.Log("DEATH"); Debug.Log("DEATH");
    }

    #endregion


}
