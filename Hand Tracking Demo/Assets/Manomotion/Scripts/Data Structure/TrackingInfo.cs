using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using System.Runtime.InteropServices;

/// <summary>
/// Contains information about position and tracking of the hand
/// </summary> 
[StructLayout(LayoutKind.Sequential)]
public struct TrackingInfo
{
    /// <summary>
    /// Provides tracking information regarding the bounding box that contains the hand.
    /// it yields normalized values between 0..1
    /// </summary>
    public BoundingBox bounding_box;

    /// <summary>
    /// Provides tracking information regarding the reference point for the Pinch class (POI). This information is primarily used in the cursor gizmo.
    /// it yields a normalized Vector3 information with the depth being 0 at the moment.
    /// a temporary solution would be to use the width of the bounding box as the depth Z value.
    /// </summary>
    public Vector3 poi;

    /// <summary>
    /// Provides tracking information regarding the bounding box center that contains the hand, this information is primarily used in the cursor gizmo.
    /// it yields a normalized Vector3 information with the depth being 0 at the moment.
    /// a temporary solution would be to combine it with the depth estimation value (see below).
    /// </summary>
    public Vector3 palm_center;

    /// <summary>
    /// Provides tracking information regarding an estimation  of the hand depth. 
    /// it yields a 0-1 float value depending based on the distance of the hand from the camera.
    /// </summary>
    public float depth_estimation;
}
