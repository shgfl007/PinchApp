using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp.Util;
using UnityEngine;
using System.Collections;
using OpenCvSharp;

#pragma warning disable 1591

namespace OpenCvSharp
{
	// TODO

	static partial class NativeMethods
	{
		[DllImport(DllSharpForUnity, CallingConvention = CallingConvention.Cdecl)]
		public static extern void matToTexture2D(IntPtr colors, IntPtr mat,int length, int type);
		[DllImport(DllSharpForUnity, CallingConvention = CallingConvention.Cdecl)]
		public static extern void webCamTextureToMat(IntPtr mat,IntPtr colors,int length);

	}
}