/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
[System.Serializable]
public class ImageHologram : Hologram
{

    private string filename;
    private string storageID;
    private string compressedImageStorageID;

    public ImageHologram() : base()
    {
        setType(hologramType.IMAGE_HOLOGRAM);
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

    public string getCompressedImageStorageID()
    {
        return this.compressedImageStorageID;
    }

    public void setCompressedImageStorageID(string compressedImageStorageID)
    {
        this.compressedImageStorageID = compressedImageStorageID;
    }

}