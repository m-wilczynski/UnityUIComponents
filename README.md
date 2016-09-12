# UnityUIComponents
Custom UI components for Unity3D game engine. Intended to fill the gap of some missing, yet useful UI components.

## Getting started

You have two options to use UnityUIComponents in your project:
* download repository, launch `/src/Localwire.UnityUIComponents.sln` and build project in **Release** mode, then copy `Localwire.UnityUIComponents.dll` over from `/src/Localwire.UnityUIComponents/bin/Release` to your project's Assets folder
* download repository, navigate to examples in `/examples/Localwire.UnityUIComponents.Examples/Assets/DLLs/` and copy over `Localwire.UnityUIComponents.dll` to your project's Assets folder

## Available components

### Autocomplete Textbox
*Localwire.UnityUIComponents.AutcompleteTextbox.AutocompleteTextboxView*

Similiar to its web equivalents, **Autocomplete Textbox** listens to user input in `InputField` and after short delay (defined by user), queries provided source of `IAutocompleteSource<T>` with input as criteria and prints results out to UI as `AutocompleteResultListElement`.



