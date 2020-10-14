using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class InputManagerArFoundation : InputManagerBaseClass
{
    #region Fields

    public ManoMotionFrame currentFrame;

    static int MinRezValue;
    static int MaxRezValue;

    private float inputFrameScale;

    private int maxCustomResolution = 300;
    
    private Texture2D frameTexture;
    private Color32[] pixelColors;

    private RenderTexture inputRenderTexture;

    [SerializeField]
    private ARCameraBackground arCameraBackground;

    private TextureFormat textureFormat;

    #endregion

    #region Initialization

    private void Awake()
    {
        ForceApplicationPermissions();

        inputFrameScale = GetInputScaleValue(Math.Max(Screen.width, Screen.height));
        
        MaxRezValue = (int)(Math.Max(Screen.width, Screen.height) * inputFrameScale);
        MinRezValue = (int)(Math.Min(Screen.width, Screen.height) * inputFrameScale);

        ManoUtils.OnOrientationChanged += HandleOrientationChanged;
    }

    private float GetInputScaleValue(int maxScreenValue)
    {
        float result;
        result = (float)maxCustomResolution / (float)maxScreenValue;
        result = Mathf.Round(result * 100f) / 100f;
        return result;
    }

    private void Start()
    {
        InitializeInputParameters();
    }

    /// <summary>
    /// Initializes the Input Parameters
    /// </summary>
    protected override void InitializeInputParameters()
    {
        textureFormat = TextureFormat.RGBA32;

        frameTexture = new Texture2D(MinRezValue, MaxRezValue, textureFormat, false);
        pixelColors = new Color32[MaxRezValue * MinRezValue];
        inputRenderTexture = new RenderTexture(MinRezValue, MaxRezValue, 0);

        RenderTexture.active = inputRenderTexture;
        currentFrame = new ManoMotionFrame();
        ResizeCurrentFrameTexture(inputRenderTexture.width, inputRenderTexture.height);

        if (OnFrameInitialized != null)
        {
            OnFrameInitialized(currentFrame);
        }

        if (OnAddonSet != null)
        {
            OnAddonSet(AddOn.ARFoundation);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Responds to a change in the orientation of a phone.
    /// It calles the general method to resize the RenderTexture(FrameClone) and the Texture2D(visual information).true
    /// </summary>
    void HandleOrientationChanged()
    {
        currentFrame.orientation = ManoUtils.Instance.currentOrientation;
        ResizeFrames();
    }

    /// <summary>
    /// Calls the main methods of resizing the RenderTexture (Camera Clone) and Texture2D (Visual information).
    /// Informs the subscribers of this event that the frames have been resized
    /// </summary>
    void ResizeFrames()
    {
        switch (ManoUtils.Instance.currentOrientation)
        {
            case DeviceOrientation.Unknown:
                ResizeInputRenderTexture(MinRezValue, MaxRezValue);
                break;
            case DeviceOrientation.Portrait:
                ResizeInputRenderTexture(MinRezValue, MaxRezValue);
                break;
            case DeviceOrientation.PortraitUpsideDown:
                ResizeInputRenderTexture(MinRezValue, MaxRezValue);
                break;
            case DeviceOrientation.LandscapeLeft:
                ResizeInputRenderTexture(MaxRezValue, MinRezValue);
                break;
            case DeviceOrientation.LandscapeRight:
                ResizeInputRenderTexture(MaxRezValue, MinRezValue);
                break;
        }

        ResizeCurrentFrameTexture(inputRenderTexture.width, inputRenderTexture.height);

        if (OnFrameResized != null)
        {
            OnFrameResized(currentFrame);
        }
    }

    /// <summary>
    /// Resizes the dimensions of the Render Texture that is used to get the image colors from ARFoundation.
    /// </summary>
    /// <param name="width">new Width Value</param>
    /// <param name="height">new Height Value</param>
    void ResizeInputRenderTexture(int width, int height)
    {
        if (inputRenderTexture != null)
        {
            inputRenderTexture.Release();
        }
        inputRenderTexture = new RenderTexture(width, height, 0);
    }

    /// <summary>
    /// Resizes the Texture2D information used in the ManoMotionFrame.
    /// Though this method should not happen very often, the garbage collector and resources unload are called to prevent a memory leak.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    new private void ResizeCurrentFrameTexture(int width, int height)
    {
        currentFrame.width = width;
        currentFrame.height = height;

        if (currentFrame.texture != null)
        {
            DestroyImmediate(currentFrame.texture);
        }

        currentFrame.texture = new Texture2D(width, height, textureFormat, false);
        currentFrame.texture.Apply();
        Resources.UnloadUnusedAssets();
        GC.Collect();
        currentFrame.pixels = currentFrame.texture.GetPixels32();
    }

    #endregion

    #region Update

    private void LateUpdate()
    {    
        UpdateFrame();
    }

    /// <summary>
    /// Updates the frame information at every frame by cloning the screen information.
    /// </summary>
    new private void UpdateFrame()
    {
        if (arCameraBackground.material == null)
        {
            Debug.LogError("arCameraBackground.material is NULL!");
            return;
        }

        Graphics.Blit(null, inputRenderTexture, arCameraBackground.material);

        if (currentFrame.texture.width != inputRenderTexture.width || currentFrame.texture.height != inputRenderTexture.height || currentFrame.texture == null)
        {
            DestroyImmediate(currentFrame.texture);
            currentFrame.texture = new Texture2D(inputRenderTexture.width, inputRenderTexture.height, textureFormat, false);

            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        currentFrame.texture.ReadPixels(new Rect(0, 0, inputRenderTexture.width, inputRenderTexture.height), 0, 0);
        currentFrame.pixels = currentFrame.texture.GetPixels32();

        if (OnFrameUpdated != null)
        {
            OnFrameUpdated(currentFrame);
        }
    }

    #endregion

    #region Application on Background

    /// <summary>
    /// Stop the ManoMotion processing when application is put to background mode.
    /// </summary>

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