using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGizmos : MonoBehaviour
{

    private GizmoManager _gizmoManager;

    private void Start()
    {
        _gizmoManager = GetComponent<GizmoManager>();
    }

    /// <summary>
    /// Toggles the boolean value of showing the hand states
    /// </summary>
    public void ToggleShowHandStates()
    {
        _gizmoManager.ShowHandStates = !_gizmoManager.ShowHandStates;
    }

    /// <summary>
    /// Toggles the boolean value of showing the manoclass
    /// </summary>
    public void ToggleShowManoclass()
    {
        _gizmoManager.ShowManoClass = !_gizmoManager.ShowManoClass;
    }

    /// <summary>
    /// Toggles the boolean value of showing the cursor that follows the bounding box center;
    /// </summary>
    public void ToggleShowPalmCenter()
    {
        _gizmoManager.ShowPalmCenter = !_gizmoManager.ShowPalmCenter;
    }

    /// <summary>
    /// Toggles the boolean value of showing the cursor that follows the bounding box center;
    /// By toggling the value it will also updatre the manomotion session to start calculating it or not.
    /// </summary>
    public void ToggleShowPOI()
    {
        _gizmoManager.ShowPOI = !_gizmoManager.ShowPOI;
        ManomotionManager.Instance.ShouldCalculatePOI(_gizmoManager.ShowPOI);
    }

    /// <summary>
    /// Toggles the boolean value of showing the handside of the detected hand;
    /// </summary>
    public void ToggleShowHandSide()
    {
        _gizmoManager.ShowHandSide = !_gizmoManager.ShowHandSide;
    }

    /// <summary>
    /// Toggles the boolean value of showing the continuous gesture of the detected hand;
    /// </summary>
    public void ToggleShowContinuousGestures()
    {
        _gizmoManager.ShowContinuousGestures = !_gizmoManager.ShowContinuousGestures;
    }

    /// <summary>
    /// Toggles the boolean value of showing Pick Trigger Gesture
    /// </summary>
    public void ToggleShowPickTriggerGesture()
    {
        _gizmoManager.ShowPickTriggerGesture = !_gizmoManager.ShowPickTriggerGesture;
    }

    /// <summary>
    /// Toggles the boolean value of showing Drop Trigger Gesture
    /// </summary>
    public void ToggleShowDropTriggerGesture()
    {
        _gizmoManager.ShowDropTriggerGesture = !_gizmoManager.ShowDropTriggerGesture;
    }

    /// <summary>
    /// Toggles the boolean value of showing Click Trigger Gesture
    /// </summary>
    public void ToggleShowClickTriggerGesture()
    {
        _gizmoManager.ShowClickTriggerGesture = !_gizmoManager.ShowClickTriggerGesture;
    }

    /// <summary>
    /// Toggles the boolean value of showing Grab Trigger Gesture
    /// </summary>
    public void ToggleShowGrabTriggerGesture()
    {
        _gizmoManager.ShowGrabTriggerGesture = !_gizmoManager.ShowGrabTriggerGesture;
    }

    /// <summary>
    /// Toggles the boolean value of showing Release Trigger Gesture
    /// </summary>
    public void ToggleShowReleaseTriggerGesture()
    {
        _gizmoManager.ShowReleaseTriggerGesture = !_gizmoManager.ShowReleaseTriggerGesture;
    }

    /// <summary>
    /// Toggles the show smoothing slider condition.
    /// </summary>
    public void ToggleShowSmoothingSlider()
    {
        _gizmoManager.ShowSmoothingSlider = !_gizmoManager.ShowSmoothingSlider;
    }

    /// <summary>
    /// Toggles the show warnings condition.
    /// </summary>
    public void ToggleShowWarnings()
    {
        _gizmoManager.ShowWarnings = !_gizmoManager.ShowWarnings;
    }

    /// <summary>
    /// Toggles the show depth estimation condition.
    /// </summary>
    public void ToggleShowDepthEstimation()
    {
        _gizmoManager.ShowDepthEstimation = !_gizmoManager.ShowDepthEstimation;
    }
}
