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
        private float _autocompleteDelay = 0.5f;

        [Header("Width of input field and result list elements")]
        [Range(1, 1920)]
        [SerializeField]
        private float _width = 250;

        [Header("Padding of results list")]
        [SerializeField]
        private RectOffset _resultsPadding = new RectOffset(10, 10, 5, 5);

        [Header("Spacing between elements")]
        [Range(0, 1000)]
        [SerializeField] private int _resultsSpacing = 5;

        [Header("Input field for autocomplete")]
        [SerializeField]
        private InputField _autocompleteInput;

        [Header("Text to show selected item")]
        [SerializeField]
        private Text _selectedItemText;
        //

        private T _selectedItem;
        private Coroutine _autocompleteDelayCoroutine;
        private IAutocompleteSourceProvider<T> _sourceProvider;

        /// <summary>
        /// Results list elements map
        /// </summary>
        protected AutocompleteResultListElement[] ResultViewsMap { get; private set; }
        
        /// <summary>
        /// Results from current autocomplete lookup
        /// </summary>
        protected T[] Results { get; private set; }

        /// <summary>
        /// Parent for every result list view created
        /// </summary>
        protected Transform ResultsRoot { get; private set; }

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

        /// <summary>
        /// Setting up autocomplete textbox (from <see cref="MonoBehaviour"/>)
        /// </summary>
        protected virtual void Start()
        {
            ValidateUI();
            BuildRoot();
            StartCoroutine(PopulateAutocompleteResultMap());
            BindInputField();
            _autocompleteInput.interactable = false;
        }

        /// <summary>
        /// Validates UI assigned from inspector before setup
        /// </summary>
        protected virtual void ValidateUI()
        {
            if (_autocompleteInput == null)
                throw new InvalidOperationException("_autocompleteInput is not set");
            if (_selectedItemText == null)
                throw new InvalidOperationException("_selectedItemText not set");
        }

        /// <summary>
        /// Creates element of result list
        /// </summary>
        /// <returns>Empty <see cref="AutocompleteResultListElement"/> view</returns>
        protected virtual AutocompleteResultListElement CreateListElementView()
        {
            var go = new GameObject("ResultElement");
            var rect = go.AddComponent<RectTransform>();
            var text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            rect.sizeDelta = new Vector2(_width, rect.sizeDelta.y);
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
                ResultViewsMap[i].transform.SetParent(ResultsRoot);
                ResultViewsMap[i].Bind(i, OnSelectedItem);
            }
        }

        private void BuildRoot()
        {
            var resultsRoot = new GameObject("ResultsRoot");
            var rect = resultsRoot.AddComponent<RectTransform>();
            resultsRoot.transform.SetParent(_autocompleteInput.transform);
            var inputRect = _autocompleteInput.transform as RectTransform;

            resultsRoot.transform.localPosition = new Vector3(0, -inputRect.rect.height);
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.pivot = new Vector2(0.5f, 1);

            var layout = resultsRoot.AddComponent<VerticalLayoutGroup>();
            layout.padding = _resultsPadding;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.spacing = _resultsSpacing;
            rect.sizeDelta = new Vector2(_width, rect.sizeDelta.y);
            ResultsRoot = resultsRoot.transform;
        }

        private void BindInputField()
        {
            var rect = _autocompleteInput.transform as RectTransform;
            rect.sizeDelta = new Vector2(_width, rect.sizeDelta.y);
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
                if (counter >= resultsToShow.Length)
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
