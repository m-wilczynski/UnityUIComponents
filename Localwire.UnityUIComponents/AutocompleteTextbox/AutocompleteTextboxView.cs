namespace Localwire.UnityUIComponents.AutocompleteTextbox
{
    using System;
    using SourceProvider;
    using Subelements;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(InputField))]
    public abstract class AutocompleteTextboxView<T> : MonoBehaviour
    {
        //Inspector fields
        [Range(1, 10)]
        [SerializeField]
        private int _maxItemsToShow = 5;

        [Range(1, 1000)]
        [SerializeField]
        private float _width;

        [SerializeField]
        private IAutocompleteSourceProvider<T> _sourceProvider;
        [SerializeField]
        private InputField _autocompleteInput;
        //

        protected AutocompleteResultListElement[] ResultViewsMap;
        protected T[] Results;
        private Transform _resultsRoot;
        private T _selectedItem;
        
        public T SelectedItem
        {
            get { return _selectedItem; }
        }

        protected virtual void Start()
        {
            Validate();
            PopulateAutocompleteResultMap();
        }

        protected virtual void Validate()
        {
            if (_sourceProvider == null)
                throw new InvalidOperationException("_sourceProvider is not set");
            if (_autocompleteInput == null)
                throw new InvalidOperationException("_autocompleteInput is not set");
        }

        protected virtual AutocompleteResultListElement CreateListElementView()
        {
            var go = new GameObject("ResultElement");
            go.AddComponent<Text>();
            return go.AddComponent<AutocompleteResultListElement>();
        }

        private void PopulateAutocompleteResultMap()
        {
            ResultViewsMap = new AutocompleteResultListElement[_maxItemsToShow];
            Results = new T[_maxItemsToShow];
            BuildRoot();

            for (var i = 0; i < _maxItemsToShow; i++)
            {
                ResultViewsMap[i] = CreateListElementView();
                ResultViewsMap[i].Bind(i, OnSelectedItem);
            }
        }

        private void BuildRoot()
        {
            var resultsRoot = new GameObject("ResultsRoot");
            resultsRoot.transform.SetParent(transform);
            _resultsRoot = resultsRoot.transform;
            resultsRoot.AddComponent<VerticalLayoutGroup>();
        }
        
        private void OnSelectedItem(int index)
        {
            _selectedItem = Results[index];
            Results = new T[_maxItemsToShow];
        }
    }
}
