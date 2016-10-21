using UnityEngine;
using System.Collections;
using OpenCvSharp;
using System;
using System.Runtime.InteropServices;


public class OpencvSharpUtils : MonoBehaviour {

	public static void webCamTextureToMat (WebCamTexture mTexture, Mat mMat)
	{
		
		Color32[] colors = mTexture.GetPixels32 ();
		GCHandle colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);
		OpenCvUtils.webCamTextureToMat (mMat, colorsHandle.AddrOfPinnedObject ());
		colorsHandle.Free ();
	}

	public static Texture2D matToTexture2D (Mat mat , Texture2D texture2D)
	{

		Color32[] colors = new Color32[mat.Cols * mat.Rows];
		GCHandle colorsHandle = GCHandle.Alloc (colors, GCHandleType.Pinned);
		OpenCvUtils.matToTexture2D (colorsHandle.AddrOfPinnedObject (),mat);
		texture2D.SetPixels32 (colors);
		texture2D.Apply ();
		colorsHandle.Free ();
		return texture2D;

	}
}
