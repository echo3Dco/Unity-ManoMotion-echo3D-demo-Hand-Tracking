using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManagerPhoneCamera : InputManagerBaseClass
{
	#region consts

	const int STARTING_WIDTH = 640;
	const int STARTING_HEIGHT = 480;

	#endregion


	#region  #Variables
	WebCamDevice[] webCamDevices;
	WebCamTexture backFacingCamera;
	public ManoMotionFrame currentManoMotionFrame;

	#endregion

	private void Awake()
	{
		ForceApplicationPermissions();
	}


	private void Start()
	{
		InitializeInputParameters();
		InitializeManoMotionFrame();
	}
	/// <summary>
	/// Initializes the parameters of the Device Camera.
	/// </summary>
	protected override void InitializeInputParameters()
	{
		webCamDevices = WebCamTexture.devices;


		if (webCamDevices.Length > 0)
		{

			backFacingCamera = new WebCamTexture(webCamDevices[0].name, STARTING_WIDTH, STARTING_HEIGHT);
			backFacingCamera.Play();
		}

		if (!backFacingCamera)
		{
			Debug.Log("Tried to create a camera but I could not");
		}

	}

	/// <summary>
	/// Initializes the ManoMotion Frame and lets the subscribers of the event know of its information.
	/// </summary>
	private void InitializeManoMotionFrame()
	{
		currentManoMotionFrame = new ManoMotionFrame();
		ResizeManoMotionFrameResolution(STARTING_WIDTH, STARTING_HEIGHT);
		currentManoMotionFrame.orientation = Input.deviceOrientation;

		if (OnFrameInitialized != null)
		{
			OnFrameInitialized(currentManoMotionFrame);

			Debug.Log("Initialized input parameters");
		}
		else
		{
			Debug.LogWarning("Noone is subscribed to OnFrameInitialized");
		}

        if (OnAddonSet != null)
        {
            OnAddonSet(AddOn.DEFAULT);

            Debug.Log("Initialized addon");
        }
        else
        {
            Debug.LogWarning("Noone is subscribed to OnFrameInitialized");
        }
    }


	/// <summary>
	/// Gets the camera frame pixel colors.
	/// </summary>
	protected void GetCameraFrameInformation()
	{
		if (!backFacingCamera)
		{
			Debug.LogError("No device camera available");
			return;
		}
		if (backFacingCamera.GetPixels32().Length < 300)
		{
			Debug.LogWarning("The frame from the camera is too small. Pixel array length:  " + backFacingCamera.GetPixels32().Length);
			return;
		}

		if (currentManoMotionFrame.pixels.Length != backFacingCamera.GetPixels32().Length)
		{
			ResizeManoMotionFrameResolution(backFacingCamera.width, backFacingCamera.height);
			return;
		}

		currentManoMotionFrame.pixels = backFacingCamera.GetPixels32();
		currentManoMotionFrame.texture.SetPixels32(backFacingCamera.GetPixels32());
		currentManoMotionFrame.texture.Apply();
		currentManoMotionFrame.orientation = Input.deviceOrientation;

		if (OnFrameUpdated != null)
		{
			OnFrameUpdated(currentManoMotionFrame);
		}
	}
	/// <summary>
	/// Sets the resolution of the currentManoMotion frame that is passed to the subscribers that want to make use of the input camera feed.
	/// </summary>
	/// <param name="newWidth">Requires a width value.</param>
	/// <param name="newHeight">Requires a height value.</param>
	protected void ResizeManoMotionFrameResolution(int newWidth, int newHeight)
	{
		currentManoMotionFrame.width = newWidth;
		currentManoMotionFrame.height = newHeight;
		currentManoMotionFrame.pixels = new Color32[newWidth * newHeight];
		currentManoMotionFrame.texture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, true);
		currentManoMotionFrame.texture.Apply();

		if (OnFrameResized != null)
		{
			OnFrameResized(currentManoMotionFrame);
		}

	}

	// Update is called once per frame
	void Update()
	{
		GetCameraFrameInformation();
	}


	private void OnEnable()
	{

		if (backFacingCamera)
		{
			Debug.Log("There is a backfacing Camera");
			if (!backFacingCamera.isPlaying)
			{
				backFacingCamera.Play();
				Debug.Log("Asked the camera to play");
			}
		}
		else
		{
			Debug.LogError("I dont have a backfacing Camera");

		}
	}

	private void OnDisable()
	{
		if (backFacingCamera && backFacingCamera.isPlaying)
		{
			backFacingCamera.Stop();
		}
	}

	#region Application on Background
	bool isPaused = false;

	void OnApplicationFocus(bool hasFocus)
	{
		isPaused = !hasFocus;
		if (isPaused)
		{
			ManomotionManager.Instance.StopProcessing();
		}

	}

	void OnApplicationPause(bool pauseStatus)
	{
		isPaused = pauseStatus;
		if (isPaused)
		{
			ManomotionManager.Instance.StopProcessing();
		}
	}
	#endregion
}
