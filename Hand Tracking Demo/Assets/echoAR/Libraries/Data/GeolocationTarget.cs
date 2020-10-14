/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
[System.Serializable]
public class GeolocationTarget : Target
{

    private string continent;
    private string country;
    private string city;
    private string place;
    private float latitude;
    private float longitude;

    public GeolocationTarget() : base()
    {
        setType(targetType.GEOLOCATION_TARGET);
    }

    public string getContinent()
    {
        return continent;
    }

    public void setContinent(string continent)
    {
        this.continent = continent;
    }

    public string getCountry()
    {
        return country;
    }

    public void setCountry(string country)
    {
        this.country = country;
    }

    public string getCity()
    {
        return city;
    }

    public void setCity(string city)
    {
        this.city = city;
    }

    public string getPlace()
    {
        return place;
    }

    public void setPlace(string place)
    {
        this.place = place;
    }

    public float getLatitude()
    {
        return latitude;
    }

    public void setLatitude(float latitude)
    {
        this.latitude = latitude;
    }

    public float getLongitude()
    {
        return longitude;
    }

    public void setLongitude(float longitude)
    {
        this.longitude = longitude;
    }
}