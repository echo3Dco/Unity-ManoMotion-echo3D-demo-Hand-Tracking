using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconMainMenu : MonoBehaviour
{
    public delegate void OrientationChange();
    public static event OrientationChange OnOrientationChanged;

    public ScreenOrientation currentOrientation;

    private void Start()
    {
        currentOrientation = ScreenOrientation.Unknown;
    }

    void Update()
    {
        CheckForScreenOrientationChange();
    }

    /// <summary>
    /// Checks for changes on the orientation of the device.
    /// </summary>
    void CheckForScreenOrientationChange()
    {
        if (currentOrientation != Screen.orientation)
        {
            currentOrientation = Screen.orientation;
            OnOrientationChanged();
        }
    }

    void OnEnable()
    {
        if (OnOrientationChanged != null)
        {
            OnOrientationChanged();
        }
    }
}