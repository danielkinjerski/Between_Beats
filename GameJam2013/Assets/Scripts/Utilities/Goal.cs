using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public GameObject manager;

	// Use this for initialization
	void OnTriggerEnter () {
        if(manager!= null)
            manager.SendMessage("OnReachedLevelGoal");
	}
	
}
