using System;
using UnityEngine;
using UnityEngine.Android;
public abstract class InputManagerBaseClass : MonoBehaviour
{
    public static Action<ManoMotionFrame> OnFrameUpdated;
    public static Action<ManoMotionFrame> OnFrameInitialized;
    public static Action<ManoMotionFrame> OnFrameResized;
    public static Action<DeviceOrientation> OnOrientationChanged;
    public static Action<AddOn> OnAddonSet;

    protected int rezMinValue;
    protected int rezMaxValue;

    protected virtual void ResizeCurrentFrameTexture(int width, int height) { }

    /// <summary>
    /// Initializes the values, such as width and height of the frame, in order to prepare a proper frame for ManoMotion tech to process.
    /// </summary>
    protected abstract void InitializeInputParameters();

    /// <summary>
    /// Forces the application to ask for camera permissions and external storage read and writte.
    /// </summary>
    public virtual void ForceApplicationPermissions()
    {
#if UNITY_ANDROID
        /* Since 2018.3, Unity doesn't automatically handle permissions on Android, so as soon as
            * the menu is displayed, ask for camera permissions. */

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Debug.Log("I dont have external write");
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Debug.Log("I dont have external read");
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            Debug.Log("I dont have camera permissions");
        }
#endif
    }

    public void StoragePermisionCheck()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Debug.Log("I dont have external write");
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Debug.Log("I dont have external read");
        }
    }

    protected virtual void UpdateFrame<T>(T parameter) { }
    protected virtual void UpdateFrame() { }
}
