/**********************************************************************

Filename    :	SFGamepad.cs
Content     :	Logic for processing gamepad events. Feel free to modify to suit your requirements.
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using Scaleform;

public class SFGamepad
{

    protected struct Gamepad
    {
        public UnityButtonToSFKey[] Keymap;
        public Joystick[] Joysticks;
        public int controllerIndex;
    };

    protected struct Joystick
    {
        public String HorizontalAxisName;
        public String VerticalAxisName;
        public double HorizontalAxisThresholdD;
        public double VerticalAxisThresholdD;
        public bool RightKeyPressed, LeftKeyPressed, UpKeyPressed, DownKeyPressed;
        public float RightTimerF, LeftTimerF, UpTimerF, DownTimerF;
    };

    protected struct UnityButtonToSFKey
	{
		public UnityButtonToSFKey(String unityButton, SFKey.Code sfKeyCode)
		{
			UnityButton = unityButton;
			SFKeyCode	= sfKeyCode;
		}

		public String UnityButton;
		public SFKey.Code SFKeyCode;
	};

    protected Gamepad[] Gamepads;

	protected SFManager SFMgr;
	protected double HorizontalAxisThresholdD = 0.5;
	protected double VerticalAxisThresholdD = 0.5;
    protected float RepeatTimerThresholdF = .5f;

    public SFGamepad() { }

    public SFGamepad(SFManager sfMgr)
    {
        SFMgr = sfMgr;
    }

    public void Init()
    {
        Gamepads = new Gamepad[1];

      
            Gamepads[0] = new Gamepad();

            Gamepads[0].Keymap = new UnityButtonToSFKey[4];
            Gamepads[0].Keymap[0] = new UnityButtonToSFKey("Fire1", SFKey.Code.A);
            Gamepads[0].Keymap[1] = new UnityButtonToSFKey("Fire2", SFKey.Code.B);
            Gamepads[0].Keymap[2] = new UnityButtonToSFKey("Fire3", SFKey.Code.X);
            Gamepads[0].Keymap[3] = new UnityButtonToSFKey("Jump", SFKey.Code.Y);
            Gamepads[0].controllerIndex = 0;

            Gamepads[0].Joysticks = new Joystick[1];
            Gamepads[0].Joysticks[0] = new Joystick();
            Gamepads[0].Joysticks[0].HorizontalAxisName = "Horizontal";
            Gamepads[0].Joysticks[0].VerticalAxisName = "Vertical";
            Gamepads[0].Joysticks[0].HorizontalAxisThresholdD = this.HorizontalAxisThresholdD;
            Gamepads[0].Joysticks[0].VerticalAxisThresholdD = this.VerticalAxisThresholdD;
			/*
            Gamepads[1] = new Gamepad();

            Gamepads[1].Keymap = new UnityButtonToSFKey[4];
            Gamepads[1].Keymap[0] = new UnityButtonToSFKey("Fire1 2", SFKey.Code.A);
            Gamepads[1].Keymap[1] = new UnityButtonToSFKey("Fire2 2", SFKey.Code.B);
            Gamepads[1].Keymap[2] = new UnityButtonToSFKey("Fire3 2", SFKey.Code.X);
            Gamepads[1].Keymap[3] = new UnityButtonToSFKey("Jump 2", SFKey.Code.Y);

            Gamepads[1].Joysticks = new Joystick[1];
            Gamepads[1].Joysticks[0] = new Joystick();
            Gamepads[1].Joysticks[0].HorizontalAxisName = "Horizontal 2";
            Gamepads[1].Joysticks[0].VerticalAxisName = "Vertical 2";
            Gamepads[1].Joysticks[0].HorizontalAxisThresholdD = this.HorizontalAxisThresholdD;
            Gamepads[1].Joysticks[0].VerticalAxisThresholdD = this.VerticalAxisThresholdD;
            Gamepads[1].controllerIndex = 1;
			*/
    }

	public void Update()
	{
        for (int a = 0; a < Gamepads.Length; a++)
        {
            // Let's process gamepad keys first. 
            int UnityButtonToSFKey_size = Gamepads[a].Keymap.Length;
            for (int i = 0; i < UnityButtonToSFKey_size; i++)
            {
                if (Input.GetButtonDown(Gamepads[a].Keymap[i].UnityButton))
                {
                    SFMgr.HandleKeyDownEvent(Gamepads[a].Keymap[i].SFKeyCode, 0, Gamepads[a].controllerIndex);
                }
                if (Input.GetButtonUp(Gamepads[a].Keymap[i].UnityButton))
                {
                    SFMgr.HandleKeyUpEvent(Gamepads[a].Keymap[i].SFKeyCode, 0, Gamepads[a].controllerIndex);
                }
            }

            // Let's process axis now.
            for (int b = 0; b < Gamepads[a].Joysticks.Length;b++ )
            {
                double horizontal = Input.GetAxis(Gamepads[a].Joysticks[b].HorizontalAxisName);
                double vertical = Input.GetAxis(Gamepads[a].Joysticks[b].VerticalAxisName);

                bool currR = horizontal > Gamepads[a].Joysticks[b].HorizontalAxisThresholdD;
                bool currL = horizontal < -Gamepads[a].Joysticks[b].HorizontalAxisThresholdD;
                bool currD = vertical < -Gamepads[a].Joysticks[b].VerticalAxisThresholdD;
                bool currU = vertical > Gamepads[a].Joysticks[b].VerticalAxisThresholdD;

                if (currR)
                {
                    Gamepads[a].Joysticks[b].RightTimerF += UnityEngine.Time.deltaTime;

                    if (!Gamepads[a].Joysticks[b].RightKeyPressed || Gamepads[a].Joysticks[b].RightTimerF > RepeatTimerThresholdF)
                    {
                        Gamepads[a].Joysticks[b].RightTimerF = 0f;
                        SFMgr.HandleKeyDownEvent(SFKey.Code.Right, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].RightKeyPressed = true;
                    }
                }
                else
                {
                    if (Gamepads[a].Joysticks[b].RightKeyPressed)
                    {
                        SFMgr.HandleKeyUpEvent(SFKey.Code.Right, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].RightKeyPressed = false;
                    }
                }

                if (currL)
                {
                    Gamepads[a].Joysticks[b].LeftTimerF += UnityEngine.Time.deltaTime;

                    if (!Gamepads[a].Joysticks[b].LeftKeyPressed || Gamepads[a].Joysticks[b].LeftTimerF > RepeatTimerThresholdF)
                    {
                        Gamepads[a].Joysticks[b].LeftTimerF = 0f;
                        SFMgr.HandleKeyDownEvent(SFKey.Code.Left, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].LeftKeyPressed = true;
                    }
                }
                else
                {
                    if (Gamepads[a].Joysticks[b].LeftKeyPressed)
                    {
                        SFMgr.HandleKeyUpEvent(SFKey.Code.Left, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].LeftKeyPressed = false;
                    }
                }

                if (currD)
                {
                    Gamepads[a].Joysticks[b].DownTimerF += UnityEngine.Time.deltaTime;

                    if (!Gamepads[a].Joysticks[b].DownKeyPressed || Gamepads[a].Joysticks[b].DownTimerF > RepeatTimerThresholdF)
                    {
                        Gamepads[a].Joysticks[b].DownTimerF = 0f;
                        SFMgr.HandleKeyDownEvent(SFKey.Code.Down, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].DownKeyPressed = true;
                    }
                }
                else
                {
                    if (Gamepads[a].Joysticks[b].DownKeyPressed)
                    {
                        SFMgr.HandleKeyUpEvent(SFKey.Code.Down, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].DownKeyPressed = false;
                    }
                }

                if (currU)
                {
                    Gamepads[a].Joysticks[b].UpTimerF += UnityEngine.Time.deltaTime;

                    if (!Gamepads[a].Joysticks[b].UpKeyPressed || Gamepads[a].Joysticks[b].UpTimerF > RepeatTimerThresholdF)
                    {
                        Gamepads[a].Joysticks[b].UpTimerF = 0f;
                        SFMgr.HandleKeyDownEvent(SFKey.Code.Up, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].UpKeyPressed = true;
                    }
                }
                else
                {
                    if (Gamepads[a].Joysticks[b].UpKeyPressed)
                    {
                        SFMgr.HandleKeyUpEvent(SFKey.Code.Up, 0, Gamepads[a].controllerIndex);
                        Gamepads[a].Joysticks[b].UpKeyPressed = false;
                    }
                }
            }
        }
	}

	bool IsConsole()
	{
		return false;
	}
}