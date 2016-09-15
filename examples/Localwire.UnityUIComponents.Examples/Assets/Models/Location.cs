namespace Assets.Models
{
    using System;

    [Serializable]
    public class LocationList
    {
        public Location[] Locations;
    }

    [Serializable]
    public class Location
    {
        public string City;
        public string Country;
        public string CountryCode;
    }
}
