using UnityEngine;
using System.Collections;

public class PushableObject : MonoBehaviour {

	//public enum ControlType { OpenDoor, DestroyObstacle, ActivateObject }
    //public ControlType controlType;

    public GameObject obj;
    public string objName;
	public bool canPush = true;
	
	public void Push(Vector3 dir)
	{
		//transform.Translate((dir * 2) * Time.deltaTime);
		if (canPush)
		{
			var direction = new Vector3(dir.x,0,dir.z);
			rigidbody.AddForce(direction * 5);
		}
	}
}
