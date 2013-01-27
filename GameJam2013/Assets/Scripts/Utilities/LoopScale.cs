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

    public bool reachedCrest = false;

    #endregion

	// Use this for initialization
	void Start () {
        trans = this.transform;
        catchPulse = CatchPulse.WaitingForPulse;
        reachedCrest = false;
        x = 0;
        if (GameManager.gameState == GameState.Tutorial)
        {
            b = 3;
        }
    }
	
	

    // Update is called once per frame
    void LateUpdate()
    {
        if (catchPulse == CatchPulse.LoadingCharacter)
            return;

        //y = z+ a * sin(bx + c)
        trans.localScale = Vector3.one * (z + a * Mathf.Sin(b * (x / 100) + c));
		x = (x + 1);


        if (GameManager.gameState == GameState.PlayGame && MainPulse && catchPulse == CatchPulse.WaitingForPulse && trans.localScale.magnitude < 1)
        {
            catchPulse = CatchPulse.LoadingCharacter;
            Debug.Log("SENDING LOAD PLAYER");
            GameObject.FindGameObjectWithTag("GameManager").SendMessage("LoadPlayer");
            b = applySpeed;
            x = 420;
            reachedCrest = false;
            
        }
        if (catchPulse == CatchPulse.Pulse && !reachedCrest && trans.localScale.magnitude > a + z)
        {
            print(trans.localScale.magnitude);
            reachedCrest = true;
        }

        if (GameManager.gameState == GameState.Tutorial)
        {
            b = Mathf.Lerp(b, 1, .001f);
        }
    }

    public void UnlockPulse()
    {
        catchPulse = CatchPulse.Pulse;
    }

    public void Initialize(float _spd)
    {
        applySpeed = _spd;
        Start();
    }
}
