using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject _target;
	public float followDistance;
	public float time;
	
	Vector3 target; 
	
	public bool lockX;
	public bool lockY;
	public bool lockZ;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		target = _target.transform.position; 
		
		transform.position = new Vector3(
			(lockX) ? transform.position.x : Mathf.Lerp(transform.position.x, target.x - followDistance, time),
			(lockY) ? transform.position.y : Mathf.Lerp(transform.position.y, target.y - followDistance, time),
			(lockZ) ? transform.position.z : Mathf.Lerp(transform.position.z, target.z - followDistance, time));
	}
}
