using UnityEngine;
using System.Collections;

public enum CatchPulse
{
    WaitingForPulse,
    LoadingCharacter,
    Pulse

}

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
    public float x = 0;

    public float applySpeed = 1;
    public CatchPulse catchPulse = CatchPulse.Pulse;

    public bool MainPulse = false;

    #endregion

	// Use this for initialization
	void Start () {
        trans = this.transform;
        catchPulse = CatchPulse.WaitingForPulse;
        x = 0;
        b = 3;
    }
	
	

    // Update is called once per frame
    void LateUpdate()
    {
        if (catchPulse == CatchPulse.LoadingCharacter)
            return;

        //y = z+ a * sin(bx + c)
        transform.localScale = Vector3.one * (z + a * Mathf.Sin(b * (x/100) + c));
		x = (x + 1);


        if (MainPulse && catchPulse == CatchPulse.WaitingForPulse && transform.localScale.magnitude < 1)
        {
            catchPulse = CatchPulse.LoadingCharacter;
            GameObject.FindGameObjectWithTag("GameManager").SendMessage("LoadPlayer");
            b = applySpeed;
            x = 420;
            Debug.Log("load");
        }
        if (MainPulse && catchPulse == CatchPulse.Pulse && transform.localScale.magnitude > a + z - .01f)
        {
            //GameObject.FindGameObjectWithTag("GameManager").SendMessage("Death");
        }
    }

    public void UnlockPulse()
    {
        catchPulse = CatchPulse.Pulse;
    }

    public void Initialize(float _spd)
    {
        Debug.Log("bad");
        applySpeed = _spd;
        Start();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("game over");
        }
    }
}
