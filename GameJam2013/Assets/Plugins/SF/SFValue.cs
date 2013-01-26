/**********************************************************************

Filename    :	SFValue.cs
Content     :	Wrapper for Value
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

namespace Scaleform
{
namespace GFx
{

[StructLayout(LayoutKind.Sequential)]
public class SFCommand: System.Object
{
    public long       movieID;
    public int        argCount;
    public IntPtr     pmethodName;
}

public class SFDllName
{
    public static string DllName = "libgfxunity3d";
}

// For a class or pointer to class to be passed to unmanaged code, it must have
// StructLayout Attribute.
public partial class  Value: System.Object
{
    
    public enum ValueType: int
    {
        // ** Type identifiers
        // Basic types
        VT_Undefined        = 0x00,
        VT_Null             = 0x01,
        VT_Boolean          = 0x02,
        VT_Int              = 0x03,
        VT_UInt             = 0x04,
        VT_Number           = 0x05,
        VT_String           = 0x06,

        // StringW can be passed as an argument type, but it will only be returned 
        // if VT_ConvertStringW was specified, as it is not a native type.
        VT_StringW          = 0x07,  // wchar_t* string
        
        // ActionScript VM objects
        VT_Object           = 0x08,
        VT_Array            = 0x09,
        // Special type for stage instances (MovieClips, Buttons, TextFields)
        VT_DisplayObject    = 0x0a,

    }

    enum ValueTypeControl:int
    {
        // ** Special Bit Flags
        // Explicit coercion to type requested 
        VTC_ConvertBit = 0x80,
        // Flag that denotes managed resources
        VTC_ManagedBit = 0x40,
        // Set if value was referencing managed value and the owner Movie/VM
        // was destroyed.
        VTC_OrphanedBit = 0x20,

        // ** Type mask
        VTC_TypeMask = VTC_ConvertBit | 0x0F,
    };
    public IntPtr   	internalData;
    public ValueType    type;
	public long         MovieId;

    public Value()
    {
        MovieId            = 0;
        internalData    = IntPtr.Zero;
    }

    new public ValueType    GetType()       { return (ValueType)(((int)(type)) & ((int)(ValueTypeControl.VTC_TypeMask))); }
    
    // Check types
    public Boolean      IsUndefined()       { return GetType() == ValueType.VT_Undefined; }
    public Boolean      IsNull()            { return GetType() == ValueType.VT_Null; }
    public Boolean      IsBoolean()         { return GetType() == ValueType.VT_Boolean; }
    public Boolean      IsInt()             { return GetType() == ValueType.VT_Int; }
    public Boolean      IsUInt()            { return GetType() == ValueType.VT_UInt; }
    public Boolean      IsNumber()          { return GetType() == ValueType.VT_Number; }
    public Boolean      IsNumeric()         { return IsInt() || IsUInt() || IsNumber(); }
    public Boolean      IsString()          { return GetType() == ValueType.VT_String; }
    public Boolean      IsStringW()         { return GetType() == ValueType.VT_StringW; }
    public Boolean      IsObject()
    {
        return (GetType() == ValueType.VT_Object || GetType() == ValueType.VT_Array ||
                        GetType() == ValueType.VT_DisplayObject);
    }
    public Boolean        IsArray()         { return GetType() == ValueType.VT_Array; }
    public Boolean        IsDisplayObject() { return GetType() == ValueType.VT_DisplayObject; }
    
	// override equals
	public bool Equals(Value obj)
	{
	/*
	    Can't do this: 
		if (obj.MovieId == MovieId && obj.internalData == internalData && obj.type == type) return true;
		return false;
	    Need to compare the underlying GFx Data, which can only be done in C++.
	 */ 
		return SF_Equals(this, obj);
	}

    public Value(String sval, long movieID)
    {
        internalData    = SF_AllocateString(sval, movieID);
        type            = ValueType.VT_String;
        MovieId         = movieID;
    }
    public Value(Boolean bval, long movieID)
    {
        internalData    = SF_AllocateBoolean(bval, movieID);
        type            = ValueType.VT_Boolean;
        MovieId         = movieID;
    }
    public Value(Double nval, long movieID)
    {
        internalData    = SF_AllocateDouble(nval, movieID);
        type            = ValueType.VT_Number;
        MovieId         = movieID;
    }
    
    // Copy Constructor
    public Value(Value val)
    {
        if (val == null) 
        {
            return;
        }
        
        if (val.internalData != IntPtr.Zero)
        {
            internalData    = SF_CreateNewValue(val.internalData, val.MovieId);
        }
        else
        {
            internalData = val.internalData;
        }
        
        type = val.type;
        MovieId = val.MovieId;
    }

    public override string ToString()
    {
        if (IsBoolean())    return "Boolean: " + GetBool() + "\n";
        if (IsString())        return "String: " + GetString() + "\n";
        if (IsUInt())        return "UInt: " + GetUInt() + "\n";
        if (IsInt())        return "Int: " + GetInt() + "\n";
        if (IsNumber())        return "Number: " + GetNumber() + "\n";
        if (IsObject())        return "Object" + "\n";
        return "Unknown";
    }
    
    public double GetNumber()
    {
        return SF_GetNumber(this);
    }

    public UInt32 GetUInt()
    {
        return SF_GetUInt(this);
    }

    public Int32 GetInt()
    {
        return SF_GetInt(this);
    }

    public  Boolean GetBool()
    {
        return SF_GetBool(this);
    }

    public String GetString()
    {
        String str = Marshal.PtrToStringAnsi(SF_GetString(this));
        return str;
    }
			
    public Value GetObject()
    {
        int SFValueSize = Marshal.SizeOf(typeof(Value));
        IntPtr ptr2 = Marshal.AllocCoTaskMem(SFValueSize);
        SF_GetObject(this, ptr2);
        
        // This value will be garbage collected as well.
        Value val = SFManager.GetValueData(ptr2);
        Marshal.FreeCoTaskMem(ptr2);
        return val;
    }
    
    public bool SetMember(String elemName, Value val)
    {
        return SF_SetMember(this, elemName, val);
    }
    
	public bool SetMember(String elemName, String str)
    {
        return SF_SetMember(this, elemName, new Value(str, this.MovieId));
    }
			
	public bool SetMember(String elemName, Boolean bval)
    {
        return SF_SetMember(this, elemName, new Value(bval, this.MovieId));
    }
			
	public bool SetMember(String elemName, int num)
    {
        return SF_SetMember(this, elemName, new Value(num, this.MovieId));
    }
		
	public bool SetMember(String elemName, uint num)
    {
        return SF_SetMember(this, elemName, new Value(num, this.MovieId));
    }
	
	public bool SetMember(String elemName, double num)
    {
        return SF_SetMember(this, elemName, new Value(num, this.MovieId));
    }
			
    public bool GetMember(String elemName, ref Value dest)
    {
        return SF_GetMember(this, elemName, dest);
    }
			
	public Value GetMember(String elemName)
    {
		Value dest = new Value();
        bool res = SF_GetMember(this, elemName, dest);
		if (res)
		{
			return dest;
		}
		else
		{
			return null;
		}
    }
			 				
    public int GetArraySize()
    {
        if (!IsArray()) 
        {
            return -1;
        }
        return SF_GetArraySize(this);
    }

    public bool SetArraySize(UInt32 sz)
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_SetArraySize(this, sz);
    }

    public Value GetElement(UInt32 idx)
    {
		Value dest = new Value();
        if (!IsArray())
        {
			return null;
        }
        bool res = SF_GetElement(this, idx, dest);
		if (res)
		{
			return dest;
		}
		return null;
    }

    public bool SetElement(UInt32 idx, Value val)
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_SetElement(this, idx, val);
    }

    public bool RemoveElement(UInt32 idx)
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_RemoveElement(this, idx);
    }

    public bool ClearElements()
    {
        if (!IsArray())
        {
            return false;
        }
        return SF_ClearElements(this);
    }

    public SFDisplayInfo GetDisplayInfo()
    {
        SFDisplayInfo dinfo = new SFDisplayInfo();
        Int32 size          = Marshal.SizeOf(typeof(SFDisplayInfo));
        bool retVal         = SF_GetDisplayInfo(this, dinfo, size);
        return retVal ? dinfo : null;
    }
    
    public bool SetDisplayInfo(SFDisplayInfo dinfo)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        Int32 size = Marshal.SizeOf(typeof(SFDisplayInfo));
        return SF_SetDisplayInfo(this, dinfo, size);
    }


    public SFDisplayMatrix GetDisplayMatrix()
    {
        SFDisplayMatrix dmat = new SFDisplayMatrix();
        Int32 size = Marshal.SizeOf(typeof(SFDisplayMatrix));
        bool retVal = SF_GetDisplayMatrix(this, dmat, size);
        return retVal ? dmat : null;
    }

    public bool SetDisplayMatrix(SFDisplayMatrix dmat)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        Int32 size = Marshal.SizeOf(typeof(SFDisplayMatrix));
        return SF_SetDisplayMatrix(this, dmat, size);
    }
    
    public bool SetColorTransform(SFCxForm cxform)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_SetColorTransform(this,  cxform);
    }

    public bool GetColorTransform(ref SFCxForm cxform)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_GetColorTransform(this, cxform);
    }

    public bool SetText(String str)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_SetText(this, str);
    }

    public bool GetText(ref Value txt)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_GetText(this, txt);
    }

    public bool CreateEmptyMovieClip(ref Value dest, String instanceName, Int32 depth)
    {

        return SF_CreateEmptyMovieClip(this, dest, instanceName, depth);
    }
    
    public bool AttachMovie(ref Value dest, String symbolName, String instanceName, Int32 depth)
    {
        return SF_AttachMovie(this, dest, symbolName, instanceName, depth);
    }

	public bool RemoveMovieAS3(Value movieRemoved)
	{
		if (movieRemoved == null) return false;
		Value[] valArray = new Value[] { movieRemoved };
		Value retVal = new Value();
		return Invoke("removeChild", valArray, ref retVal);
	}

	public bool RemoveMovieAS2(Value movieRemoved)
	{
		if (movieRemoved == null) return false;
		
		Value retVal = new Value();
		return movieRemoved.Invoke("removeMovie", null, ref retVal);
		
	}

    public bool GotoAndPlayFrame(String frameName)
    {
        if (!IsDisplayObject()) 
        {
            return false;
        }
        return SF_GotoAndPlayFrame(this, frameName);
    }

    public bool GotoAndStopFrame(String frameName)
    {
        if (!IsDisplayObject())
        {
            return false;
        }
        return SF_GotoAndStopFrame(this, frameName);
    }

    public bool GotoAndPlay(Int32 frameNum)
    {
        if (!IsDisplayObject()) 
        {
            return false;
        }
        return SF_GotoAndPlay(this, frameNum);
    }

    public bool GotoAndStop(Int32 frameNum)
    {
        if (!IsDisplayObject()) 
        {
            return false;
        }
        return SF_GotoAndStop(this, frameNum);
    }

    public bool Invoke(String methodName, Value[] valArray, ref Value retVal)
    {

        int valueSize   = Marshal.SizeOf(typeof(Value));
        int IntPtrSize  = Marshal.SizeOf(typeof(IntPtr));
        int IntSize     = Marshal.SizeOf(typeof(int)); 	
		int numElem		= valArray.GetLength(0);
        
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
        
        bool result = SF_Invoke2(this, methodName, numElem, ptr, retVal);
        Marshal.FreeCoTaskMem(ptr);
        return result;
    
    }
    
    ~Value()
    {
        if (internalData != IntPtr.Zero)
        {
			SFManager.AddValueToReleaseList(internalData);
        //    SF_DecrementValRefCount(internalData);
			
        }
    }
}

} // namespace GFx;

} // namespace Scaleform;
 