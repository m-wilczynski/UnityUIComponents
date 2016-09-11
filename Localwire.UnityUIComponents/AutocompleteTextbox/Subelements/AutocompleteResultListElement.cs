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
        private Action<int> _onClick;

        public void Bind(int index, Action<int> onClick)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (_isBound) return;

            _index = index;
            _onClick = onClick;
            gameObject.AddComponent<EventTrigger>().AddEvent(_onClick, EventTriggerType.PointerUp, _index);
            
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
