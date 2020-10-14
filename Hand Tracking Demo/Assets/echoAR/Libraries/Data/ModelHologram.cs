/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System;
using System.Collections.Generic;

[System.Serializable]
public class ModelHologram : Hologram
{

    private string filename;
    private string storageID;
    private List<string> textureFilenames;
    private List<string> textureStorageIDs;
    private string materialFilename;
    private string materialStorageID;
    private string encodedFile;

    public ModelHologram() : base()
    {
        setType(hologramType.MODEL_HOLOGRAM);
    }

    public string getFilename()
    {
        return filename;
    }

    public void setFilename(string filename)
    {
        this.filename = filename;
    }

    public string getStorageID()
    {
        return storageID;
    }

    public void setStorageID(string storageID)
    {
        this.storageID = storageID;
    }

    public List<string> getTextureFilenames()
    {
        return textureFilenames;
    }

    public string getTextureFilename()
    {
        return (textureFilenames == null || textureFilenames.Count == 0) ? null : textureFilenames[0];
    }

    public string getTextureFilename(string textureStorageID)
    {
        return (textureFilenames == null || textureFilenames.Count == 0) ? null : textureFilenames[textureStorageIDs.IndexOf(textureStorageID)];
    }

    public void setTextureFilename(string textureFilename)
    {
        if (textureFilenames == null) textureFilenames = new List<string>();
        if (textureFilenames.Count == 0) textureFilenames.Add(textureFilename);
        else textureFilenames[0] = textureFilename;
    }

    public List<string> getTextureStorageIDs()
    {
        return textureStorageIDs;
    }

    public string getTextureStorageID()
    {
        return (textureStorageIDs == null || textureStorageIDs.Count == 0) ? null : textureStorageIDs[0];
    }

    public void setTextureStorageID(string textureStorageID)
    {
        if (textureStorageIDs == null) textureStorageIDs = new List<string>();
        if (textureStorageIDs.Count == 0) textureStorageIDs.Add(textureStorageID);
        else textureStorageIDs[0] = textureStorageID;
    }

    public void addTexture(string textureFilename, string textureStorageID)
    {
        if (textureFilenames == null) textureFilenames = new List<string>();
        if (textureStorageIDs == null) textureStorageIDs = new List<string>();
        textureFilenames.Add(textureFilename);
        textureStorageIDs.Add(textureStorageID);
    }

    public string getMaterialFilename()
    {
        return materialFilename;
    }

    public void setMaterialFilename(string materialFilename)
    {
        this.materialFilename = materialFilename;
    }

    public string getMaterialStorageID()
    {
        return materialStorageID;
    }

    public void setMaterialStorageID(string materialStorageID)
    {
        this.materialStorageID = materialStorageID;
    }

    public string getEncodedFile()
    {
        return encodedFile;
    }

    public void setEncodedFile(string encodedFile)
    {
        this.encodedFile = encodedFile;
    }
}
