namespace Localwire.UnityUIComponents.AutocompleteTextbox.SourceProvider
{
    using System;
    using System.Collections.Generic;

    public interface IAutocompleteSourceProvider<T>
    {
        Func<T, string> LabelNameSelector { get; }
        IEnumerable<T> Find(string searchString);
    }
}
