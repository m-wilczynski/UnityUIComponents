# UnityUIComponents
Custom UI components for Unity3D game engine. Intended to fill the gap of some missing, yet useful UI components.

## Getting started

You have two options to use UnityUIComponents in your project:
* download repository, launch **/src/Localwire.UnityUIComponents.sln** and build project in **Release** mode, then copy `Localwire.UnityUIComponents.dll` over to your project's Assets folder
* download repository, navigate to examples in **/examples/Localwire.UnityUIComponents.Examples/Assets/DLLs/** and copy over `Localwire.UnityUIComponents.dll` to your project's Assets folder

## Available components

### Autocomplete Textbox


Similiar to its web equivalents, **Autocomplete Textbox** listens to user input in `InputField` and after short delay (defined by user), queries provided source of `IAutocompleteSource<T>` with input as criteria and prints results out to UI as `AutocompleteResultListElement`.
Unlike most of built-in Unity3D components, selected item will not only be shown as string in `Text` component but also provided as actual, strongly-typed item of type `T` through `AutocompleteTextboxView.SelectedItem` property.

<br/>
**Example:**

[![https://gyazo.com/66c4beeae56d77b9e0c4f658d8737167](https://i.gyazo.com/66c4beeae56d77b9e0c4f658d8737167.gif)](https://gyazo.com/66c4beeae56d77b9e0c4f658d8737167)<br/>
**Quickstart:**

There are always two steps required for setting up this component: 
* defining source for autocomplete (`IAutocompleteSource<T>`)
* defining component itself (`AutocompleteTextboxView<T>`).

Let's think of a situation when we want to query some source of locations, defined as
```cs
public class Location
{
    public string City;
    public string Country;
    public string CountryCode;
}
```

from a JSON file. Let's name it `cities_testdata.json` and put it in our **Assets\Resources\** folder.

Now that we have file we want to work with, let's define autocomplete source for querying by creating our implementation of `IAutocompleteSource<T>`:
```cs
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

[Serializable]
public class LocationList
{
    public Location[] Locations;
}
```

Now that we have source to query upon, let's create a component that we will use for autocomplete:
```cs
public class LocationsAutocomplete : AutocompleteTextboxView<Location>
{
    protected override void Start()
    {
        base.Start();
        SourceProvider = new JsonLocationsSource();
    }
}
```

And that's it. Now use **Add Component...** in Unity3D Editor to add `LocationsAutocomplete` component, drag and drop `InputField` you want to use to components inspector and (optionally) `Text` to display currently display item.

Code above is part of `UnityUIComponents.Examples` solution that you can find in **/examples** directory of this project.
