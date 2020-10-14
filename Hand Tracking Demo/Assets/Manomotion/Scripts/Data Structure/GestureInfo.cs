using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manoclass is the core block of the Gesture Classification. This value will be continuously updated based on the hand detection on a give frame.
/// </summary>
public enum ManoClass
{
    NO_HAND = -1,
    GRAB_GESTURE = 0,
    PINCH_GESTURE = 1,
    POINTER_GESTURE = 2
}
;

/// <summary>
/// The HandSide gives the information of which side of the hand is being detected with respect to the camera.
/// </summary>
public enum HandSide
{
    None = -1,
    Backside = 0,
    Palmside = 1
}
;

/// <summary>
/// Trigger Gestures are a type of Gesture Information retrieved for a given frame when the user perfoms the correct sequence of hand movements that matches to their action.
/// </summary>
public enum ManoGestureTrigger
{
    NO_GESTURE = -1,
    CLICK = 1,
    GRAB_GESTURE = 4,
    DROP = 8,
    PICK = 7,
    RELEASE_GESTURE = 3
}
;

/// <summary>
/// Similar to Manoclass Continuous Gestures are Gesture Information that is being updated on every frame according to the detection of the hand pose.
/// </summary>
public enum ManoGestureContinuous
{
    NO_GESTURE = -1,
    HOLD_GESTURE = 1,
    OPEN_HAND_GESTURE = 2,
    OPEN_PINCH_GESTURE = 3,
    CLOSED_HAND_GESTURE = 4,
    POINTER_GESTURE = 5,
}
;

/// <summary>
///  Information about the gesture performed by the user.
/// </summary>
public struct GestureInfo
{
    /// <summary>
    /// Class or gesture family.
    /// </summary>
    public ManoClass mano_class;

    /// <summary>
    /// Continuous gestures are those that are mantained throug multiple frames.
    /// </summary>
    public ManoGestureContinuous mano_gesture_continuous;

    /// <summary>
    /// Trigger gestures are those that happen in one frame.
    /// </summary>
    public ManoGestureTrigger mano_gesture_trigger;

    /// <summary>
    /// State is the information of the pose of the hand depending on each class
    /// The values go from 0 (most open) to 13 (most closed)
    /// </summary>
    public int state;

    /// <summary>
    /// Represents which side of the hand is seen by the camera.
    /// </summary>
    public HandSide hand_side;
}