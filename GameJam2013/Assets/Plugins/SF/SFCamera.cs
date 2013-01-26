/**********************************************************************

Filename    :	SFCamera.cs
Content     :	Creates ScaleformManager, Hooks into Unity event handling system
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

// Note that SFCamera is currently a Singleton as it creates a new SFMgr in Start().
public class SFCamera : MonoBehaviour
{

	// true if the SFCamera has been initialized; false otherwise.
	public bool isInitialized = false;
	// The mouse position during the last onGUI() call. Used for MouseMove tracking.
	protected Vector2 lastMousePos;
	// Reference to the SFManager that manages all SFMovies.
	protected static SFManager SFMgr;

	protected static bool bInitOnce = false;

    protected SFGamepad gamePad;

#if UNITY_IPHONE
	[DllImport("__Internal")]
	public static extern void UnityRenderEvent(int id);

	[DllImport("__Internal")]
	private static extern void SF_Uninit();
	
#elif UNITY_ANDROID
	[DllImport("gfxunity3d")]
	public static extern void UnityRenderEvent(int id);

	[DllImport("gfxunity3d")]
	private static extern void SF_Uninit();

#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
	[DllImport("libgfxunity3d")]
	private static extern void SF_Uninit();

#endif
	public SFInitParams InitParams;
	public void Awake()
	{
		//  Mark DontDestroyOnload so that this object is not destroyed when MainMenu level is unloaded and Game Level is loaded.

		//	if (bInitOnce == false)
		{

			//		bInitOnce = true;
		}
	}
	public void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		SFMgr = new SFManager(InitParams);
        gamePad = new SFGamepad(SFMgr);
        gamePad.Init();
		SFMgr.Init();
		// SFMgr.InstallDelegates();
		InitParams.Print();
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
		GL.IssuePluginEvent(0);
#elif UNITY_IPHONE	|| UNITY_ANDROID	
		UnityRenderEvent(0);
#endif
		GL.InvalidateState();

	}
	// Issues calls to Scaleform Rendering. Rendering is multithreaded on windows and single threaded on iOS/Android
	protected IEnumerator CallPluginAtEndOfFrames()
	{
		while (true)
		{
			// Wait until all frame rendering is done
			yield return new WaitForEndOfFrame();
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
			GL.IssuePluginEvent(1);
#elif UNITY_IPHONE	|| UNITY_ANDROID
            UnityRenderEvent(1);
#endif
		}
	}

	void OnApplicationQuit()
	{
		// This is used to initiate RenderHALShutdown, which must take place on the render thread. 
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
		GL.IssuePluginEvent(2);
#endif
		SF_Uninit();
	}

	public SFManager GetSFManager()
	{
		return SFMgr;
	}
/*
	public void AddValueToReleaseList(IntPtr valIntPtr)
	{
		if (SFMgr != null)
		{
			SFMgr.AddValueToReleaseList(valIntPtr);
		}
	}
*/
	// Hook into Unity Events
	public virtual void OnGUI()
	{
        if (SFMgr == null) return;

        
		// Process touch events:
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				SFMgr.HandleTouchEvent(touch);
			}
		}

		Vector2 mousePos;

		// Get Time since last frame: keep in mind the following from Unity Doc
		// Note that you should not rely on Time.deltaTime from inside OnGUI since 
		// OnGUI can be called multiple times per frame and deltaTime would hold the 
		// same value each call, until next frame where it would be updated again.
		// float deltaTime = Time.deltaTime;
		// Check if the mouse moved:

		mousePos = Event.current.mousePosition;
		if ((mousePos[0] != lastMousePos[0]) || (mousePos[1] != lastMousePos[1]))
		{
			SFMgr.HandleMouseMoveEvent(mousePos[0], mousePos[1]);
			lastMousePos[0] = mousePos[0]; lastMousePos[1] = mousePos[1];
		}

		switch (Event.current.type)
		{
			case EventType.MouseMove:
				break;
			case EventType.MouseDown:
				SFMgr.HandleMouseEvent(Event.current);
				break;
			case EventType.MouseUp:
				SFMgr.HandleMouseEvent(Event.current);
				break;
			case EventType.KeyDown:
				if (Event.current.keyCode != KeyCode.None)
				{
					SFMgr.HandleKeyDownEvent(Event.current);
				}
				if (Event.current.character != 0)
				{
					SFMgr.HandleCharEvent(Event.current);
				}
				break;
			case EventType.KeyUp:
				SFMgr.HandleKeyUpEvent(Event.current);
				break;
			case EventType.Repaint:

				break;
		}
	}

	public virtual void OnDestroy()
	{

	}

	public void Update()
	{
		if (SFMgr != null)
		{
            gamePad.Update();
			SFMgr.ProcessCommands();
			SFMgr.Update();
			SFMgr.Advance(Time.deltaTime);
		}
	}

	public void GetViewport(ref int ox, ref int oy, ref int width, ref int height)
	{
		width = Screen.width;
		height = Screen.height;
		ox = 0; oy = 0;
		// Note that while using D3D renderer, the tool bar (that contains "Maximize on Play" text) is part of 
		// the viewport, while using GL renderer, it's not. So there should be a further condition here depending on 
		// what type of renderer is being used, however I couldn't find a macro for achieving that. 
#if UNITY_EDITOR 
		oy = 24;
#endif
	}

    public SFMovieCreationParams CreateRTTMovieCreationParams(string swfName, int RTToX, int RTToY, RenderTexture texture, Color32 clearColor)
    {
        // Used for Android only
        Int64 start = 0, length = 0;
        Int32 fd = 0;

        // On Android, swf file data is read into a buffer located in unmanaged memory. This buffer is passed to 
        // Scaleform runtime when the movie is created. Buffer lifetime is managed in C# and the buffer is destroyed 
        // when the movie is destroyed. 
        // TBD: figure out a way to load swf file from disk on Android.
        IntPtr pDataUnManaged = new IntPtr();
        String SwfPath = SFManager.GetScaleformContentPath() + swfName;

#if (!UNITY_EDITOR && UNITY_ANDROID)
		GetFileInformation(SwfPath, ref fd, ref start, ref length, ref pDataUnManaged);
#endif

        return new SFMovieCreationParams(SwfPath, RTToX, RTToY, texture.width, texture.height, start, length, pDataUnManaged, fd, false, texture, clearColor, ScaleModeType.SM_ShowAll, true);
    }

    public SFMovieCreationParams CreateMovieCreationParams(string swfName)
    {
        int ox = 0, oy = 0, width = 0, height = 0;
        // Used for Android only
        Int64 start = 0, length = 0;
        Int32 fd = 0;
        // On Android, swf file data is read into a buffer located in unmanaged memory. This buffer is passed to 
        // Scaleform runtime when the movie is created. Buffer lifetime is managed in C# and the buffer is destroyed 
        // when the movie is destroyed. 
        // TBD: figure out a way to load swf file from disk on Android.
        IntPtr pDataUnManaged = new IntPtr();
        String SwfPath = SFManager.GetScaleformContentPath() + swfName;
#if (!UNITY_EDITOR && UNITY_ANDROID)
		GetFileInformation(SwfPath, ref fd, ref start, ref length, ref pDataUnManaged);
#endif
        GetViewport(ref ox, ref oy, ref width, ref height);
        return new SFMovieCreationParams(SwfPath, ox, oy, width, height, start, length, pDataUnManaged, fd, false, ScaleModeType.SM_ShowAll, true);
    }

    protected void GetFileInformation(String name, ref Int32 fd, ref Int64 start, ref Int64 length, ref IntPtr pDataUnManaged)
	{
#if UNITY_ANDROID
		IntPtr cls_Activity = (IntPtr)AndroidJNI.FindClass("com/unity3d/player/UnityPlayer");
		IntPtr fid_Activity = AndroidJNI.GetStaticFieldID(cls_Activity, "currentActivity", "Landroid/app/Activity;");
		IntPtr obj_Activity = AndroidJNI.GetStaticObjectField(cls_Activity, fid_Activity);

		IntPtr obj_cls = AndroidJNI.GetObjectClass(obj_Activity);
		IntPtr asset_func = AndroidJNI.GetMethodID(obj_cls, "getAssets", "()Landroid/content/res/AssetManager;");

		IntPtr assetManager = AndroidJNI.CallObjectMethod(obj_Activity, asset_func, new jvalue[2]);

		IntPtr assetManagerClass = AndroidJNI.GetObjectClass(assetManager);
		IntPtr openFd = AndroidJNI.GetMethodID(assetManagerClass, "openFd", "(Ljava/lang/String;)Landroid/content/res/AssetFileDescriptor;");
		jvalue[] param_array2 = new jvalue[2];
		jvalue param = new jvalue();
		param.l = AndroidJNI.NewStringUTF(name);
		param_array2[0] = param;
		IntPtr jfd;
		try
		{
			jfd = AndroidJNI.CallObjectMethod(assetManager, openFd, param_array2);
			IntPtr assetFdClass = AndroidJNI.GetObjectClass(jfd);
			IntPtr getParcelFd = AndroidJNI.GetMethodID(assetFdClass, "getParcelFileDescriptor", "()Landroid/os/ParcelFileDescriptor;");
			IntPtr getStartOffset = AndroidJNI.GetMethodID(assetFdClass, "getStartOffset", "()J");
			IntPtr getLength = AndroidJNI.GetMethodID(assetFdClass, "getLength", "()J");
			start = AndroidJNI.CallLongMethod(jfd, getStartOffset, new jvalue[2]);
			length = AndroidJNI.CallLongMethod(jfd, getLength, new jvalue[2]);

			IntPtr fileInputStreamId = AndroidJNI.GetMethodID(assetFdClass, "createInputStream", "()Ljava/io/FileInputStream;");
			IntPtr fileInputStream = AndroidJNI.CallObjectMethod(jfd, fileInputStreamId, new jvalue[2]);
			IntPtr fileInputStreamClass = AndroidJNI.GetObjectClass(fileInputStream);
			// Method signatures:newbytear B: byte, Z: boolean
			IntPtr read = AndroidJNI.GetMethodID(fileInputStreamClass, "read", "([BII)I");
			jvalue[] param_array = new jvalue[3];
			jvalue param1 = new jvalue();
			IntPtr pData = AndroidJNI.NewByteArray((int)(length));
			param1.l = pData;
			jvalue param2 = new jvalue();
			param2.i = 0;
			jvalue param3 = new jvalue();
			param3.i = (int)(length);
			param_array[0] = param1;
			param_array[1] = param2;
			param_array[2] = param3;
			int numBytesRead = AndroidJNI.CallIntMethod(fileInputStream, read, param_array);
			UnityEngine.Debug.Log("Bytes Read = " + numBytesRead);

			Byte[] pDataManaged = AndroidJNI.FromByteArray(pData);
			pDataUnManaged = Marshal.AllocCoTaskMem((int)length);
			Marshal.Copy(pDataManaged, 0, pDataUnManaged, (int)length);

			jfd = AndroidJNI.CallObjectMethod(jfd, getParcelFd, new jvalue[2]);

			IntPtr parcelFdClass = AndroidJNI.GetObjectClass(jfd);
			jvalue[] param_array3 = new jvalue[2];
			IntPtr getFd = AndroidJNI.GetMethodID(parcelFdClass, "getFileDescriptor", "()Ljava/io/FileDescriptor;");
			jfd = AndroidJNI.CallObjectMethod(jfd, getFd, param_array3);
			IntPtr fdClass = AndroidJNI.GetObjectClass(jfd);

			IntPtr descriptor = AndroidJNI.GetFieldID(fdClass, "descriptor", "I");
			fd = AndroidJNI.GetIntField(jfd, descriptor);

		}
		catch (IOException ex)
		{
			UnityEngine.Debug.Log("IO Exception: Failed to load swf file");
		}
#endif
		return;
	}
}