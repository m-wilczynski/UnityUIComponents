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
        private IAutocompleteSourceProvider<T> _sourceProvider;

        /// <summary>
        /// Source from which elements for autocomplete will be provided
        /// </summary>
        public IAutocompleteSourceProvider<T> SourceProvider
        {
            get { return _sourceProvider; }
            set
            {
                if ((_sourceProvider != null && _sourceProvider.Equals(value)) || value == null)
                {
                    _autocompleteInput.interactable = false;
                    return;
                }
                _sourceProvider = value;
                _autocompleteInput.interactable = true;
            }
        }

        /// <summary>
        /// Currently selected item
        /// </summary>
        public T SelectedItem
        {
            get { return _selectedItem; }
        }

        protected virtual void Start()
        {
            ValidateUI();
            BuildRoot();
            StartCoroutine(PopulateAutocompleteResultMap());
            BindInputField();
            _autocompleteInput.interactable = false;
        }

        protected virtual void ValidateUI()
        {
            if (_autocompleteInput == null)
                throw new InvalidOperationException("_autocompleteInput is not set");
            if (_selectedItemText == null)
                throw new InvalidOperationException("_selectedItemText not set");
        }

        protected virtual AutocompleteResultListElement CreateListElementView()
        {
            var go = new GameObject("ResultElement");
            go.AddComponent<Text>();
            return go.AddComponent<AutocompleteResultListElement>();
        }

        private IEnumerator PopulateAutocompleteResultMap()
        {
            yield return new WaitForEndOfFrame();
            ResultViewsMap = new AutocompleteResultListElement[_maxItemsToShow];
            Results = new T[_maxItemsToShow];

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
            var results = SourceProvider.Find(input);
            PopulateWithResults(results);
        }

        private void OnSelectedItem(int index)
        {
            _selectedItem = Results[index];
            _selectedItemText.text = SourceProvider.LabelTextFor(_selectedItem);
            Results = new T[_maxItemsToShow];
        }

        private void PopulateWithResults(IEnumerable<T> results)
        {
            var resultsToShow = results.Take(_maxItemsToShow).ToArray();
            int counter = 0;

            foreach (var view in ResultViewsMap)
            {
                if (counter >= _maxItemsToShow)
                {
                    view.Hide();
                }
                else
                {
                    view.SwapItem(_sourceProvider.LabelTextFor(resultsToShow[counter]));
                    Results[counter] = resultsToShow[counter];
                    view.Show();
                }
                counter++;
            }
        }
    }
}
