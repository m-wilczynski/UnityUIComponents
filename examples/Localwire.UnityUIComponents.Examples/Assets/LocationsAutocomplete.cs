using Localwire.UnityUIComponents.AutocompleteTextbox;

namespace Assets.DLLs
{
    using UnityEngine;

    public class LocationsAutocomplete : AutocompleteTextboxView<Location>
    {
        protected override void Start()
        {
            base.Start();
            SourceProvider = new GameObject().AddComponent<JsonLocationsWebSource>();
        }
    }
}
