using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AddOn
{
	DEFAULT = 0,
	AR_KIT = 1,
	AR_CORE = 2,
	VUFORIA = 3,
	ARFoundation = 4
};

/// <summary>
/// Provides additional information regarding the image being sent to the SDK.
/// </summary>
public enum Flags
{
	FLAG_IMAGE_SIZE_IS_ZERO = 1000,
	FLAG_IMAGE_IS_TOO_SMALL = 1001
};

/// <summary>
/// The session used.
/// </summary>
public struct Session
{
	public Flags flag;
	public DeviceOrientation orientation;
	public AddOn add_on;
	public float smoothing_controller;
    public float gesture_smoothing_controller;
    public Features enabled_features;
}

/// <summary>
/// 1 for using it and 0 for not using it.
/// </summary>
public struct Features
{ 
	public int pinch_poi;
};