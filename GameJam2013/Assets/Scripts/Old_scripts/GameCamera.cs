/* Gale Camera
 * Created by SrBilyon Harris - 2012
 * Contributions by Daniel Kinjerski
 * 
 * This script controls the multiple systems that are in the camera system
 */
using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{

	public GameObject target;
	public GameObject closestWaypoint;
	public GameObject playerTarget;
	public Quaternion rotation;
	public Vector3 position;
	public GameObject[] waypoints;
	public float distanceFromTarget;
	public float newDistance;
	public float zoomSpeed = 0.01f;
	public Vector3 vDisplacement;
	public float lerpSpeed = 0.02f;
	
	float currentDistance;
	float largestDistance;
	public Camera theCamera;
	public float height = 5.0f;
	Vector3 avgDistance;
	public float distance = 0.0f;                    // Default Distance 
	public float speed = 1;
	public float offset;
	
	public bool versusMode = false;


	// Use this for initialization
	void Start ()
	{
		//playerTarget = GameObject.Find("Player");
		//target = GameObject.Find("Player");
	}
	
	void Update ()
	{
		if (Input.GetKey (KeyCode.Minus)) {
			distanceFromTarget -= .1f; 
		}
		if (Input.GetKey (KeyCode.Equals)) {
			distanceFromTarget += .1f; 
		}
		
		
		if (Input.GetKey (KeyCode.KeypadMinus)) {
			distanceFromTarget -= .1f; 
		}
		if (Input.GetKey (KeyCode.KeypadPlus)) {
			distanceFromTarget += .1f; 
		}
	}
	// Update is called once per frame
	void LateUpdate ()
	{
		//if (!target)
		//	return;
        
	
		FindClosestWaypoint ();
		target = closestWaypoint;

		position = target.transform.position;

		//Finally Set rotation and position of camera 
		if (target.transform.localRotation.eulerAngles.x > 180) {
			float tempX = target.transform.localRotation.eulerAngles.x;
			tempX -= 360;
		} else if (target.transform.localRotation.eulerAngles.y > 180) {
			float tempY = target.transform.localRotation.eulerAngles.y;
			tempY -= 360;
		} else if (target.transform.localRotation.eulerAngles.z > 180) {
			float tempZ = target.transform.localRotation.eulerAngles.z;
			tempZ -= 360;
		}
		
		zoomSpeed = target.GetComponent<CamWaypoint> ().zoomSpeed;
		lerpSpeed = target.GetComponent<CamWaypoint> ().lerpSpeed;
		newDistance = target.GetComponent<CamWaypoint> ().newDistance;

		transform.localRotation = Quaternion.Lerp (transform.localRotation, target.transform.localRotation, lerpSpeed / 100);
		transform.position = playerTarget.transform.position - (transform.rotation * Vector3.forward * distanceFromTarget + vDisplacement);
		
		distanceFromTarget = Mathf.Lerp (distanceFromTarget, newDistance, zoomSpeed);

	}
	/// <summary>
	/// Finds the closest waypoint.
	/// </summary>
	/// <returns>
	/// The closest waypoint.
	/// </returns>
	public GameObject FindClosestWaypoint ()
	{
		// Find all game objects with tag "Waypoint"
		waypoints = GameObject.FindGameObjectsWithTag ("Waypoint"); 
    	
		var distance = Mathf.Infinity; 
		//var position = transform.position; 
    	
		// Iterate through them and find the closest one
		foreach (GameObject go in waypoints) { 
			var diff = (go.transform.position - playerTarget.transform.position);
			var curDistance = diff.sqrMagnitude; 
    			
			if (curDistance < distance) { 
				closestWaypoint = go; 
				distance = curDistance; 
			} 
		} 
		return closestWaypoint; 
	}
	
	
	
	////////////////////////////////////////////////////////////////////

	//IMPORTANT! Tag ALL players with "Player" so they are recognized.//

	////////////////////////////////////////////////////////////////////

//========================================

	float returnLargestDifference ()
	{
		currentDistance = 0.0f;
		largestDistance = 0.0f;

		currentDistance = Vector3.Distance (playerTarget.transform.position, target.transform.position);
		
		if (currentDistance > largestDistance) {
			largestDistance = currentDistance;
		}
		return largestDistance;
	}
}
