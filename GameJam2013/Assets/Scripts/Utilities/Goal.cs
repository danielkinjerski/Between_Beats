using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public GameObject manager;
    public bool reached = false;

	// Use this for initialization
	void OnTriggerEnter () {
        if (manager != null && !reached && GameManager.gameState == GameState.PlayGame)
        {
            manager.SendMessage("OnReachedLevelGoal");
            this.enabled = false;
            reached = true;
        }
	}

    public void Initialize()
    {
        reached = false;
        Debug.Log("INIT");
    }
	
}
