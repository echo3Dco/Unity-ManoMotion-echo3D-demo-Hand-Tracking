using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// Warnings are a list of messages that the SDK is providing in order to prevent a situation where the hand will be not clearly detected.
/// </summary>
public enum Warning
{
    NO_WARNING = 0,
    WARNING_HAND_NOT_FOUND = 1,
    WARNING_APPROACHING_UPPER_EDGE = 4,
    WARNING_APPROACHING_LEFT_EDGE = 5,
    WARNING_APPROACHING_RIGHT_EDGE = 6,
};

/// <summary>
/// The Hand value provides additional information regarding the classification of the hand regarding if its a right or a left hand
/// </summary>
public enum Hand
{
    LEFT = 0,
    RIGHT = 1,
};

/// <summary>
/// Contrains information about the hand
/// </summary>
public struct HandInfo
{
    /// <summary>
    /// Information about position
    /// </summary>
    public TrackingInfo tracking_info;

    /// <summary>
    /// Information about gesture
    /// </summary>
    public GestureInfo gesture_info;

    /// <summary>
    /// Warnings of conditions that could mean problems on detection
    /// </summary>
    public Warning warning;
}