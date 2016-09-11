namespace Localwire.UnityUIComponents.AutocompleteTextbox.SourceProvider
{
    using System;
    using System.Collections.Generic;

    public interface IAutocompleteSourceProvider<T>
    {
        string LabelTextFor(T element);
        IEnumerable<T> Find(string searchString);
    }
}
