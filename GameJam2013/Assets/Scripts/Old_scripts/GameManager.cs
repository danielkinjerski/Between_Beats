using UnityEngine;
using System.Collections;

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
    private bool toggle;
    public bool cheats;

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

    void Awake () {
        //gameState = GameState.OpeningWindow;
        Debug.Log("Direct to play, just change to OpeningWindow.");
        gameState = GameState.PlayGame;

        Debug.Log("Disable all menus");
	}

    #endregion

    #region UI Events
    void BackToMain()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    void Quit()
    {
        Application.Quit();
    }
    void Play()
    {
        gameState = GameState.PlayGame;
    }
    void Pause()
    {
        gameState = GameState.Pause;
    }
    void GameOver()
    {
        gameState = GameState.GameOver;
    }
    #endregion

    #region Utilities

   

    #endregion


}
