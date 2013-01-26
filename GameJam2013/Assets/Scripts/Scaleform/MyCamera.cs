
/**********************************************************************

Filename    :   MyCamera.cs
Content     :   Inherits from SFCamera
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
using Scaleform;

/* The user should override SFCamera and add methods for creating movies whenever specific events take place in the game.
*/
public class MyCamera : SFCamera {
	
    public UI_Scene_Demo1 demo1 = null;
    public RenderTexture RenderableTexture = null;
    
    // Hides the Start function in the base SFCamera. Will be called every time the ScaleformCamera (Main Camera game object)
    // is created. Use new and not override, since return type is different from that of base::Start()
    new public  IEnumerator Start()
    {
        base.Start();
        yield return StartCoroutine("CallPluginAtEndOfFrames");		
    }

    // Application specific code goes here
    new public void Update()
    {
        CreateGameHud();

        int rotation = 0;
        if (demo1 != null)
        {
            rotation = demo1.GetRotation();
        }
        transform.Rotate(0,0,rotation*Time.deltaTime);
        base.Update ();
    }
	
    private void CreateGameHud()
    {
        if (demo1 == null)
        {
            // SFManager is created in base.Start()
            SFMovieCreationParams creationParams = CreateMovieCreationParams("Demo1.swf");
            creationParams.eScaleModeType = ScaleModeType.SM_ShowAll;
            creationParams.bInitFirstFrame = false;
            //    demo1 = new GFx.Movie(SFMgr, creationParams);
            //   Scaleform.GFx.Movie demo2 = new Scaleform.GFx.Movie(SFMgr, creationParams);
            demo1 = new UI_Scene_Demo1(SFMgr, creationParams, RenderableTexture);

            // SFManager is created in base.Start()
            //SFMovieCreationParams creationParams2 = CreateMovieCreationParams("Scaling1.swf");
            //creationParams2.eScaleModeType = ScaleModeType.SM_NoScale;
            //creationParams2.InitFirstFrame = true;
            //Scaleform.GFx.Movie demo2 = new Scaleform.GFx.Movie(SFMgr, creationParams2);
        }
    }
}