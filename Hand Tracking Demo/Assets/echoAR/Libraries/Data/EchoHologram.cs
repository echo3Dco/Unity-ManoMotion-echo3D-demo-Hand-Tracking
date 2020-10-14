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
public class EchoHologram : Hologram
{
    private string filename;
    private string textureFilename;
    private string encodedEcho;
    private List<string> vidoes;

    public EchoHologram() : base()
    {
        setType(hologramType.ECHO_HOLOGRAM);
    }

    public string getFilename()
    {
        return filename;
    }

    public void setFilename(string filename)
    {
        this.filename = filename;
    }

    public string getTextureFilename()
    {
        return textureFilename;
    }

    public void setTextureFilename(string textureFilename)
    {
        this.textureFilename = textureFilename;
    }

    public string getEncodedEcho()
    {
        return encodedEcho;
    }

    public void setEncodedEcho(string encodedEcho)
    {
        this.encodedEcho = encodedEcho;
    }

    public List<string> getVidoes()
    {
        return vidoes;
    }

    public void setVidoes(List<string> vidoes)
    {
        this.vidoes = vidoes;
    }
}