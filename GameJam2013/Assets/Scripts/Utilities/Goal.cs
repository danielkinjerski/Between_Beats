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


        if (manager != null && !reached && GameManager.gameState == GameState.PlayGame)
        {
            manager.GetComponent<GameManager>().OnReachedLevelGoal();
            Debug.Log("hit goal" + other.gameObject.name);
            reached = true;
            //Destroy(this);
            //this.enabled = false;
        }
	}

    public void Reset()
    {
        reached = false;
    }
	
}
