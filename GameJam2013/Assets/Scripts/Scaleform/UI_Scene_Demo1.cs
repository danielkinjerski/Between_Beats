/**********************************************************************

Filename    :	UI_Scene_Demo1.cs
Content     :  
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using System;
using System.Collections;
using UnityEngine;
using Scaleform;
using Scaleform.GFx;


public class UI_Scene_Demo1 : Movie
{
    // Reference to the primary MovieClip (hud) within the Demo.swf movie. This is not the root!
    protected Value	demo1 = null;
    protected bool	btn1Toggle = true;
    protected bool	btn3Toggle = true;
    protected bool	btn7Toggle = true;
    protected bool	btn8Toggle = false;
    protected int	rot = 0;
    private Movie	TestMovie = null; 
    private SFManager SFMgr = null;
    #if UNITY_IPHONE || UNITY_ANDROID
    private TouchScreenKeyboard Keyboard = null;
    private Boolean KeyboardActive = false;
    private Value TextfieldRef = null;
    #endif
    Movie movieToReplace = null;
    RenderTexture RenderableTexture = null;
    
    public UI_Scene_Demo1(SFManager sfmgr, SFMovieCreationParams cp, RenderTexture renderableTexture) :
        base(sfmgr, cp)
    {
        SFMgr = sfmgr;
        RenderableTexture = renderableTexture;
        this.SetFocus(true);
    }

    public void OnRegisterSWFCallback(Value movieRef)
    {
        Debug.Log("In UnityScript: Registering Demo1 Movie");
        demo1 = movieRef;
    }

    public void OnButton1Clicked(String strng)
    {
        Debug.Log("In UnityScript: OnButton1Clicked");
    }

    public void OnTextfieldFocus(Value textFieldRef)
    {
        Debug.Log("In UnityScript: Textfield received focus");
        #if UNITY_IPHONE || UNITY_ANDROID
            TextfieldRef = textFieldRef;
            Keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
            KeyboardActive = true;
        #endif
    }
	
    public override void Update()
    {
        #if UNITY_IPHONE || UNITY_ANDROID
            if (TextfieldRef != null && Keyboard != null)
            {
                if (Keyboard.done && (KeyboardActive == true))
                {
                    TextfieldRef.SetText(Keyboard.text);
                    Debug.Log("In UnityScript: Setting New Text: " + Keyboard.text);
                    Keyboard.active = false;
                    KeyboardActive	= false;
                }
            }
        #endif
    }

    public void OnButton2Clicked(int _rot)
    {
        Debug.Log("In UnityScript: OnButton2Clicked " + "args: " + rot);
        if (demo1 != null)
        {
            rot = _rot;
        }
    }
	
    // This shows how to use displayInfo to modify display properties of flash objects
    public void OnButton3Clicked(Value mc)
    {
        Debug.Log("In UnityScript: OnButton3Clicked");
        if (demo1 != null)
        {
            Value targetMovie = demo1.GetMember("mc3");
            // ButtonMovie should equal mc, since we passed the third movieclip in the externalinterface callback, 
            // check that this is the case
            Debug.Log("Is the passed argument the same as the one obtained from GetMember? " + mc.Equals(targetMovie));
			
            if (targetMovie != null)
            {
                SFDisplayInfo dInfo = targetMovie.GetDisplayInfo();
                // Can print the various enteries of DisplayInfo if you like
                //	dInfo.Print();
                if (dInfo == null) return;
                if (btn3Toggle)
                {
                    dInfo.Z += 100;
                    targetMovie.SetDisplayInfo(dInfo);
                }
                else
                {
                    dInfo.Z -= 100;
                    targetMovie.SetDisplayInfo(dInfo);
                }
                btn3Toggle = !btn3Toggle;
            }
        }
    }

    public void OnButton4Clicked(Value scoreArray)
    {
        Debug.Log("In OnButton4Clicked");
        int len = scoreArray.GetArraySize();
        Debug.Log("Unity: Array Length: " + len);
        Value elem = scoreArray.GetElement(0);
        // Due to asynchronous nature of ActionScript Callbacks, the name change will take effect in the next frame!"
        elem.SetMember("Name", "Michael");
    }

    public void OnButton7Clicked(String strng)
    {
        Debug.Log("In UnityScript: OnButton7Clicked");
        
        if (demo1 != null)
        {
            GameObject sound1 = GameObject.Find("Sound1") as GameObject;
            if (sound1 != null)
            {
                AudioSource curAudioSource = sound1.GetComponent(typeof(AudioSource)) as AudioSource;
                curAudioSource.Play();
            }
            btn7Toggle = !btn7Toggle;
        }
    }
	
    public void OnButton8Clicked(bool bToggle)
    {
        Debug.Log("In UnityScript: OnButton8Clicked");
		
        btn8Toggle = bToggle;
        
        SFCamera camera = Component.FindObjectOfType(typeof(SFCamera)) as SFCamera;
        if (camera == null)
        {
            return;
        }
        
        if (btn8Toggle)
        {
            if (movieToReplace != null)
            {
                return;
            }
            
            SFMovieCreationParams creationParams = camera.CreateMovieCreationParams("Window_texture.swf");
            creationParams.eScaleModeType = ScaleModeType.SM_NoScale;
            creationParams.bInitFirstFrame = true;
            movieToReplace = new Movie(SFMgr, creationParams);

            if (movieToReplace != null)
            {
                SFMgr.ReplaceTexture(movieToReplace.GetID(), "texture1", RenderableTexture.GetNativeTextureID(), RenderableTexture.width, RenderableTexture.height);
            }
        }
        else
        {
            if (movieToReplace == null)
            {
                return;
            }
            
            SFMgr.DestroyMovie(movieToReplace);
            movieToReplace = null;
        }
    }

    public void OnButton6Clicked(String movieName, bool bMovieLoaded)
    {
        Debug.Log("In OnButton6Clicked");

        if (!bMovieLoaded)
        {
            SFCamera sfCam = Component.FindObjectOfType(typeof(SFCamera)) as SFCamera;
            if (sfCam)
                TestMovie = new Movie(SFMgr, sfCam.CreateMovieCreationParams(movieName));
        }
        else
        {
            SFMgr.DestroyMovie(TestMovie);
            TestMovie = null;
        }
    }

    public int GetRotation()
    {
        return rot;
    }

    public void SendFlashAMessage(string msg)
    {
        Debug.Log("SendFlashAMessage: " + msg);
        Value frameVal = new Value(msg, MovieID);
        Value[] args = { frameVal };
        Value retVal = Invoke("root.ReceiveMessageFromUnity", args, 1);
        Debug.Log("Return Value from SendFlashAMessage: " + retVal);
    }
}
	
	