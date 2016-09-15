namespace Assets
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Localwire.UnityUIComponents.AutocompleteTextbox.SourceProvider;
    using Models;
    using UnityEngine;

    public class JsonLocationsSource : IAutocompleteSourceProvider<Location>
    {
        private Location[] _locations;

        public string LabelTextFor(Location element)
        {
            return element.City + "[" + element.CountryCode + "]";
        }

        public IEnumerable<Location> Find(string searchString)
        {
            LoadIfNeeded();
            return _locations.Where(loc => loc.City.Contains(searchString));
        }

        private void LoadIfNeeded()
        {
            if (_locations != null) return;
            using (var reader = new StreamReader("Assets/Resources/cities_testdata.json"))
            {
                var json = reader.ReadToEnd();
                _locations = JsonUtility.FromJson<LocationList>(json).Locations;
            }
        }
    }
}
