/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Globalization;
using UnityEngine.Video;

public class RemoteTransformations : MonoBehaviour
{
    [HideInInspector]
    public Entry entry;
    public bool usesPosition = true;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and transforms the object based on the result.
    /// </summary>

    // Initial transformation
    private Vector3 initialWorldSpacePosition;
    private Quaternion initialWorldSpaceRotation;
    private Vector3 initialScale;

    // Use this for initialization
    void Start()
    {
        try
        {
            // Set initial transfomation
            initialWorldSpacePosition = (this.gameObject.transform.parent) ? this.gameObject.transform.parent.transform.position : this.gameObject.transform.position;
            initialWorldSpaceRotation = this.gameObject.transform.rotation;
            initialScale = this.gameObject.transform.localScale;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    }

    public void Update()
    {
        string value = "";
        float positionFactor = 1f;

        // Attach to parent
        if (this.gameObject.transform.parent) {
            initialWorldSpacePosition =  this.gameObject.transform.parent.transform.position;
            positionFactor = this.gameObject.transform.parent.transform.localScale.magnitude;
        }

        // Handle translation
        // For this particular app, this interferes with continous movement, so I made it conditional
        if (usesPosition) {
            Vector3 positionOffest = Vector3.zero;
            if (entry.getAdditionalData().TryGetValue("x", out value))
            {
                positionOffest.x = float.Parse(value, CultureInfo.InvariantCulture);
            }
            if (entry.getAdditionalData().TryGetValue("y", out value))
            {
                positionOffest.y = float.Parse(value, CultureInfo.InvariantCulture);
            }
            if (entry.getAdditionalData().TryGetValue("z", out value))
            {
                positionOffest.z = float.Parse(value, CultureInfo.InvariantCulture);
            }
            this.gameObject.transform.position = initialWorldSpacePosition + positionOffest * positionFactor;
        }

        // Handle spinning
        float speed = 150;
        if (entry.getAdditionalData().TryGetValue("speed", out value))
        {
            speed *= float.Parse(value, CultureInfo.InvariantCulture);
        }
        float offset = 0;
        if (entry.getAdditionalData().TryGetValue("direction", out value))
        {
            if (value.Equals("right"))
                offset += Time.time % 360 * speed;
            else
                offset -= Time.time % 360 * speed;
        }

        // Handle rotation
        Quaternion targetQuaternion = initialWorldSpaceRotation;
        float x = 0, y = 0, z = 0;
        
        if (entry.getAdditionalData().TryGetValue("xAngle", out value))
        {
            x = float.Parse(value, CultureInfo.InvariantCulture);

        }
        if (entry.getAdditionalData().TryGetValue("yAngle", out value))
        {
            y = float.Parse(value, CultureInfo.InvariantCulture);
        }
        if (entry.getAdditionalData().TryGetValue("zAngle", out value))
        {
            z = float.Parse(value, CultureInfo.InvariantCulture);
        }
        this.gameObject.transform.rotation = Quaternion.Euler(x, y + offset, z);

        // Handle Scale
        float scaleFactor = 1f;
        if (entry.getAdditionalData().TryGetValue("scale", out value))
        {
            scaleFactor = float.Parse(value, CultureInfo.InvariantCulture);
        }
        this.gameObject.transform.localScale = initialScale * scaleFactor;

        // Mute
         bool mute = false;
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("mute", out value)) {
            mute = value.Equals("true") ? true : false;
        }
        VideoPlayer videoPlayer = this.GetComponent<VideoPlayer>();
        for (ushort i = 0; videoPlayer != null && i < videoPlayer.controlledAudioTrackCount; ++i)
            videoPlayer.SetDirectAudioMute(i, mute);
    }
}