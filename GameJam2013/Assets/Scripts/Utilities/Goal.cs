using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public GameObject manager;
    public bool reached = false;

	// Use this for initialization
	void OnTriggerEnter () {

        if (GameManager.gameState == GameState.PlayGame)
        {
            reached = false;

        }


        if (manager != null && !reached && GameManager.gameState == GameState.PlayGame)
        {
            manager.SendMessage("OnReachedLevelGoal");
            reached = true;
            this.enabled = false;
        }
	}

    public void OnEnable()
    {
        reached = false;
        Debug.Log("INIT");
    }
	
}
