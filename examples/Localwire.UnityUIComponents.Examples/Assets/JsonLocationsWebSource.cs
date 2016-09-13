namespace Assets
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Localwire.UnityUIComponents.AutocompleteTextbox.SourceProvider;
    using UnityEngine;

    public class JsonLocationsWebSource : MonoBehaviour, IAutocompleteSourceProvider<Location>
    {
        private Location[] _locations;

        void Start()
        {
            StartCoroutine(LoadIfNeeded());
        }

        public string LabelTextFor(Location element)
        {
            return element.City + "[" + element.CountryCode + "]";
        }

        public IEnumerable<Location> Find(string searchString)
        {
            return _locations != null ? _locations.Where(loc => loc.City.Contains(searchString)) : Enumerable.Empty<Location>();
        }

        private IEnumerator LoadIfNeeded()
        {
            if (_locations != null) yield break;
            //This will work only when hosted on GitHub
            var request =
                new WWW(
                    "https://m-wilczynski.github.io/UnityUIComponents/examples/Localwire.UnityUIComponents.Examples/Assets/Resources/cities_testdata.json");
            yield return request;
            _locations = JsonUtility.FromJson<LocationList>(request.text).Locations;
        }
    }
}
