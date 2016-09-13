# UnityUIComponents
Custom UI components for Unity3D game engine. Intended to fill the gap of some missing, yet useful UI components.

## Getting started

You have two options to use UnityUIComponents in your project:
* download repository, launch **/src/Localwire.UnityUIComponents.sln** and build project in **Release** mode, then copy `Localwire.UnityUIComponents.dll` over to your project's Assets folder
* download repository, navigate to examples in **/examples/Localwire.UnityUIComponents.Examples/Assets/DLLs/** and copy over `Localwire.UnityUIComponents.dll` to your project's Assets folder

## Available components

### Autocomplete Textbox
  **[Documentation](https://github.com/m-wilczynski/UnityUIComponents/wiki/Autocomplete-Textbox) | [Live Preview](https://m-wilczynski.github.io/UnityUIComponents/)**

<br/>
Similiar to its web equivalents, **Autocomplete Textbox** listens to user input in `InputField` and after short delay (defined by user), queries provided source of `IAutocompleteSource<T>` with input as criteria and prints results out to UI as `AutocompleteResultListElement`.
Unlike most of built-in Unity3D components, selected item will not only be shown as string in `Text` component but also provided as actual, strongly-typed item of type `T` through `AutocompleteTextboxView.SelectedItem` property.

<br/>
**Example:**

[![https://gyazo.com/66c4beeae56d77b9e0c4f658d8737167](https://i.gyazo.com/66c4beeae56d77b9e0c4f658d8737167.gif)](https://gyazo.com/66c4beeae56d77b9e0c4f658d8737167)<br/>
