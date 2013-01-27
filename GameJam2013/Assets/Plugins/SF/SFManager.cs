/**********************************************************************

Filename    :	SFManager.cs
Content     :	SFManager implementation
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using Scaleform.GFx;

namespace Scaleform
{
    
public class DisplayInfo
{
    float x;
    float y;
    float z;
    float rotation;
    float xscale;
    float yscale;
    float zscale;
    float alpha;
    bool  visible;
    float xrotation;
    float yrotation;
}

public enum ScaleModeType
{
	SM_NoScale,
	SM_ShowAll,
	SM_ExactFit,
	SM_NoBorder
};

// Note: Must use struct and not class, otherwise iOS will throw AOT compilation errors
[StructLayout(LayoutKind.Explicit, Pack = 4)]
public struct SFMovieCreationParams
{
    public SFMovieCreationParams(String name, int ox, int oy, int width, int height,
        Int64 start, Int64 length,	IntPtr pdata, Int32 fd, bool initFirstFrame,
        RenderTexture texture, Color32 clearColor,
        ScaleModeType scaleModeType = ScaleModeType.SM_ShowAll, bool bautoManageVP = true)
    {
        MovieName               = name;
        oX                      = ox;
        oY                      = oy;
        Width                   = width;
        Height                  = height;
		Start					= start;
		Length					= length;
		Fd						= fd;
        bInitFirstFrame          = initFirstFrame;
        bAutoManageViewport     = bautoManageVP;
		pData					= pdata;
		eScaleModeType			= scaleModeType;
        bPadding                = false;
        bRenderToTexture        = (texture != null);
        textureId               = ((texture != null) ? (UInt32)texture.GetNativeTextureID() : 0);
        texWidth                = ((texture != null) ? (UInt32)texture.width : 0);
        texHeight               = ((texture != null) ? (UInt32)texture.height : 0);
        clearRed                = clearColor.r;
        clearGreen              = clearColor.g;
        clearBlue               = clearColor.b;
        clearAlpha              = clearColor.a;
        //	pData = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.IntPtr)));
	//	Marshal.WriteIntPtr(pData, pdata);
    }

    public SFMovieCreationParams(String name, int ox, int oy, int width, int height,
        Int64 start, Int64 length, IntPtr pdata, Int32 fd, bool initFirstFrame,
        ScaleModeType scaleModeType = ScaleModeType.SM_ShowAll, bool bautoManageVP = true)
    {
        MovieName = name;
        oX = ox;
        oY = oy;
        Width = width;
        Height = height;
        Start = start;
        Length = length;
        Fd = fd;
        bInitFirstFrame = initFirstFrame;
        bAutoManageViewport = bautoManageVP;
        pData = pdata;
        eScaleModeType = scaleModeType;
        bPadding = false;
        bRenderToTexture = false;
        textureId = 0;
        texWidth = 0;
        texHeight = 0;
        clearRed = 0;
        clearGreen = 0;
        clearBlue = 0;
        clearAlpha = 0;
    }

	[FieldOffset(0)]
    public String   MovieName;
	[FieldOffset(4)]
    public int      oX;
	[FieldOffset(8)]
    public int      oY;
	[FieldOffset(12)]
    public int      Width;
	[FieldOffset(16)]
    public int      Height;
	// The song and dance below is necessary because OSX's compiler does not align Start to 8 bytes on the C++ side of things
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(20)]
#else
	[FieldOffset(24)]
#endif
    public Int64	Start;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(28)]
#else
	[FieldOffset(32)]
#endif
	public Int64	Length;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(36)]
#else
	[FieldOffset(40)]
#endif
	public IntPtr	pData;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(40)]
#else
	[FieldOffset(44)]
#endif
	public Int32	Fd;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(44)]
#else
	[FieldOffset(48)]
#endif
	public ScaleModeType eScaleModeType;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(48)]
#else
	[FieldOffset(52)]
#endif
    // Determines if the movie is advanced right after creation. 
    public bool     bInitFirstFrame;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(49)]
#else
	[FieldOffset(53)]
#endif
    public bool     bAutoManageViewport;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(50)]
#else
	[FieldOffset(54)]
#endif
    public bool     bPadding;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(51)]
#else
	[FieldOffset(55)]
#endif
    public bool     bRenderToTexture;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(52)]
#else
	[FieldOffset(56)]
#endif
    public UInt32   texWidth;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(56)]
#else
	[FieldOffset(60)]
#endif
    public UInt32   texHeight;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(60)]
#else
	[FieldOffset(64)]
#endif
    public Byte     clearRed;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(61)]
#else
	[FieldOffset(65)]
#endif
    public Byte     clearGreen;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(62)]
#else
	[FieldOffset(66)]
#endif
    public Byte     clearBlue;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(63)]
#else
	[FieldOffset(67)]
#endif
    public Byte     clearAlpha;
#if UNITY_STANDALONE_OSX || UNITY_IPHONE
	[FieldOffset(64)]
#else
	[FieldOffset(68)]
#endif
    public UInt32   textureId;
}



// Purpose of this class is to manage Movie creation/destruction etc.
public partial class SFManager
{
    public event SF_ExternalInterfaceDelegate EIEvent
    {
        add
        {
            sf_eiDelegate += value;
        }
        remove
        {
            sf_eiDelegate -= value;
        }
    }
    
    public enum MovieLifeCycleEvents:int 
    {
        Movie_Created = 0,
        Movie_Destroyed, 
    }  
    
    // Maintains a list of all SFMovies created in the game
    public List<Movie>              SFMovieList;
    List<long>                      MarkForReleaseIDs;
    static List<IntPtr>             MarkForReleaseValues;    
    List<SFLifecycleEvent>          LifecycleEventsList;
    bool                            sf_Initialized;
    
    SF_ExternalInterfaceDelegate    sf_eiDelegate;
    SF_DisplayInfoDelegate          sf_dInfoDelegate;
    SF_AllocateDelegate             sf_allocDelegate;
    SF_LogDelegate                  sf_logDelegate;
    
    IntPtr                          pValues_PreAllocated;
    IntPtr                          pValueQueue;
    IntPtr                          pCommandQueue;
    IntPtr                          pASOutput;
    Int32                           NumPreAllocatedValues;
    IntPtr                          pCommandOffset;
    IntPtr                          pValueOffset;
    IntPtr                          pASOutputOffset;
    Int32                           MaxLogBufferMessageSize; // corresponds to the buffer size in GFx.
	int								ScreenWidth;
	int								ScreenHeight;
    
    // Delegate Declarations
    public delegate void SF_ExternalInterfaceDelegate( long movieID, String command, IntPtr pValue, int numArgs, int valueSize);
    public delegate void SF_LogDelegate(String message);
    public delegate IntPtr SF_AllocateDelegate( int numVal);
    public delegate IntPtr SF_DisplayInfoDelegate(IntPtr ptr);

	

    public SFManager(SFInitParams initParams)
    {
        SFInitParams2 initParams2   = new SFInitParams2(initParams);
        int initParamsSize          = Marshal.SizeOf(typeof(SFInitParams2));
        // int SFCommandSize           = Marshal.SizeOf(typeof(SFCommand)); // Unused.
        int SFValueSize             = Marshal.SizeOf(typeof(Value));
        
        // initParams2.Print();
        IntPtr pdata = Marshal.AllocCoTaskMem(initParamsSize);
        Marshal.StructureToPtr(initParams2, pdata, false);
        SF_Init(pdata, initParamsSize);
        SF_LoadFontConfig(GetScaleformContentPath() + "FontConfig/");
        AllocateSharedData();
        SF_SetSharedData(pCommandOffset, pCommandQueue, 0);
        SF_SetSharedData(pValueOffset, pValueQueue, 1);  
        SF_SetSharedData(pASOutputOffset, pASOutput, 2);
        
        Marshal.DestroyStructure(pdata, typeof(SFInitParams));
        
        SFMovieList = new List<Movie>();
        
        for (int j = 0; j < SFMovieList.Count; j++) // Loop through List with for
        {
            sf_Initialized = false;
        }
        
        MarkForReleaseIDs           = new List<long>();
        MarkForReleaseValues        = new List<IntPtr>();
        LifecycleEventsList         = new List<SFLifecycleEvent>();
        pValues_PreAllocated        = Marshal.AllocCoTaskMem(SFValueSize * NumPreAllocatedValues);
        
        SFKey.CreateKeyDictionary();
    }

    public void Init()
    {
        sf_Initialized = true;
    }

    public void Destroy()
    {
        // Clear out ReleaseList
        MarkForReleaseIDs.Clear();
        SFMovieList.Clear();
        GC.Collect();
#if UNITY_ANDROID || UNITY_IPHONE	
		Console.WriteLine("In SFManager::OnDestroy");	
		SF_DestroyManager();
		SF_Destroy();
#endif
        sf_Initialized = false;
    }

    ~SFManager()
    {
        Marshal.FreeCoTaskMem(pValues_PreAllocated);
        Marshal.FreeCoTaskMem(pValueQueue);
        Marshal.FreeCoTaskMem(pCommandQueue);
        Marshal.FreeCoTaskMem(pCommandOffset);
        Marshal.FreeCoTaskMem(pValueOffset);
        // Notify all movies that the Manager has been destroyed.
        for (int i = 0; i < SFMovieList.Count; i++)
        {
            SFMovieList[i].OnSFManagerDied();
        }
    }
    
    private void AllocateSharedData()
    {
        int SFCommandSize           = Marshal.SizeOf(typeof(SFCommand));
        int SFValueSize             = Marshal.SizeOf(typeof(Value));
        int SFCharSize              = Marshal.SizeOf(typeof(char));
        NumPreAllocatedValues       = 10;
        MaxLogBufferMessageSize     = 2048; 
        pValueQueue                 = Marshal.AllocCoTaskMem(SFValueSize * NumPreAllocatedValues*10);
        pCommandQueue               = Marshal.AllocCoTaskMem(SFCommandSize * NumPreAllocatedValues);
        pASOutput                   = Marshal.AllocCoTaskMem(10*MaxLogBufferMessageSize*SFCharSize);
        pCommandOffset              = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.Int32)));
        pValueOffset                = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.Int32)));
        pASOutputOffset             = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(System.Int32)));
        
        Marshal.WriteInt32(pCommandOffset, 0);
        Marshal.WriteInt32(pValueOffset, 0);
        Marshal.WriteInt32(pASOutputOffset, 0);
        
        for (int i = 0; i < (10*MaxLogBufferMessageSize); i++)
        {
            Marshal.WriteByte(pASOutput, i, 0);
        }
    }
    
    private Int32 RandomNumber()
    {
        System.Random random = new System.Random();
        return random.Next();
    }
    
    public void OnLogMessage(String message)
    {
        LogMessage(message);
    }
    
    public void LogMessage(String message)
    {
        Console.WriteLine(message);
    }
    
    private void HandleASTraces()
    {
        int numTraces = Marshal.ReadInt32(pASOutputOffset);
        if (numTraces == 0)
        {
            return;
        }
        
        IntPtr ptr = new IntPtr(pASOutput.ToInt32());
        String str = Marshal.PtrToStringAnsi(ptr);
        Console.Write(str); // Write trace() to Console.
        UnityEngine.Debug.Log( str ); // Write trace() to Debug Log.
        
        Marshal.WriteInt32(pASOutputOffset, 0);
        
        // Clear the buffer
        for (int i = 0; i < 10*MaxLogBufferMessageSize; i++)
        {
            Marshal.WriteByte(pASOutput, i, 0);
        }
    }
    
    public SFCommand GetCommandData(IntPtr pqueue)
    {    
        SFCommand command = new SFCommand();
        // Marshal.PtrToStructure(pqueue, command); // Works on Windows, unsupported on iOS due to aot compilation.
        IntPtr  ptr = new IntPtr(pqueue.ToInt32()); // Workaround for iOS.
        command.movieID = Marshal.ReadInt64(ptr);
        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(long)));
        command.argCount = Marshal.ReadInt32(ptr);  
        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(int)));
        command.pmethodName = Marshal.ReadIntPtr(ptr);
        return command;
    }

    public static String GetScaleformContentPath()
    {
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR)
        return Application.dataPath + "/StreamingAssets/";			
#elif UNITY_IPHONE
		return (Application.dataPath + "/Raw/");

#elif UNITY_STANDALONE_OSX 
		return Application.dataPath + "/Data/StreamingAssets/";

#elif UNITY_ANDROID
	return "";
#endif
    }

    // Delegates don't work in an iOS environment. In order to get around this limitation, we put 
    // ExternalInterface notifications in a queue which is preallocated in the constructor of SFManager.
    public void ProcessCommands()
    {
        // Deal with AS Traces:
        HandleASTraces();
            
        int numCommands = Marshal.ReadInt32(pCommandOffset);
        if (numCommands == 0) return;
        
        int commandSize = Marshal.SizeOf(typeof(SFCommand));
        int sfValueSize = Marshal.SizeOf(typeof(Value));
        IntPtr pqueue = new IntPtr(pCommandQueue.ToInt32());
        IntPtr pargs = new IntPtr(pValueQueue.ToInt32());
        
        int cumNumArgs = 0;
        for (int i = 0; i < numCommands; i++)
        {
            pqueue = new IntPtr(pCommandQueue.ToInt32() + commandSize*i);
            SFCommand command       = GetCommandData(pqueue);
            int numArgs             = command.argCount;
            long movieID             = command.movieID;
            
            String methodName = Marshal.PtrToStringAnsi(command.pmethodName);
            OnExternalInterface(movieID, methodName, pargs, numArgs, sfValueSize);
            
            cumNumArgs += numArgs;
            pargs = new IntPtr(pValueQueue.ToInt32() + sfValueSize*cumNumArgs);
        }
        
        SF_ClearCommandBuffer(numCommands);
        Marshal.WriteInt32(pCommandOffset, 0);
        Marshal.WriteInt32(pValueOffset, 0);
    }
    
    IntPtr AllocateImpl(int numVal)
    {
        if (numVal < NumPreAllocatedValues)
        {
            return pValues_PreAllocated;
        }
        else
        {
            int SFValueSize = Marshal.SizeOf(typeof(Value));
            // Allocate space on the COM heap. Should also be able to use AllocHGlobal in order to 
            // allocate space on process heap.
            IntPtr ptr = Marshal.AllocCoTaskMem(SFValueSize * numVal);
            return ptr;
        }
    }
    
    IntPtr AllocateDisplayInfo(IntPtr sz)
    {
        int dInfoSize       = Marshal.SizeOf(typeof(SFDisplayInfo));
        int floatSize       = Marshal.SizeOf(typeof(float));
        int doubleSize      = Marshal.SizeOf(typeof(double));
        int intPtrSize      = Marshal.SizeOf(typeof(IntPtr));
        
        // Allocate space on the COM heap. Should also be able to use AllocHGlobal in order to 
        // allocate space on process heap.
        IntPtr dInfoPtr     = Marshal.AllocCoTaskMem(dInfoSize);
        IntPtr vmptr        = Marshal.AllocCoTaskMem(4*3*floatSize);
        IntPtr projPtr      = Marshal.AllocCoTaskMem(4*4*floatSize);
        
        IntPtr pdata1       = new IntPtr(dInfoPtr.ToInt32() + doubleSize * 11);
        Marshal.WriteIntPtr(pdata1, vmptr);
        
        IntPtr pdata2       = new IntPtr(pdata1.ToInt32() + intPtrSize);
        Marshal.WriteIntPtr(pdata2, projPtr);
        
        // Return size of managed DisplayInfo struct to C++ so that it can be compared with
        // native dinfo size in order for marshalling to work correctly. 
        Marshal.WriteInt32(sz, dInfoSize);
        return dInfoPtr;
    }
    
   public static Value GetValueData(IntPtr pqueue)
    {
        Value pvalue = new Value();
        
        // Marshal.PtrToStructure(pqueue, pvalue); // The easy way to do this (supported on Windows).
        IntPtr ptr = new IntPtr(pqueue.ToInt32()); // Workaround for Mono on iOS.
        pvalue.internalData = Marshal.ReadIntPtr(ptr);
        
		ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(IntPtr)));
        pvalue.type = (Value.ValueType)Marshal.ReadInt32(ptr);
        	
        ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(Int32)));
        pvalue.MovieId = Marshal.ReadInt64(ptr);
        
        return pvalue;
    }
		
    void OnExternalInterface(long movieID, String command, IntPtr ptr, int numArgs, int valueSize)
    {
        int SFValueSize = Marshal.SizeOf(typeof(Value));
        int count = 0;
        
        // Array of types passed to ExternalInterface
        Type[] typeArray;
        System.Object[] args;
        
        // Note that we can't preallocate typeArray and args since we have to pass them to the 
        // GetMethod function below and there is no way to pass the size of the array, so the
        // array can't contain any null values.
        typeArray = new Type[numArgs];
        args = new System.Object[numArgs];
      
        for (int i = 0; i < numArgs; i++)
        {
            // Can't add an integer offset to IntPtr as you would with C/C++ pointer 
            IntPtr data = new IntPtr(ptr.ToInt32() + SFValueSize * i);
            
            // This Value makes a copy of the data and will be garbage collected.
            Value val = GetValueData(data);
            // Value val = (Value)Marshal.PtrToStructure(data, typeof(Value)); // Unsupported on iOS.
            
            if (val.IsString())
            {
                String str = val.GetString();
                typeArray.SetValue(typeof(String), count);
                args.SetValue(str, count);
                count++;
            }
            else if (val.IsNumber())
            {
                double num = val.GetNumber();
                Console.Write(num);
                typeArray.SetValue(typeof(double), count);
                args.SetValue(num, count);
                count++;
            }    
            else if (val.IsBoolean())
            {
                Boolean boolVal = val.GetBool();
                typeArray.SetValue(typeof(Boolean), count);
                args.SetValue(boolVal, count);
                count++;
            }    
            else if (val.IsUInt())
            {
                UInt32 uintVal = val.GetUInt();
                typeArray.SetValue(typeof(UInt32), count);
                args.SetValue(uintVal, count);
                count++;
            }    
            else if (val.IsInt())
            {
                Int32 intVal = val.GetInt();
                typeArray.SetValue(typeof(Int32), count);
                args.SetValue(intVal, count);
                count++;
            }    
            else if (val.IsObject())
            {
                Value newval = val.GetObject();
                typeArray.SetValue(typeof(Value), count);
                args.SetValue(newval, count);
                count++;
            }
        }
        
        // Preallocated memory is destroyed in the destructor
        /* This code can be uncommented if delegates are being used for EI implementation.
        if (ptr != pValues_PreAllocated)
        {
            Console.WriteLine("Destroying Value Array");
            Marshal.FreeCoTaskMem(ptr);
        }
        */
        
        // At this point, count must be = numArgs, since we should be able to determine the type of each Value. If not,
        // there is some problem.
        if (count != numArgs)
        {
            LogMessage("Invalid Type passed in ExternalInterface!");
            return;
        }
        
        for (int j = 0; j < SFMovieList.Count; j++) // Loop through List with for
        {
            Movie movie = SFMovieList[j];
            long mId = movie.GetID();
            if (movieID != mId) continue;
            
            Type SFMovieClass = movie.GetType();
            UnityEngine.Debug.Log("ExternalInterface arrived");
            // Command passed as an argument is the method we want to call
            MethodInfo methodInfo;
            if (typeArray == null)
            {
                methodInfo = SFMovieClass.GetMethod(command);
            }
            else
            {
                methodInfo = SFMovieClass.GetMethod(command, typeArray);
            }
            
            if( methodInfo != null )
            {
                // LogMessage("Method Implementing " + command + " found, Invoking method");
                methodInfo.Invoke(movie, args); // Call the method
            }
            else
            {
                UnityEngine.Debug.Log("Handler for command: " + command + " not found!");
            }
            return;
        }
    }

	public void EnableIME()
	{
#if UNITY_STANDALONE_WIN
		SF_EnableIME();
#endif
	}
    
    public void QueuedDestroy()
    {
        sf_Initialized        = false;
    }
    
    public bool IsSFInitialized()
    {
        return sf_Initialized; 
    }

    public void AddToLifecycleEventList(SFLifecycleEvent ev)
    {
        LifecycleEventsList.Add(ev);
    }
    
    public void AddToReleaseList(long movieId)
    {
        MarkForReleaseIDs.Add(movieId);
    }
    
    static public void AddValueToReleaseList(IntPtr valIntPtr)
    {
        MarkForReleaseValues.Add(valIntPtr);    
    }
    
    public void AddMovie(Movie movie)
    {
        SFMovieList.Add(movie);
    }
    
    public void ReleaseMoviesMarkedForRelease()
    {
        if (MarkForReleaseIDs.Count == 0)
        {
            return;
        }
        for (int i = 0; i < MarkForReleaseIDs.Count; i++) 
        {
            SF_DestroyMovie(MarkForReleaseIDs[i]);
        }
        MarkForReleaseIDs.Clear();
    }
    
    static public void ReleaseValuesMarkedForRelease()
    {
        if (MarkForReleaseValues.Count == 0)
        {
            return;
        }
        for (int i = 0; i < MarkForReleaseValues.Count; i++) 
        {
            // Console.WriteLine("Finalizing Object");
            IntPtr internalData = MarkForReleaseValues[i];
            if (internalData != IntPtr.Zero)
            {
                // Console.WriteLine("Releasing Internal Data");
                Value.SF_DecrementValRefCount(internalData);
            }
        }
        MarkForReleaseValues.Clear();
    }
    
    public void DestroyMovie(Movie movie)
    {    
        SFMovieList.Remove(movie);
        SF_NotifyNativeManager(movie.MovieID, MovieLifeCycleEvents.Movie_Destroyed);
    }
    /*
	public T CreateMovie <T> (SFMovieCreationParams params) where T : Movie
	{
		Activator.CreateInstance (typeof (T), argslist);
	}
	*/
		
    public Movie CreateMovie(SFMovieCreationParams creationParams, Type MovieClassType)
    {
        Type[] argTypes = new Type[]{typeof(SFManager), typeof(SFMovieCreationParams)};
        object[] argVals = new object[] {this, creationParams};
        ConstructorInfo cInfo = MovieClassType.GetConstructor(argTypes);
        
        // Console.WriteLine(MovieClassType);
        // Console.WriteLine(cInfo);
        
        if (cInfo == null)
        {
            return null;
        }
        Movie movie = (Movie)cInfo.Invoke(argVals);
        return movie;
    }

    public Movie GetTopMovie()
    {
        if (SFMovieList != null && SFMovieList.Count != 0)
            return SFMovieList[0];
        return null;
    }

    public Movie GetBottomMovie()
    {
        if (SFMovieList != null)
        {
            int numMovies = SFMovieList.Count;
            if (numMovies != 0)
                return SFMovieList[numMovies - 1];
        }
        return null;
    }

    public int GetNumMovies()
    {
        if (SFMovieList != null)
        {
            return SFMovieList.Count;
        }
        return -1;
    }

    public void Update()
    {
        if (!IsSFInitialized())
        {
            return;
        }
		SF_ProcessMarkedForDeleteMovies();
		ReleaseMoviesMarkedForRelease();
		ReleaseValuesMarkedForRelease();
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            SFMovieList[i].Update();
        }
    }

    public bool DoHitTest(float x, float y)
    {
        if (!IsSFInitialized())
        {
            return false;
        }
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].DoHitTest(x, y))
            {
                return true; 
            }
        }
        return false;
    }

    public bool ReplaceTexture(long movieId, String textureName, int textureId, int RTWidth, int RTHeight)
    {
        return SF_ReplaceTexture(movieId, textureName, textureId, RTWidth, RTHeight);
    }

	public void ApplyLanguage(String langName)
	{
		SF_ApplyLanguage(langName);
	}
	
    public void Advance(float deltaTime)
    {
        if (!IsSFInitialized())
        {
            return;
        }

		// Check if viewport coordinates have changed
		int NewScreenWidth	= Screen.width;
		int NewScreenHeight	= Screen.height;
		int ox = 0;
		int oy = 0;
		#if UNITY_IPHONE
		oy = 0;
		#else
		oy = 24; // Note that while using D3D renderer, the tool bar (that contains "Maximize on Play" text) is part of 
		// the viewport, while using GL renderer, it's not. So there should be a further condition here depending on 
		// what type of renderer is being used, however I couldn't find a macro for achieving that. 
		#endif 

		if (ScreenWidth != NewScreenWidth || ScreenHeight != NewScreenHeight)
		{
			//UnityEngine.Debug.Log("ScreenWidth = " + NewScreenWidth + "ScreenHeight = " + NewScreenHeight);
			ScreenHeight = NewScreenHeight;
			ScreenWidth  = NewScreenWidth; 
			SF_SetNewViewport(ox, oy, ScreenWidth, ScreenHeight);
		}
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            SFMovieList[i].Advance(deltaTime);
        }
        if (LifecycleEventsList != null)
        {
            for (int i = 0; i < LifecycleEventsList.Count; i++) // Loop through List with for
            {
                LifecycleEventsList[i].Execute();
            }
        }
		ReleaseMoviesMarkedForRelease();
		ReleaseValuesMarkedForRelease();
    }
    
    public void Display()
    {
        if (!IsSFInitialized())
        {
            return;
        }
        
        SF_Display();
        // This indicates to Unity that render states might have changed and it can't assume anything about previous render states
        GL.InvalidateState();
    }
    
    public void InstallDelegates()
    {
        sf_eiDelegate = this.OnExternalInterface;
        SF_SetExternalInterfaceDelegate(sf_eiDelegate);
        
        sf_allocDelegate = new SF_AllocateDelegate(this.AllocateImpl);
        SF_SetAllocateValues(sf_allocDelegate);
        
        sf_logDelegate = new SF_LogDelegate(this.OnLogMessage);
        SF_SetLogDelegate(sf_logDelegate);
        
        sf_dInfoDelegate = new SF_DisplayInfoDelegate(this.AllocateDisplayInfo);
        SF_SetDisplayInfoDelegate(sf_dInfoDelegate);
    }

    public bool HandleMouseMoveEvent(float x, float y)
    {
        if (!IsSFInitialized())
        {
            return false;
        }
        
        int icase = 3;
#if UNITY_EDITOR
		y = y + 24; // Need to offset by height of title bar
#endif
        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleMouseEvent(x, y, icase))
            {
                handled = true;
            }
        }
        return handled;
    }

    public bool HandleMouseEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }
        
        int icase = 0;
        Vector2 mousePos = ev.mousePosition;
        switch (ev.type)
        {
            case EventType.MouseDown:
                icase = 1;
                break;
            case EventType.MouseUp:
                icase = 2;
                break;
            case EventType.MouseMove:
                icase = 3;
                break;
        }

#if UNITY_EDITOR
		mousePos[1] = mousePos[1] + 24; // Need to offset by height of title bar
#endif
        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleMouseEvent(mousePos[0], mousePos[1], icase))
            {
                handled = true;
            }
        }
        return handled;
    }
    
    public bool HandleTouchEvent(UnityEngine.Touch touch)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

		int icase = 0;
		switch (touch.phase)
		{
			case TouchPhase.Began:
				icase = 1; 
				break;
			case TouchPhase.Moved:
				icase = 2; 
				break;
			case TouchPhase.Ended:
				icase = 3; 
				break;				
		}
        bool handled = false;
		for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleTouchEvent(touch.fingerId, touch.position[0], touch.position[1], icase))
            {
                handled = true;
            }
        }
        return handled;
    }

    // Overload for handling keydown event from Gamepad
    public bool HandleKeyDownEvent(SFKey.Code code, SFKeyModifiers.Modifiers mod = 0, int keyboardIndex = 0)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(code, mod, 1, keyboardIndex))
            {
                handled = true;
            }
        }
        return handled;
    }

    // Overload for handling keyup event from Gamepad
    public bool HandleKeyUpEvent(SFKey.Code code, SFKeyModifiers.Modifiers mod = 0, int keyboardIndex = 0)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(code, mod, 0, keyboardIndex))
            {
                handled = true;
            }
        }

        return handled;
    }


    public bool HandleKeyDownEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        SFKey.Code cd = SFKey.GetSFKeyCode(ev.keyCode);
        SFKeyModifiers.Modifiers modifiers = SFKey.GetSFModifiers(ev.modifiers);
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(cd, modifiers, 1))
            {
                handled = true;
            }
        }
        return handled;
    }

    public bool HandleKeyUpEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        SFKey.Code cd = SFKey.GetSFKeyCode(ev.keyCode);
        SFKeyModifiers.Modifiers modifiers = SFKey.GetSFModifiers(ev.modifiers);
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleKeyEvent(cd, modifiers, 0))
            {
                handled = true;
            }
        }
        return handled;
    }

    public bool HandleCharEvent(UnityEngine.Event ev)
    {
        if (!IsSFInitialized())
        {
            return false;
        }

        bool handled = false;
        UInt32 wchar = ev.character;
        for (int i = 0; i < SFMovieList.Count; i++) // Loop through List with for
        {
            if (SFMovieList[i].HandleCharEvent(wchar))
            {
                handled = true;
            }
        }
        return handled;
    }

    private void PrintAddress(System.Object o)
    {
        GCHandle h      = GCHandle.Alloc(o, GCHandleType.Pinned);
        IntPtr addr     = h.AddrOfPinnedObject();
        Console.WriteLine(addr.ToString("x"));
        h.Free();
    } 
}

} // namespace Scaleform;
