/**********************************************************************

Filename    :	SFInitParams.cs
Content     :	Inititialization parameters for Scaleform runtime
Created     :   
Authors     :   Ankur Mohan

Copyright   :   Copyright 2012 Autodesk, Inc. All Rights reserved.

Use of this software is subject to the terms of the Autodesk license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.
 
***********************************************************************/

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace Scaleform
{
// Need Serializable and class attributes to make InitParms visible in the editor.
[StructLayout(LayoutKind.Explicit, Pack=4)]
[Serializable]
public class SFInitParams
{

    // static int sizeInt = Marshal.SizeOf(typeof(int));
    public SFInitParams()
    {
        mSetFontCacheParams = true;
        mSetFontPackParams  = true;
        mInitIME            = false;
    }
    
    public enum ASVersion:int
    {
        AS2 = 0,
        AS3,
        Both
    }
    
    [FieldOffset(0)]
    public ASVersion mASVersion = ASVersion.AS3;
    [FieldOffset(4)]
    public bool mInitVideo = false;
    [FieldOffset(5)]
    public bool mInitSound = true;
    
    public enum VideoSoundSystem : int
    {
        SystemSound = 0,
        FMod,
        WWise,
        Default
    }
    
    [FieldOffset(8)]
    public VideoSoundSystem mVideoSoundSystem = VideoSoundSystem.SystemSound;
    
    public enum InitIME:int
    {
        Yes = 0,
        No
    }
    [FieldOffset(12)]
    public bool mInitIME = false;
    
    public enum EnableAmpProfiling : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(16)]
    public EnableAmpProfiling mEnableAmpProfiling;
    public enum EnableProgressiveLoading : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(20)]
    public EnableProgressiveLoading mProgLoading;
    
    [StructLayout(LayoutKind.Explicit, Pack=4)]
    [Serializable]
    public class FontCacheConfig
    {
		[FieldOffset(0)]
        public int TextureHeight    = 1024;
		[FieldOffset(4)]
        public int TextureWidth        = 512;
		[FieldOffset(8)]
        public int MaxNumTextures    = 1;
		[FieldOffset(12)]
        public int MaxSlotHeight    = 48;
    }
    
    [FieldOffset(24)]
    public FontCacheConfig mFontCacheConfig;
    [FieldOffset(40)]
    public bool mSetFontCacheParams = true;
    [FieldOffset(41)]
    public bool mEnableDynamicCache = true;

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public class FontPackParams
    {
        public int NominalSize    = 32;
        public int PadPixels    = 2;
        public int TextureWidth = 512;
        public int TextureHeight= 512;
    }
    
    [FieldOffset(44)]
    public FontPackParams mFontPackParams;
    [FieldOffset(60)]
    public bool mSetFontPackParams = true;
    [FieldOffset(64)]
    public int    mGlyphCountLimit = 1000;
    [FieldOffset(68)]
    public float SoundVolume = 10;
    [FieldOffset(72)]
    public bool IsMute = false;
    
    public void Print()
    {
        Console.WriteLine("ASVersion:");
        switch(mASVersion)
        {
            case ASVersion.AS2:
                Console.WriteLine("AS2");
                break;
            case ASVersion.AS3:
                Console.WriteLine("AS3");
                break;
            case ASVersion.Both:
                Console.WriteLine("Both");
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }
        
        // Console.Write("mSetFontPackParams = " + mSetFontPackParams);
        // Console.Write(mFontCacheConfig.TextureHeight); 
        
        // Complete the rest..
    }
}


// Need struct definition in order to prevent AOT compilation error on iOS
[StructLayout(LayoutKind.Explicit, Pack = 4)]
[Serializable]
public struct SFInitParams2
{
    public SFInitParams2(int dummy)
    {
        mASVersion              = ASVersion.AS3;
        mInitVideo              = false;
        mInitSound              = true;
        mVideoSoundSystem       = VideoSoundSystem.SystemSound;
        mInitIME                = false;
        mEnableAmpProfiling     = EnableAmpProfiling.Yes;
        mSetFontCacheParams     = true;
        mEnableDynamicCache     = true;
        mSetFontPackParams      = true;
        mInitIME                = false;
        mProgLoading            = EnableProgressiveLoading.Yes;
        
        mFontCacheConfig.TextureHeight      = 1024;
        mFontCacheConfig.TextureWidth       = 1024;
        mFontCacheConfig.MaxNumTextures     = 1;
        mFontCacheConfig.MaxSlotHeight      = 48;
        
        mFontPackParams.NominalSize     = 32;
        mFontPackParams.PadPixels       = 2;
        mFontPackParams.TextureWidth    = 512;
        mFontPackParams.TextureHeight   = 512;
        
        mSetFontPackParams      = true;
        mGlyphCountLimit        = 1000;
        SoundVolume             = 10;
        IsMute                  = false;
    }
    
    public SFInitParams2(SFInitParams initParams)
    {
        mASVersion    = (ASVersion)initParams.mASVersion;
        mInitVideo    = initParams.mInitVideo;
        mInitSound    = initParams.mInitSound;
        mVideoSoundSystem       = (VideoSoundSystem) initParams.mVideoSoundSystem;
        mInitIME                = initParams.mInitIME;
        mEnableAmpProfiling     = (EnableAmpProfiling) initParams.mEnableAmpProfiling;
        mSetFontCacheParams     = initParams.mSetFontCacheParams;
        mEnableDynamicCache     = initParams.mEnableDynamicCache;
        mSetFontPackParams      = initParams.mSetFontPackParams;
        mInitIME                = initParams.mInitIME;
        mProgLoading            = (EnableProgressiveLoading) initParams.mProgLoading;
        
        mFontCacheConfig.TextureHeight      = initParams.mFontCacheConfig.TextureHeight;
        mFontCacheConfig.TextureWidth       = initParams.mFontCacheConfig.TextureWidth;
        mFontCacheConfig.MaxNumTextures     = initParams.mFontCacheConfig.MaxNumTextures;
        mFontCacheConfig.MaxSlotHeight      = initParams.mFontCacheConfig.MaxSlotHeight;

        mFontPackParams.NominalSize         = initParams.mFontPackParams.NominalSize;
        mFontPackParams.PadPixels           = initParams.mFontPackParams.PadPixels;
        mFontPackParams.TextureWidth        = initParams.mFontPackParams.TextureWidth;
        mFontPackParams.TextureHeight       = initParams.mFontPackParams.TextureHeight;

        mSetFontPackParams      = initParams.mSetFontCacheParams;
        mGlyphCountLimit        = initParams.mGlyphCountLimit;
        SoundVolume             = initParams.SoundVolume;
        IsMute                  = initParams.IsMute;
    }
    
    public enum ASVersion : int
    {
        AS2 = 0,
        AS3,
        Both
    }
    
    [FieldOffset(0)]
    public ASVersion mASVersion;
    [FieldOffset(4)]
    public bool mInitVideo;
    [FieldOffset(5)]
    public bool mInitSound;

    public enum VideoSoundSystem : int
    {
        SystemSound = 0,
        FMod,
        WWise,
        Default
    }
    [FieldOffset(8)]
    public VideoSoundSystem mVideoSoundSystem;

    public enum InitIME : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(12)]
    public bool mInitIME;

    public enum EnableAmpProfiling : int
    {
        Yes = 0,
        No
    }
    
    [FieldOffset(16)]
    public EnableAmpProfiling mEnableAmpProfiling; // Unused.
    public enum EnableProgressiveLoading : int
    {
        Yes = 0,
        No
    }
    [FieldOffset(20)]
    public EnableProgressiveLoading mProgLoading;

	[StructLayout(LayoutKind.Explicit, Pack = 4)]
    [Serializable]
    public struct FontCacheConfig
    {
		[FieldOffset(0)]
        public int TextureHeight;
		[FieldOffset(4)]
        public int TextureWidth;
		[FieldOffset(8)]
        public int MaxNumTextures;
		[FieldOffset(12)]
        public int MaxSlotHeight;
    }
    
    [FieldOffset(24)]
    public FontCacheConfig mFontCacheConfig;
    [FieldOffset(40)]
    public bool mSetFontCacheParams;
    [FieldOffset(41)]
    public bool mEnableDynamicCache;

    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct FontPackParams
    {
        public int NominalSize;
        public int PadPixels;
        public int TextureWidth;
        public int TextureHeight;
    }
    
    [FieldOffset(44)]
    public FontPackParams mFontPackParams;
    [FieldOffset(60)]
    public bool mSetFontPackParams;
    [FieldOffset(64)]
    public int mGlyphCountLimit;
    [FieldOffset(68)]
    public float SoundVolume;
    [FieldOffset(72)]
    public bool IsMute;

    public void Print()
    {
        Console.WriteLine("ASVersion:");
        switch (mASVersion)
        {
            case ASVersion.AS2:
                Console.WriteLine("AS2");
                break;
            case ASVersion.AS3:
                Console.WriteLine("AS3");
                break;
            case ASVersion.Both:
                Console.WriteLine("Both");
                break;
            default:
                Console.WriteLine("Unknown");
                break;
        }

        // Console.Write("mSetFontPackParams = " + mSetFontPackParams);
        // Console.Write(mFontCacheConfig.TextureHeight);

        // Complete the rest..
    }
}
}