using UnityEngine;
using System.Collections;

public class LoopScale : MonoBehaviour {


    #region Variables

    /// <summary>
    /// Reference to the transfrom object
    /// </summary>
    Transform trans;

    /// <summary>
    /// Average value, offset from zero. 
    /// This is the point at which the object will float around
    /// </summary>
    public float z = 1;

    /// <summary>
    /// Amplitude, this is how high and low the curve will go
    /// </summary>
    public float a = 1;

    /// <summary>
    /// Angular Frequency, this is how fast it will traverse the curve
    /// </summary>
    public float b = 3f;

    /// <summary>
    /// Phase Angle, check back for more details
    /// </summary>
    public float c = .5f;

    /// <summary>
    /// this is how far the curve has been traversed
    /// </summary>
    private float x = 0;

    float period = 0;

    #endregion

	// Use this for initialization
	void Start () {
        trans = this.transform;
		
		
		float y = -z/a;
		y = 1/Mathf.Sin(y);		
        x = ((y * 100) / b) - c;
	
       // period = a * Mathf.Sin(b * x + c) + z;

	}
	
	

    // Update is called once per frame
    void LateUpdate()
    {
        //y = z+ a * sin(bx + c)
        transform.localScale = Vector3.one * (z + a * Mathf.Sin(b * (x/100) + c));
		x = (x + 1);
      
    }


    public void SetTarget()
    {

    }
}
