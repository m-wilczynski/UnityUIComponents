using Localwire.UnityUIComponents.AutocompleteTextbox;

namespace Assets.DLLs
{
    public class LocationsAutocomplete : AutocompleteTextboxView<Location>
    {
        protected override void Start()
        {
            base.Start();
            SourceProvider = new JsonLocationsSource();
        }
    }
}
