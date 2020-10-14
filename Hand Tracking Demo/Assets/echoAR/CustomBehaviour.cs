/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Entry entry;

    private static PlaceOnPlane placeOnPlane;
    private bool addedToPOP = false;
    private bool isVisible = false;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and names the object based on the result.
    /// </summary>

    // Use this for initialization
    void Start()
    {
        // Add RemoteTransformations script to object and set its entry
        RemoteTransformations remoteT = this.gameObject.AddComponent<RemoteTransformations>();
        remoteT.entry = entry;
        remoteT.usesPosition = false;

        // Query additional data to get the name
        string value = "";
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("name", out value))
        {
            // Set name
            this.gameObject.name = value;
        }
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
            renderer.enabled = isVisible;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (placeOnPlane == null) {
            placeOnPlane = GameObject.Find("AR Session Origin").GetComponent<PlaceOnPlane>();
        } else if (!addedToPOP) {
            placeOnPlane.AddModel(gameObject);
            addedToPOP = true;
        }

        // Update visibility
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
            renderer.enabled = isVisible;
        }
    }

    public void SetVisible(bool v) {
        isVisible = v;
    }
}