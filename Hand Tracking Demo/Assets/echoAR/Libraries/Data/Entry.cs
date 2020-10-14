/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entry
{
    // Entry properties
    private string id;
    private Target target;
    private Hologram hologram;
    private bool[] sdks;
    private Dictionary<string, string> additionalData;

    /**
     * Entry constructor
     */
    public Entry() : this(null, null)
    {
        
    }

    /**
     * Entry constructor
     * @param target
     * @param hologram
     */
    public Entry(Target target, Hologram hologram)
    {
        // Initialize
        this.target = target;
        this.hologram = hologram;
        this.sdks = new bool[System.Enum.GetNames(typeof(SDKs)).Length];
        this.additionalData = new Dictionary<string, string>();
    }

    /**
     * ID getter
     * @return The entry's ID
     */
    public string getId()
    {
        return id;
    }

    /**
     * ID setter
     * @param id An ID
     */
    public void setId(string id)
    {
        this.id = id;
    }

    /**
     * Target getter
     * @return The entry's target
     */
    public Target getTarget()
    {
        return target;
    }

    /**
     * Target setter
     * @param target
     */
    public void setTarget(Target target)
    {
        this.target = target;
    }

    /**
     * Hologram getter
     * @return The entry's hologram
     */
    public Hologram getHologram()
    {
        return hologram;
    }

    /**
     * Hologram setter
     * @param hologram
     */
    public void setHologram(Hologram hologram)
    {
        this.hologram = hologram;
    }

    /**
     * Supported SDKs getter
     * @return A boolean array that flags the supported SDKs
     */
    public bool[] getSupportedSDKs()
    {
        return sdks;
    }

    /**
     * Supported SDKs setter
     * @param sdks A boolean array that flags the supported SDKs
     */
    public void setSupportedSDKs(bool[] sdks)
    {
        this.sdks = sdks;
    }

    /**
     * Supported SDKs setter
     * @param sdk An SDK option
     * @param supported A flag indicating if the given SDK should be supported
     */
    public void setSupportedSDK(SDKs sdk, bool supported)
    {
        this.sdks[(int)sdk] = supported;
    }

    /**
     * Get a map of the collection's additional data
     * @return A map of the collection's additional data
     */
    public Dictionary<string, string> getAdditionalData()
    {
        return additionalData;
    }

    /**
     * Add additional data to this entry
     * @param key
     * @param value
     */
    public void addAdditionalData(string key, string value)
    {
        this.additionalData.Add(key, value);
    }

    /**
     * Delete additional data for this entry
     * @param key
     */
    public void removeAdditionalData(string key)
    {
        this.additionalData.Remove(key);
    }
    // SDK options
    public enum SDKs
    {
        VUFORIA,
        ARCORE,
        ARKIT,
        UNITY,
        EASYAR,
        WIKITUDE,
        KUDAN,
        WEBXR,
        ARJS
    }
}