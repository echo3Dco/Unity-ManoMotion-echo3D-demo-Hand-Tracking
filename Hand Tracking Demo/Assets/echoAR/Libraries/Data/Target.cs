/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections.Generic;

[System.Serializable]
public class Target
{
    public enum targetType { IMAGE_TARGET, GEOLOCATION_TARGET, BRICK_TARGET };

    private string id;
    private targetType type;
    private List<string> holograms;

    public Target()
    {
        holograms = new List<string>();
    }

    public string getId()
    {
        return id;
    }

    public void setId(string id)
    {
        this.id = id;
    }

    public targetType getType()
    {
        return type;
    }

    public void setType(targetType type)
    {
        this.type = type;
    }

    public List<string> getHolograms()
    {
        return holograms;
    }

    public void setHolograms(List<string> holograms)
    {
        this.holograms = holograms;
    }

    public void addHologram(Hologram hologram)
    {
        this.holograms.Add(hologram.getId());
    }

    public void addHolograms(List<Hologram> holograms)
    {
        foreach (Hologram hologram in holograms)
            addHologram(hologram);
    }

    public string toString()
    {
        return "Target{" +
                "id='" + id + '\'' +
                ", type=" + type +
                '}';
    }
}