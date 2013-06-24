using UnityEngine;
using System.Collections;

/// <summary>
/// InputHandler.cs is a bridge for handling input that can be redefined later
/// </summary>
public class InputHandler : MonoBehaviour
{
    #region Keyboard Booleans
    public static bool b1, b1Held;
    public static bool b2, b2Held;
    public static bool b3, b3Held;
    public static bool b4, b4Held;

    public static bool bL1, bL1Held;
    public static bool bL2, bL2Held;
    public static bool bR1, bR1Held;
    public static bool bR2, bR2Held;

    public static bool bSys;
    #endregion

    #region Joystick Booleans
    public static bool jbO, jbOHeld;
    public static bool jbU, jbUHeld;
    public static bool jbY, jbYHeld;
    public static bool jbA, jbAHeld;

    public static bool jbL1, jbL1Held;
    public static bool jbL2, jbL2Held;
    public static bool jbR1, jbR1Held;
    public static bool jbR2, jbR2Held;

    public static bool jbSys;
    #endregion

    public static Vector2 lAxis;
    public static Vector2 rAxis;
    public static Vector2 dPad;

    #region Keyboard KeyCodes
    public KeyCode button1, button1Held;
    public KeyCode button2, button2Held;
    public KeyCode button3, button3Held;
    public KeyCode button4, button4Held;

    public static float button1Timer = 0, button1TimeWait = 0.5f;
    public static float button2Timer = 0, button2TimeWait = 0.5f;
    public static float button3Timer = 0, button3TimeWait = 0.5f;
    public static float button4Timer = 0, button4TimeWait = 0.5f;

    public KeyCode buttonL1, buttonL1Held;
    public KeyCode buttonL2, buttonL2Held;
    public KeyCode buttonR1, buttonR1Held;
    public KeyCode buttonR2, buttonR2Held;

    public KeyCode buttonSys, buttonSysHeld;
    #endregion

    #region Joystick KeyCodes
    /// <summary>
    /// O Button
    /// </summary>
    public KeyCode joybutton1 = KeyCode.Joystick1Button0, joybutton1Held = KeyCode.Joystick1Button0;

    /// <summary>
    /// U Button
    /// </summary>
    public KeyCode joybutton2 = KeyCode.Joystick1Button3, joybutton2Held = KeyCode.Joystick1Button3;
    /// <summary>
    /// Y Button
    /// </summary>
    public KeyCode joybutton3 = KeyCode.Joystick1Button2, joybutton3Held = KeyCode.Joystick1Button2;
    /// <summary>
    /// A Button
    /// </summary>
    public KeyCode joybutton4 = KeyCode.Joystick1Button1, joybutton4Held = KeyCode.Joystick1Button1;

    /// <summary>
    /// Left Bumper
    /// </summary>
    public KeyCode joybuttonL1 = KeyCode.Joystick1Button4, joybuttonL1Held = KeyCode.Joystick1Button4;
    /// <summary>
    /// Right Bumper
    /// </summary>
    public KeyCode joybuttonL2 = KeyCode.Joystick1Button5, joybuttonL2Held = KeyCode.Joystick1Button5;
    /// <summary>
    /// Left Trigger
    /// </summary>
    public KeyCode joybuttonR1 = KeyCode.Joystick1Button6, joybuttonR1Held = KeyCode.Joystick1Button6;
    /// <summary>
    /// Right Trigger
    /// </summary>
    public KeyCode joybuttonR2 = KeyCode.Joystick1Button7, joybuttonR2Held = KeyCode.Joystick1Button7;
    #endregion

    // Update is called once per frame
    void Update()
    {
        ButtonEvents();

        #region Button 1
        if (Input.GetKey(button1) || Input.GetKey(joybutton1))
        {
            button1Timer += Time.deltaTime;
            if (button1Timer > button1TimeWait)
            {
                b1Held = true; jbOHeld = true; print("Button 1 Held");
            }
        }
        else { button1Timer = 0; b1Held = false; jbOHeld = false; }
        #endregion

        #region Button 2
        if (Input.GetKey(button2) || Input.GetKey(joybutton2))
        {
            button2Timer += Time.deltaTime;
            if (button2Timer > button2TimeWait)
            {
                b2Held = true; jbUHeld = true; print("Button 2 Held");
            }
        }
        else { button2Timer = 0; b2Held = false; jbUHeld = false; }
        #endregion

        #region Button 3
        if (Input.GetKey(button3) || Input.GetKey(joybutton3))
        {
            button3Timer += Time.deltaTime;
            if (button3Timer > button3TimeWait)
            {
                b3Held = true; jbYHeld = true; print("Button 3 Held");
            }
        }
        else { button3Timer = 0; b3Held = false; jbYHeld = false; }
        #endregion
    }

    KeyCode Key(string theString)
    {
        var newString = (KeyCode)System.Enum.Parse(typeof(KeyCode), theString);
        return newString;
    }

    /// <summary>
    /// Captures the input from Unity GetKey and sets bools to emulate the 
    /// Key Presses. The buttons configuration can be changed in the inspector
    /// </summary>
    void ButtonEvents()
    {
        #region Keyboard
        #region Button 1
        if (Input.GetKeyDown(button1))
        { b1 = true; Debug.Log(button1 + " pressed"); }
        else b1 = false;
        #endregion

        #region Button 2
        if (Input.GetKeyDown(button2))
        { b2 = true; Debug.Log(button2 + " pressed"); }
        else b2 = false;
        #endregion

        #region Button 3
        if (Input.GetKeyDown(button3))
        { b3 = true; Debug.Log(button3 + " pressed"); }
        else b3 = false;
        #endregion

        #region Button 4
        if (Input.GetKeyDown(button4))
        { b4 = true; Debug.Log(button4 + " pressed"); }
        else b4 = false;
        #endregion

        #region L1
        if (Input.GetKeyDown(buttonL1))
        { bL1 = true; Debug.Log(buttonL1 + " pressed"); }
        else bL1 = false;
        #endregion

        #region R2
        if (Input.GetKeyDown(buttonR1))
        { bR1 = true; Debug.Log(buttonR1 + " pressed"); }
        else bR1 = false;
        #endregion

        #region L2
        if (Input.GetKeyDown(buttonL2))
        { bL2 = true; Debug.Log(buttonL2 + " pressed"); }
        else bL2 = false;
        #endregion

        #region L2 Held
        if (Input.GetKey(buttonL2Held))
        { bL2Held = true; Debug.Log(buttonL2Held + " pressed"); }
        else bL2Held = false;
        #endregion

        #region R2
        if (Input.GetKeyDown(buttonR2))
        { bR2 = true; Debug.Log(buttonR2 + " pressed"); }
        else bR2 = false;
        #endregion

        #region R2 Held
        if (Input.GetKey(buttonR2Held))
        { bR2Held = true; Debug.Log(buttonR2Held + " pressed"); }
        else bR2Held = false;
        #endregion
        #endregion

        #region Joystick

        #region Button O
        if (Input.GetKeyDown(joybutton1))
        { jbO = true; Debug.Log(joybutton1 + " pressed"); }
        else jbO = false;
        #endregion

        #region Button U
        if (Input.GetKeyDown(joybutton2))
        { jbU = true; Debug.Log(joybutton2 + " pressed"); }
        else jbU = false;
        #endregion

        #region Button Y
        if (Input.GetKeyDown(joybutton3))
        { jbY = true; Debug.Log(joybutton3 + " pressed"); }
        else jbY = false;
        #endregion

        #region Button A
        if (Input.GetKeyDown(joybutton4))
        { jbA = true; Debug.Log(joybutton4 + " pressed"); }
        else jbA = false;
        #endregion

        #region L1
        if (Input.GetKeyDown(joybuttonL1))
        { jbL1 = true; Debug.Log(joybuttonL1 + " pressed"); }
        else jbL1 = false;
        #endregion

        #region R2
        if (Input.GetKeyDown(joybuttonR1))
        { jbR1 = true; Debug.Log(joybuttonR1 + " pressed"); }
        else jbR1 = false;
        #endregion

        #region L2
        if (Input.GetKeyDown(joybuttonL2))
        { jbL2 = true; Debug.Log(joybuttonL2 + " pressed"); }
        else jbL2 = false;
        #endregion

        #region L2 Held
        if (Input.GetKey(joybuttonL2Held))
        { jbL2Held = true; Debug.Log(joybuttonL2Held + " pressed"); }
        else jbL2Held = false;
        #endregion

        #region R2
        if (Input.GetKeyDown(joybuttonR2))
        { jbR2 = true; Debug.Log(joybuttonR2 + " pressed"); }
        else jbR2 = false;
        #endregion

        #region R2 Held
        if (Input.GetKey(joybuttonR2Held))
        { jbR2Held = true; Debug.Log(joybuttonR2Held + " pressed"); }
        else jbR2Held = false;
        #endregion
        #endregion
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(100, 100, 200, 50), "x: " + lAxis.x + " y: " + lAxis.y);
        //GUI.Label(new Rect(100, 115, 200, 50), "Button 1: " + button1);
        //GUI.Label(new Rect(100, 130, 200, 50), "Button 2: " + button2);
        //GUI.Label(new Rect(100, 145, 200, 50), "Button 3: " + button3);
        //GUI.Label(new Rect(100, 160, 200, 50), "Button 4: " + button4);

        //GUI.Label(new Rect(100, 175, 200, 50), "Button 2 Held: " + button2Held);
    }
}
