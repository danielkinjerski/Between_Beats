using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public GameObject manager;
    public bool reached = false;

	// Use this for initialization
	void OnTriggerEnter (Collider other) {

        if (other.tag != "Player")
            return;

        //if (this.enabled && GameManager.gameState == GameState.PlayGame)
        //{
        //    reached = false;
        //    Debug.Log("renale");
        //}


        if (this.enabled && manager != null && !reached && GameManager.gameState == GameState.PlayGame)
        {
            manager.SendMessage("OnReachedLevelGoal");
            Debug.Log("hit goal" + other.gameObject.name);
            reached = true;
            this.enabled = false;
        }
	}

    public void OnEnable()
    {
        reached = false;
    }
	
}
