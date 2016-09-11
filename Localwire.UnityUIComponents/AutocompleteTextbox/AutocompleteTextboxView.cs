namespace Localwire.UnityUIComponents.AutocompleteTextbox
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using SourceProvider;
    using Subelements;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(InputField))]
    public abstract class AutocompleteTextboxView<T> : MonoBehaviour
    {
        //Inspector fields
        [Header("Minimum characters required to fire autocomplete")]
        [Range(2, 100)]
        [SerializeField]
        private int _minimumChars = 3;
        
        [Header("Maxmimum results of autocomplete to show")]
        [Range(1, 10)]
        [SerializeField]
        private int _maxItemsToShow = 5;

        [Header("Delay before autocomplete fires")]
        [Range(0, 5)]
        [SerializeField]
        private float _autocompleteDelay = 1f;

        [Header("Width of input field and result list elements")]
        [Range(1, 1000)]
        [SerializeField]
        private float _width = 250;

        [Header("Source for results lookup")]
        [SerializeField]
        private IAutocompleteSourceProvider<T> _sourceProvider;

        [Header("Input field for autocomplete")]
        [SerializeField]
        private InputField _autocompleteInput;

        [Header("Text to show selected item")]
        [SerializeField]
        private Text _selectedItemText;
        //

        protected AutocompleteResultListElement[] ResultViewsMap;
        protected T[] Results;
        private Transform _resultsRoot;
        private T _selectedItem;
        private Coroutine _autocompleteDelayCoroutine;

        /// <summary>
        /// Currently selected item
        /// </summary>
        public T SelectedItem
        {
            get { return _selectedItem; }
        }

        protected virtual void Start()
        {
            Validate();
            PopulateAutocompleteResultMap();
            BindInputField();
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
                ResultViewsMap[i].transform.SetParent(_resultsRoot);
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

        private void BindInputField()
        {
            _autocompleteInput.onValueChanged.AddListener(input =>
            {
                if (input.Length < _minimumChars) return;
                if (_autocompleteDelayCoroutine != null)
                    StopCoroutine(_autocompleteDelayCoroutine);
                _autocompleteDelayCoroutine = StartCoroutine(OnInputFieldValueChanged(input));
            });
        }

        private IEnumerator OnInputFieldValueChanged(string input)
        {
            yield return new WaitForSeconds(_autocompleteDelay);
            var results = _sourceProvider.Find(input);
            PopulateWithResults(results);
        }

        private void OnSelectedItem(int index)
        {
            _selectedItem = Results[index];
            Results = new T[_maxItemsToShow];
        }

        private void PopulateWithResults(IEnumerable<T> results)
        {
            var resultsToShow = results.Take(_maxItemsToShow).ToArray();
            int counter = 0;

            foreach (var view in ResultViewsMap)
            {
                if (counter <= _maxItemsToShow)
                {
                    view.Hide();
                }
                else
                {
                    view.SwapItem(_sourceProvider.LabelNameSelector(resultsToShow[counter]));
                    view.Show();
                }
                counter++;
            }
        }
    }
}
