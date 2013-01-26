/**********************************************************************

Filename    :   RotatingCube.cs
Content     :   Inherits from MonoBehaviour
Created     :   
Authors     :   Ryan Holtz

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
using Scaleform.GFx;

public class RotatingCube : MonoBehaviour
{
    public GameObject RotatableCube = null;

    public void Start()
    {
        
    }

    public void Update()
    {
        if (RotatableCube != null)
        {
            Vector3 angles = RotatableCube.transform.eulerAngles;
            angles.x += 0.1f;
            angles.y += 0.333f;
    	    RotatableCube.transform.eulerAngles = angles;
        }
    }
}