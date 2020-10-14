using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class ManoUtils : MonoBehaviour
{
    #region Singleton
    private static ManoUtils instance;
    public static ManoUtils Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    public static Action OnOrientationChanged;
    public DeviceOrientation currentOrientation;

    private Vector3 correction_ratio = Vector3.one;

    protected void Awake()
    {
        if (instance == null)
            instance = this;
        if (!cam)
            cam = Camera.main;
    }

    [SerializeField]
    private Camera cam;

    /// <summary>
    /// Calculates the new position in relation to the main camera.
    /// </summary>
    /// <param name="Point">Requires a Vector3 point</param>
    /// <param name="depth">Requires the float value of depth</param>
    /// <returns></returns>
    internal Vector3 CalculateNewPosition(Vector3 Point, float depth)
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        Vector3 correct_point = Point - Vector3.one * 0.5f;
        correct_point.Scale(correction_ratio);
        correct_point = correct_point + Vector3.one * 0.5f;
        correct_point = new Vector3(Mathf.Clamp(correct_point.x, 0, 1), Mathf.Clamp(correct_point.y, 0, 1), Mathf.Clamp(correct_point.z, 0, 1));

        return cam.ViewportToWorldPoint(correct_point + Vector3.forward * depth);
    }

    /// <summary>
    /// Adjust the transform in the received mesh renderer to fit the screen without stretching
    /// </summary>
    /// <param name="mesh_renderer"></param>
    internal void AjustBorders(MeshRenderer mesh_renderer, Session session)
    {
        float ratio = CalculateRatio(mesh_renderer, session);
        float size = CalculateSize(mesh_renderer, session, ratio);
        AdjustMeshScale(mesh_renderer, session, ratio, size);
        CalculateCorrectionPoint(mesh_renderer, session, ratio, size);
    }

    /// <summary>
    /// Calculates the ratio for the Mesh Renderer according to the phone orientation
    /// </summary>
    /// <param name="mesh_renderer"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    internal float CalculateRatio(MeshRenderer mesh_renderer, Session session)
    {
        float ratio = 1;
        switch (session.orientation)
        {
            case DeviceOrientation.Portrait:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case DeviceOrientation.PortraitUpsideDown:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
            case DeviceOrientation.LandscapeLeft:
                ratio = (float)ManomotionManager.Instance.Width / ManomotionManager.Instance.Height;
                break;
            case DeviceOrientation.LandscapeRight:
                ratio = (float)ManomotionManager.Instance.Width / ManomotionManager.Instance.Height;
                break;
            default:
                ratio = (float)ManomotionManager.Instance.Height / ManomotionManager.Instance.Width;
                break;
        }
        return ratio;
    }

    internal float CalculateSize(MeshRenderer mesh_renderer, Session session, float ratio)
    {
        if (!cam)
            cam = Camera.main;
        float size = 1;
        float height = 2.0f * Mathf.Tan(0.5f * cam.fieldOfView * Mathf.Deg2Rad) * mesh_renderer.transform.localPosition.z;
        float width;

        switch (session.orientation)
        {
            case DeviceOrientation.Portrait:
                size = height;
                break;
            case DeviceOrientation.PortraitUpsideDown:
                size = height;
                break;
            case DeviceOrientation.LandscapeLeft:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
            case DeviceOrientation.LandscapeRight:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
            default:
                width = height * Screen.width / Screen.height;
                size = width / ratio;
                break;
        }

        return size;
    }

    internal void AdjustMeshScale(MeshRenderer mesh_renderer, Session session, float ratio, float size)
    {
        switch (session.orientation)
        {
            case DeviceOrientation.Portrait:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case DeviceOrientation.PortraitUpsideDown:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
            case DeviceOrientation.LandscapeLeft:
                mesh_renderer.transform.localScale = new Vector3(size * ratio, size, 0f);
                break;
            case DeviceOrientation.LandscapeRight:
                mesh_renderer.transform.localScale = new Vector3(size * ratio, size, 0f);
                break;
            default:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
        }
    }

    internal void CalculateCorrectionPoint(MeshRenderer mesh_renderer, Session session, float ratio, float size)
    {
        Vector3 screen_ratio;
        Vector3 image_ratio;

        switch (session.orientation)
        {
            case DeviceOrientation.Portrait:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case DeviceOrientation.PortraitUpsideDown:
                screen_ratio = new Vector3(((float)Screen.height / Screen.width), 1, 1);
                image_ratio = new Vector3(ratio, 1, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case DeviceOrientation.LandscapeLeft:
                screen_ratio = new Vector3(1, 1 / ((float)Screen.height / Screen.width), 1);
                image_ratio = new Vector3(1, 1 / ratio, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            case DeviceOrientation.LandscapeRight:
                screen_ratio = new Vector3(1, 1 / ((float)Screen.height / Screen.width), 1);
                image_ratio = new Vector3(1, 1 / ratio, 1);
                correction_ratio = Vector3.Scale(screen_ratio, image_ratio);
                break;
            default:
                mesh_renderer.transform.localScale = new Vector3(size, size * ratio, 0f);
                break;
        }
    }

    private void Start()
    {
        currentOrientation = DeviceOrientation.Portrait;
        if (OnOrientationChanged != null)
        {
            OnOrientationChanged();
        }
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
        if (Input.deviceOrientation != DeviceOrientation.FaceDown && Input.deviceOrientation != DeviceOrientation.FaceUp && Input.deviceOrientation != DeviceOrientation.Unknown)
        {
            if (currentOrientation != Input.deviceOrientation)
            {
                currentOrientation = Input.deviceOrientation;

                if (OnOrientationChanged != null)
                {
                    OnOrientationChanged();
                }
            }
        }
    }

    /// <summary>
    /// Retrieve the absolute values of a Vector3
    /// </summary>
    /// <returns>The abs.</returns>
    /// <param name="vector">Requires a Vector3 value.</param>
    Vector3 VectorAbs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    /// <summary>
    /// Properly orients a MeshRenderer in order to be displayed properly
    /// </summary>
    /// <param name="meshRenderer">Mesh renderer.</param>
    public void OrientMeshRenderer(MeshRenderer meshRenderer)
    {
        if (ManomotionManager.Instance.Manomotion_Session.add_on == AddOn.DEFAULT)
        {
            switch (ManomotionManager.Instance.Manomotion_Session.orientation)
            {

                case DeviceOrientation.Portrait:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                case DeviceOrientation.PortraitUpsideDown:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    break;
                case DeviceOrientation.LandscapeLeft:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                case DeviceOrientation.LandscapeRight:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    break;
                case DeviceOrientation.Unknown:
                    meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
                default:
                    break;
            }
        }
        else
        {
            meshRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
