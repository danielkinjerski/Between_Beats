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
public  enum CurrentPlayMode
{
    Black = 1,
    White = 2,
    Grey = 3
}
#endregion
public class GameManager : MonoBehaviour
{

    #region Variables

    public GameObject OpeningWindow, GameOverWindow, SelectionWindow, TutorialWindow,
                    BlackWorld, WhiteWorld,
                    BlackCam, WhiteCam, MainCam,
                    Facebook, fbbutton, fbsuccess, fbpost,
                    Character,
                    CharBRender, CharWRender;
    UILabel fb;
    public UILabel deathLbl, timeLbl;
    public Material BlackMat, WhiteMat;
    public static GameState gameState = GameState.OpeningWindow;
    public static CurrentPlayMode currentPlayMode = CurrentPlayMode.Black;
    private bool toggle;
    public bool cheats;
    private int deaths;
    private float time, maxTime;

    #endregion

    #region Mono Inherit Functions

    void Awake () {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1f);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1f);
        gameState = GameState.OpeningWindow;
        OpeningWindow.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
        SelectionWindow.SetActiveRecursively(false);
        TutorialWindow.SetActiveRecursively(false);
        

        BlackCam.camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        WhiteCam.camera.rect = new Rect(0, 0, 0.5f, 1);

        MainCam.camera.rect = new Rect(0, 0, 1, 1);
        MainCam.active = Facebook.active = true;
        BlackCam.active = WhiteCam.active = fbsuccess.active = false;
        fb = fbsuccess.GetComponent<UILabel>();

        deathLbl.text = "You died: @ time(s)!";
        timeLbl.text = "Your max time: @";
	}
	
	void Update ()
    {

        #region Fade in/out
        if (toggle)
        {
            if (GameManager.currentPlayMode == CurrentPlayMode.Black)
            {
                if (Toggle(ref WhiteMat, true)&&Toggle(ref BlackMat, false))
                {
                    toggle = false;
                    ActivateBlackMode(false);
                    GameManager.currentPlayMode = CurrentPlayMode.White;
                }
            }
            else if (GameManager.currentPlayMode == CurrentPlayMode.White)
            {
                if(Toggle(ref BlackMat, true)&& Toggle(ref WhiteMat, false))
                {
                    toggle = false;
                    ActivateWhiteMode(false);
                    GameManager.currentPlayMode = CurrentPlayMode.Black;
                }
            }
        }
        #endregion
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
    void Switch()
    {
        if (currentPlayMode == CurrentPlayMode.Grey)
            return;

        toggle = true;
        switch (GameManager.currentPlayMode)
        {
            case CurrentPlayMode.Black:
                ActivateWhiteMode(true);
                WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 0);
                break;
            case CurrentPlayMode.White:
                ActivateBlackMode(true);
                BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 0);
                break;
        }
    }
    void Play()
    {
        OpeningWindow.SetActiveRecursively( false );
        SelectionWindow.SetActiveRecursively(true);
    }
    void Gray() 
    {
        WhiteCam.active = BlackCam.active = true;
        SelectionWindow.SetActiveRecursively(false);
        gameState = GameState.PlayGame;
        currentPlayMode = CurrentPlayMode.Grey;
        time = Time.timeSinceLevelLoad;
        CharWRender.active = CharBRender.active = true;

        TutorialWindow.SetActiveRecursively(true);
    }
    void Black() 
    {
        WhiteCam.active = BlackCam.active = false;
        MainCam.active = true;
        ActivateWhiteMode(false);
        ActivateBlackMode(true);
        SelectionWindow.SetActiveRecursively(false);
        gameState = GameState.PlayGame;
        GameManager.currentPlayMode = CurrentPlayMode.Black;
        time = Time.timeSinceLevelLoad;
        CharWRender.active = false;
        CharBRender.active = true;
        TutorialWindow.SetActiveRecursively(true);
    }
    void White() 
    {
        WhiteCam.active = BlackCam.active = false;
        MainCam.active = true;
        ActivateBlackMode(false);
        ActivateWhiteMode(true);
        SelectionWindow.SetActiveRecursively(false);
        gameState = GameState.PlayGame;
        GameManager.currentPlayMode = CurrentPlayMode.White;
        time = Time.timeSinceLevelLoad;
        CharWRender.active = true;
        CharBRender.active = false;
        TutorialWindow.SetActiveRecursively(true);
    }
    void FaceBook()
    {
        Facebook.SendMessage("GetToken");
    }
    void PostResults()
    {
        fbpost.SetActiveRecursively(false);
        float timer = Time.timeSinceLevelLoad - time;
        if (timer > maxTime)
            maxTime = timer;
        string minutes = Mathf.Floor(maxTime / 60).ToString("00");
        string seconds = (maxTime % 60).ToString("00");
        Facebook.GetComponent<Facebook>().Publish("I died "+deaths+" time(s) and lasted for  a maximum of " + minutes + " minute(s) " + seconds + " second(s)!"  );
    }
    void SuccessFacebookLink()
    {
        if (fb.text != "Success!")
        {
            fb.color = Color.green;
            fb.text = "Success!";
        }
    }
    void ProcessFacebookLink()
    {
        if (fbbutton.active && fb.text != "Processing... Please Wait")
        {
            fbsuccess.active = true;
            fb.color = Color.yellow;
            fb.text = "Processing... Please Wait";
        }
        fbbutton.SetActiveRecursively(false);
    }
    void FailedFacebookLink()
    {
        if (fb.text != "There seems to have been a problem\nWe are having issues with the web version.\nSorry about that, feel free to play!")
        {
            fb.color = Color.red;
            fb.text = "There seems to have been a problem\nWe are having issues with the web version.\nSorry about that, feel free to play!";
        }
    }
    void Pause()
    {
        gameState = GameState.Pause;
    }
    void GameOver()
    {
        deaths++;
        float timer = Time.timeSinceLevelLoad - time;
        if (timer > maxTime)
            maxTime = timer;
        string minutes = Mathf.Floor(maxTime / 60).ToString("00");
        string seconds = (maxTime % 60).ToString("00");
        gameState = GameState.GameOver;
        OpeningWindow.SetActiveRecursively(false);
        GameOverWindow.SetActiveRecursively(true);
        if (fb.color == Color.red)
            fbpost.SetActiveRecursively(false);
        deathLbl.text = "You died: @ time(s)!";
        timeLbl.text = "Your max time: @";
        deathLbl.text = deathLbl.text.Replace("@", deaths.ToString());
        timeLbl.text = timeLbl.text.Replace("@", minutes + " minutes " + seconds + " seconds");
        TutorialWindow.SetActiveRecursively(false);
    }
    void Replay()
    {
        gameState = GameState.PlayGame;
        time = Time.timeSinceLevelLoad;
        Character.SetActiveRecursively(true);
        GameOverWindow.SetActiveRecursively(false);
        TutorialWindow.SetActiveRecursively(true);
        if(currentPlayMode == CurrentPlayMode.Black)
            CharWRender.active = false;
        if (currentPlayMode == CurrentPlayMode.White)
            CharBRender.active = false;
    }
    #endregion

    #region Utilities

    bool Toggle(ref Material mat, bool pulse)
    {
        if ((mat.color.a <= 0 && !pulse) || (mat.color.a >= 1 && pulse))
            return true;

        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, (pulse) ? (mat.color.a + .1f) : (mat.color.a - .1f));

        return false;
    }

    void ActivateBlackMode(bool active)
    {
        BlackWorld.SetActiveRecursively(active);
        if (active)
        {
            MainCam.camera.backgroundColor = new Color(.85f, .85f, .85f);
            CharBRender.active = true;
            CharWRender.active = false;
        }
        
    }
    void ActivateWhiteMode(bool active)
    {
        WhiteWorld.SetActiveRecursively(active);
        if (active)
        {
            MainCam.camera.backgroundColor = new Color(.29f, .29f, .29f);
            CharBRender.active = false;
            CharWRender.active = true;
        }
    }

    void OnApplicationQuit()
    {
        BlackMat.color = new Color(BlackMat.color.r, BlackMat.color.g, BlackMat.color.b, 1);
        WhiteMat.color = new Color(WhiteMat.color.r, WhiteMat.color.g, WhiteMat.color.b, 1);
    }

    #endregion


}
