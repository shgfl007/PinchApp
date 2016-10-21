using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp.Util;
using UnityEngine;
using System.Collections;
using OpenCvSharp;
using System.Runtime.InteropServices;

namespace OpenCvSharp
{
	public class OpenCvUtils
	{
		public static void webCamTextureToMat(Mat mat, IntPtr colors)
		{
			NativeMethods.webCamTextureToMat (mat.CvPtr, colors, mat.Cols* mat.Rows);
		}

		public static void matToTexture2D(IntPtr colors, Mat mat)
		{
			NativeMethods.matToTexture2D (colors, mat.CvPtr, mat.Cols * mat.Rows, mat.Type());
		}
	}
}

