using UnityEngine;
using System.Collections;

public class SphereCollision : MonoBehaviour {

	// Use this for initialization
	void OnTriggerExit (Collider other) {
        if (other.tag == "Player")
        {
            Debug.Log("died");
        }
	}
	
}
