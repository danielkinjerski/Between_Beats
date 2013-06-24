using UnityEngine;
using System.Collections.Generic;
using System;
using System.Timers;

/// <summary>
/// OUYA variant of NGUI's built-in UIButtonKeys script. Handles OUYA input, which doesn't go through the normal Unity input on the console.
/// </summary>
/// <remarks>
/// NGUI's UICamera.cs needs to have its "Application.platform == RuntimePlatform.Android" check under Awake() commented out to allow
/// buttons to be selected properly.
/// The input checks in here could be done more efficiently by modifying NGUI's UICamera.cs directly for all of the input,
/// but we want to keep this as non-intrusive as humanly possible.
/// </remarks>
[RequireComponent(typeof(Collider))]
[AddComponentMenu("NGUI/Interaction/OUYA Button Keys")]
public class OuyaUIButtonKeys : MonoBehaviour
{
    public bool startsSelected = false;
    public OuyaUIButtonKeys selectOnClick;
    public OuyaUIButtonKeys selectOnUp;
    public OuyaUIButtonKeys selectOnDown;
    public OuyaUIButtonKeys selectOnLeft;
    public OuyaUIButtonKeys selectOnRight;
    public OuyaSDK.KeyEnum activateButton = OuyaSDK.KeyEnum.BUTTON_O;

    private const double JoystickUpdateInterval = 200;
    private const float JoystickThreshold = 0.9f;
    private Timer joystickTimer;
    private bool joystickCanTrigger = true;

    void Start()
    {
        if (startsSelected && (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject)))
        {
            UICamera.selectedObject = gameObject;
        }
    }

    void Awake()
    {
        OuyaInputManager.OuyaButtonEvent.addButtonEventListener(HandleButtonEvents);
        joystickTimer = new Timer(JoystickUpdateInterval);
        joystickTimer.Elapsed += new ElapsedEventHandler(joystickTimer_Elapsed);
    }

    void joystickTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        joystickCanTrigger = true;
        joystickTimer.Stop();
    }

    private void StartJoystickResetTimer()
    {
        joystickCanTrigger = false;
        joystickTimer.Start();
    }

    void OnDestroy()
    {
        OuyaInputManager.OuyaButtonEvent.removeButtonEventListener(HandleButtonEvents);
    }

    private void HandleButtonEvents(OuyaSDK.OuyaPlayer p, OuyaSDK.KeyEnum b, OuyaSDK.InputAction bs)
    {
        if (enabled && NGUITools.GetActive(gameObject))
        {
            // Ignoring P to allow any player to use the menu
            if (bs.Equals(OuyaSDK.InputAction.KeyDown))
            {
                switch (b)
                {
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_UP:
                        OnKey(KeyCode.UpArrow);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_DOWN:
                        OnKey(KeyCode.DownArrow);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_LEFT:
                        OnKey(KeyCode.LeftArrow);
                        break;
                    case OuyaSDK.KeyEnum.BUTTON_DPAD_RIGHT:
                        OnKey(KeyCode.RightArrow);
                        break;
                }
            }

            if (b == activateButton && gameObject == UICamera.selectedObject)
            {
                SendMessage("OnPress", bs.Equals(OuyaSDK.InputAction.KeyDown), SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void Update()
    {
        if (joystickCanTrigger && enabled && NGUITools.GetActive(gameObject))
        {
            float greatestX = 0.0f;
            float greatestY = 0.0f;
            float leastX = 0.0f;
            float leastY = 0.0f;

            //foreach (OuyaSDK.OuyaPlayer player in Enum.GetValues(typeof(OuyaSDK.OuyaPlayer))) // TODO: Use once input lag is fixed
            for (int i = 1; i <= 4; ++i) // HACK: check a hard-coded number of players
            {
                OuyaSDK.OuyaPlayer player = (OuyaSDK.OuyaPlayer)i;

                float joystickX = OuyaInputManager.GetAxis("LX", player);
                float joystickY = OuyaInputManager.GetAxis("LY", player);

                greatestX = Math.Max(joystickX, greatestX);
                greatestY = Math.Max(joystickY, greatestY);
                leastX = Math.Min(joystickX, leastX);
                leastY = Math.Min(joystickY, leastY);
            }

            if (greatestX > JoystickThreshold)
            {
                StartJoystickResetTimer();
                OnKey(KeyCode.RightArrow);
            }
            if (greatestY > JoystickThreshold)
            {
                StartJoystickResetTimer();
                OnKey(KeyCode.UpArrow);
            }
            if (leastX < -JoystickThreshold)
            {
                StartJoystickResetTimer();
                OnKey(KeyCode.LeftArrow);
            }
            if (leastY < -JoystickThreshold)
            {
                StartJoystickResetTimer();
                OnKey(KeyCode.DownArrow);
            }
        }


    }

    void OnKey(KeyCode key)
    {
        if (enabled && NGUITools.GetActive(gameObject))
        {
            switch (key)
            {
                case KeyCode.LeftArrow:
                    if (selectOnLeft != null) UICamera.selectedObject = selectOnLeft.gameObject;
                    break;
                case KeyCode.RightArrow:
                    if (selectOnRight != null) UICamera.selectedObject = selectOnRight.gameObject;
                    break;
                case KeyCode.UpArrow:
                    if (selectOnUp != null) UICamera.selectedObject = selectOnUp.gameObject;
                    break;
                case KeyCode.DownArrow:
                    if (selectOnDown != null) UICamera.selectedObject = selectOnDown.gameObject;
                    break;
                case KeyCode.Tab:
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        if (selectOnLeft != null) UICamera.selectedObject = selectOnLeft.gameObject;
                        else if (selectOnUp != null) UICamera.selectedObject = selectOnUp.gameObject;
                        else if (selectOnDown != null) UICamera.selectedObject = selectOnDown.gameObject;
                        else if (selectOnRight != null) UICamera.selectedObject = selectOnRight.gameObject;
                    }
                    else
                    {
                        if (selectOnRight != null) UICamera.selectedObject = selectOnRight.gameObject;
                        else if (selectOnDown != null) UICamera.selectedObject = selectOnDown.gameObject;
                        else if (selectOnUp != null) UICamera.selectedObject = selectOnUp.gameObject;
                        else if (selectOnLeft != null) UICamera.selectedObject = selectOnLeft.gameObject;
                    }
                    break;
            }
        }
    }

    void OnClick()
    {
        if (enabled && selectOnClick != null)
        {
            UICamera.selectedObject = selectOnClick.gameObject;
        }
    }
}