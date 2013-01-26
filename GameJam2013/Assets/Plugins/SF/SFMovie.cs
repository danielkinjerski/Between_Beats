/**********************************************************************

Filename    :	SFMovie.cs
Content     :	Movie Wrapper
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace Scaleform
{
namespace GFx
{

[StructLayout(LayoutKind.Explicit,CharSet=CharSet.Ansi)]
class test
{
    public test(int _a, int _b)
    {a = _a; b = _b;
    }
    [FieldOffset(0), MarshalAs( UnmanagedType.I4)] public int a;
    [FieldOffset(4), MarshalAs( UnmanagedType.I4)]public int b;
}

public partial class Movie
{

    public    String        MovieName;
    public    long          MovieID;
    public    Boolean       MarkForRelease;
    public    SFManager     sfMgr;
    public    bool          IsFocussed;
    public    bool          bAdvanceWhenGamePaused;
    public    bool          bAutoManageViewport;
	// Currently used on Android to read swf file data in an unmanaged memory buffer and pass it to Scaleform runtime
	public	  IntPtr		pDataUnmanaged;
	public    ScaleModeType eScaleModeType;
    public    SFRTT         mRenderTexture;
    struct _ViewPort
    {
        public int oX;
        public int oY;
        public int Width;
        public int Height;
    } 
    _ViewPort ViewPort;

    public Movie()
    {
        MovieID = 0;
    }

    public Movie(SFManager sfmgr, SFMovieCreationParams creationParams)
    {
        if (sfmgr == null)
        {
            MovieID = 0;
            return;
        }
        
        MovieName                   = creationParams.MovieName;
        ViewPort.oX                 = creationParams.oX;
        ViewPort.oY                 = creationParams.oY;
        ViewPort.Width              = creationParams.Width;
        ViewPort.Height             = creationParams.Height;
		pDataUnmanaged				= creationParams.pData; 
        MovieID                     = 0; // Assigned when the C++ Movie is created. 
        MarkForRelease              = false;
        sfMgr                       = sfmgr;
        IsFocussed                  = false;
        bAdvanceWhenGamePaused      = false;
        bAutoManageViewport         = creationParams.bAutoManageViewport;
		eScaleModeType				= creationParams.eScaleModeType;

		int cpSize = Marshal.SizeOf(typeof(SFMovieCreationParams));

		IntPtr pdata = Marshal.AllocCoTaskMem(cpSize);
		Marshal.StructureToPtr(creationParams, pdata, false);

        MovieID = SF_CreateMovie(pdata);
		Marshal.DestroyStructure(pdata, typeof(SFMovieCreationParams));
		if (MovieID != -1)
		{
			sfMgr.AddMovie(this);
		}
    }
    
    // Important: We can't destroy the underlying C++ Movie object in finalize method, because this Finalize
    // can get called from the garbage collector thread, and we can only destroy C++ Movie objects from
    // the main thread. Therefore, we inform the SFManager that this movie is to be deleted, and it's put on a
    // release queue. This queue is cleared (see ReleaseMoviesMarkedForRelease method on the SFMAnager) during
    // the update function, which is invoked on the main thread. 
    ~Movie()
    {
        Console.WriteLine("In Movie Destructor. ID = " + MovieID);
        if (MovieID != 0)
        {
            MarkForRelease = true;
            if (sfMgr != null)
            {
                sfMgr.AddToReleaseList(MovieID);
            }
            MovieID = -1;
        }
		Marshal.FreeCoTaskMem(pDataUnmanaged);
    }
    
    public long GetID() 
    { 
        return MovieID; 
    }
    
    public void SetID(long id)
    {
        MovieID = id; 
    }
    
    public void OnSFManagerDied()
    {
        // The SFManager just died, set our reference to null. 
        sfMgr = null;
    }
    
    public void SetViewport(int ox, int oy, int width, int height)
    {
        ViewPort.oX = ox; ViewPort.oY = oy; ViewPort.Width = width;
        ViewPort.Height = height;
    }
    
    public virtual void Update()
    {
        // Override in derived class for movie specific update actions
    }
    
    public virtual void Advance(float deltaTime)
    {
        if (MovieID == -1) return;
        if (!MarkForRelease)
        {
            if (bAdvanceWhenGamePaused)
            {
                // Advance the movie automatically.
                SF_Advance(MovieID, 0, true);
            }
            else
            {
                SF_Advance(MovieID, deltaTime);
            }
        }
    }

    public bool DoHitTest(float x, float y)
    {
        if (MovieID == -1 || mRenderTexture == null)
        {
            return false;
        }

        // Adjust according to viewport
        if (mRenderTexture)
        {
            RaycastHit hit;
            if (Camera.main!=null && Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(x, y)), out hit))
            {
                Renderer hitRenderer = hit.collider.renderer;
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (!(hit.collider is MeshCollider) || hitRenderer == null || meshCollider == null)
                {
                    return false;
                }

                if (hit.collider.gameObject.GetComponent("SFMovie") == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        return SF_DoHitTest(MovieID, x, y);
    }

    public void SetFocus(bool focus)
    {
        if (MovieID == -1)
        {
            return;
        }
        IsFocussed = focus;
        SF_SetFocus(MovieID, focus);
    }

    public void SetRTT(SFRTT rtt)
    {
        mRenderTexture = rtt;
    }

    public SFRTT GetRTT() { return mRenderTexture; }

    public bool IsRTTMovie()
    {
        return mRenderTexture == null ? false : true;
    }
    public virtual bool AcceptKeyEvents()
    {
        // Can check for various conditions here. Derived classes can override this function as well
        return IsFocussed;
    }

    public virtual bool AcceptCharEvents()
    {
        // Can check for various conditions here. Derived classes can override this function as well
        return IsFocussed;
    }
    
    public virtual bool AcceptMouseEvents()
    {
        // Can check for various conditions here. Derived classes can override this function as well
        // Check if the mouse event is over the movie viewport..
        return true;
    }

	public virtual bool AcceptTouchEvents()
	{
		// Can check for various conditions here. Derived classes can override this function as well
		// Check if the mouse event is over the movie viewport..
		return true;
	}
    
    public bool HandleMouseEvent(float x, float y, int icase)
    {
        if (MovieID == -1)
        {
            return false;
        }
        
        if (AcceptMouseEvents())
        {
            // Adjust according to viewport
            float xx = x;
            float yy = y;
            if (mRenderTexture != null && Camera.main != null)
            {
				RaycastHit hit;
        		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(x, Screen.height - y)), out hit))
				{
          			Renderer hitRenderer = hit.collider.gameObject.renderer;
                    MeshCollider meshCollider = hit.collider as MeshCollider;
                    if (!(hit.collider is MeshCollider) || hitRenderer == null || meshCollider == null)
                    {
                        return false;
                    }

                    SFRTT rtt = hit.collider.gameObject.GetComponent("SFRTT") as SFRTT;
                    if (rtt == null)
                    {
                        return false;
                    }
	                float rttWidth 	= hitRenderer.material.mainTexture.width;
	                float rttHeight = hitRenderer.material.mainTexture.height;
	                float hitCoordX = hit.textureCoord.x * rttWidth;
	                float hitCoordY = rttHeight - hit.textureCoord.y * rttHeight;
	                xx = hitCoordX;
	                yy = hitCoordY;
                    yy -= 32; // HACK
				}
            }
            else
            {
                xx -= ViewPort.oX;
                yy -= ViewPort.oY;
            }

            if (SF_HandleMouseEvent(MovieID, xx, yy, icase))
            {
                UnityEngine.Debug.Log("Handle mouse event");
                return true;
            }
        }
        return false;
    }

    public bool HandleKeyEvent(SFKey.Code cd, SFKeyModifiers.Modifiers mod, UInt32 down, int keyboardIndex = 0)
    {
        if (MovieID == -1)
        {
            return false;
        }
        if (AcceptKeyEvents())
        {
            return SF_HandleKeyEvent(MovieID, cd, mod, down, keyboardIndex);
        }
        return false;
    }
    
    public bool HandleCharEvent(UInt32 wchar)
    {
        if (MovieID == -1)
        {
            return false;
        }
        if (AcceptCharEvents())
        {
            return SF_HandleCharEvent(MovieID, wchar);
        }
        return false;
    }
    
    public bool HandleTouchEvent(int fingerId, float x, float y, int icase)
    {
		if (AcceptTouchEvents())
        {
            // Adjust according to viewport
            float xx = x - ViewPort.oX;
    	   	float yy = y; // - ViewPort.oY;
            return SF_HandleTouchEvent(MovieID, fingerId, xx, yy, icase);
        }
        return false;
    }
    
    public void SetBackgroundAlpha(float alpha)
    {
        if (MovieID == -1)
        {
            return;
        }
        SF_SetBackgroundAlpha(MovieID, alpha);
    }
    
    public Value Invoke(String methodName, Value[] valArray, int numElem)
    {
        int valueSize   = Marshal.SizeOf(typeof(Value));
        int IntPtrSize  = Marshal.SizeOf(typeof(IntPtr));
        int IntSize     = Marshal.SizeOf(typeof(int)); 
        
        IntPtr ptr = Marshal.AllocCoTaskMem(valueSize * numElem);

        for (int i = 0; i < numElem; i++)
        {
            // Can't add an integer offset to IntPtr as you would with C/C++ pointer 
            IntPtr data = new IntPtr(ptr.ToInt32() + valueSize * i);
            Marshal.WriteIntPtr(data, valArray[i].internalData);
            data = new IntPtr(data.ToInt32() + IntPtrSize);
            Marshal.WriteInt32(data, (int)valArray[i].type);
            data = new IntPtr(data.ToInt32() + IntSize);
            Marshal.WriteInt64(data, (long)valArray[i].MovieId);
        }
        Value retVal = new Value(); 
        bool result = SF_Invoke3(MovieID, methodName, numElem, ptr, retVal);
        Marshal.FreeCoTaskMem(ptr);
		if (result)
		{
			return retVal;
		}
		else
		{
			return new Value(false, MovieID);
		}
    
    }
}

} // namespace GFx;

} // namespace Scaleform;