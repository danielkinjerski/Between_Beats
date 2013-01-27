using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public GameObject manager;
    public bool reached = false;

	// Use this for initialization
	void OnTriggerEnter () {
        if (manager != null && !reached)
        {
            manager.SendMessage("OnReachedLevelGoal");
            reached = true;
        }
	}
	
}
