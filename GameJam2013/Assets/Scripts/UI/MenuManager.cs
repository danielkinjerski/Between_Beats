using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {
    public List<GameObject> Menus;
    GameState curstate;
    public TweenColor text, background;
    public UILabel tut1, tut2;


    void Start()
    {
        foreach (GameObject menu in Menus)
            menu.SetActive(false);
       // Menus[0].SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {

        if (curstate != GameManager.gameState)
            ActivateMenu();
        curstate = GameManager.gameState;
	
	}

    void ActivateMenu()
    {
        switch (GameManager.gameState)
        {

            case GameState.OpeningWindow:
                foreach (GameObject menu in Menus)
                    menu.SetActive(false);
                Menus[0].SetActive(true);
                break;
            case GameState.Tutorial:
                foreach (GameObject menu in Menus)
                    menu.SetActive(false);
                Menus[1].SetActive(true);
                break;
            case GameState.PlayGame:
                foreach (GameObject menu in Menus)
                    menu.SetActive(false);
                Menus[2].SetActive(true);
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                background.color = new Color(background.color.r, background.color.g, background.color.b, 1);
                text.enabled = true;
                background.enabled = true;
                break;
            case GameState.Pause:
                foreach (GameObject menu in Menus)
                    menu.SetActive(false);
                Menus[3].SetActive(true);
                break;
            case GameState.GameOver:
                foreach (GameObject menu in Menus)
                    menu.SetActive(false);
                Menus[4].SetActive(true);
                break;
            case GameState.GameWin:
                foreach (GameObject menu in Menus)
                    menu.SetActive(false);
                Menus[5].SetActive(true);
                break;
            default:
                break;

        }
    }

    void TurnOffLevelText()
    {
        Menus[2].SetActive(false);
    }   
    public void TutorialOneFinished()
    {
        Invoke("LoadText", 2);
    }
    void LoadText()
    {
        tut1.gameObject.SetActive(false);
        tut2.gameObject.SetActive(true);
    }

    public void TutorialTwoFinished()
    {
        GameObject.Find("GameManager").SendMessage("SkipTutorial");
    }
}
