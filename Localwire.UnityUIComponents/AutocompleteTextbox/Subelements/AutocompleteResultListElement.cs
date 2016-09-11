namespace Localwire.UnityUIComponents.AutocompleteTextbox.Subelements
{
    using System;
    using Shared;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class AutocompleteResultListElement : MonoBehaviour
    {
        private bool _isBound;
        private int _index;

        public void Bind(int index, Action<int> onClick)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (_isBound) return;

            _index = index;
            gameObject.AddComponent<EventTrigger>().AddEvent(onClick, EventTriggerType.PointerUp, _index);
            
            _isBound = true;
        }

        public virtual void SwapItem(string newLabel)
        {
            GetComponent<Text>().text = newLabel;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
                
    }
}
