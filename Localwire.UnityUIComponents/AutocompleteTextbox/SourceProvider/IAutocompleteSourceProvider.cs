namespace Localwire.UnityUIComponents.AutocompleteTextbox.SourceProvider
{
    using System;
    using System.Collections.Generic;

    public interface IAutocompleteSourceProvider<T>
    {
        /// <summary>
        /// Selects name for label from element of type <see cref="T"/>
        /// </summary>
        /// <param name="element">Element for which to select name</param>
        /// <returns>Selected name</returns>
        string LabelTextFor(T element);

        /// <summary>
        /// Finds elements in source matching param criteria
        /// </summary>
        /// <param name="searchString">Search criteria</param>
        /// <returns>Elements matching criteria</returns>
        IEnumerable<T> Find(string searchString);
    }
}
