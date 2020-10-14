using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ExampleDetectionCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statusText;

    [SerializeField]
    private GameObject Square;
    [SerializeField]
    private GameObject textDisplay;

    [SerializeField]
    private GameObject GizmoCanvas;

    [SerializeField]
    private ManoVisualization manoVisualization;

    private bool showBBStoredValue;

    void Start()
    {
        ARSession.stateChanged += HandleStateChanged;
        ToggleVisualizationValues.OnShowBoundingBoxValueChanged += HandleShowBoundingBoxValueChanged;
        showBBStoredValue = manoVisualization.Show_bounding_box;
    }

    void HandleShowBoundingBoxValueChanged(bool state)
    {
        showBBStoredValue = state;
    }

    void HandleStateChanged(ARSessionStateChangedEventArgs eventArg)
    {
        switch (eventArg.state)
        {
            case ARSessionState.None:
                statusText.text = "session status none";

                break;
            case ARSessionState.Unsupported:
                statusText.text = "ARFoundation not supported";

                break;
            case ARSessionState.CheckingAvailability:
                statusText.text = "Checking availability";

                break;
            case ARSessionState.NeedsInstall:
                statusText.text = "Needs Install";

                break;
            case ARSessionState.Installing:
                statusText.text = "Installing";

                break;
            case ARSessionState.Ready:
                statusText.text = "Ready";

                break;
            case ARSessionState.SessionInitializing:
                statusText.text = "Poor SLAM Quality";
                break;

            case ARSessionState.SessionTracking:
                statusText.text = "Tracking quality is Good";

                break;
            default:
                break;
        }

        textDisplay.SetActive(eventArg.state != ARSessionState.SessionTracking);
        Square.SetActive(eventArg.state != ARSessionState.SessionTracking);
        GizmoCanvas.SetActive(eventArg.state == ARSessionState.SessionTracking);
    }
}
