namespace Localwire.UnityUIComponents.Shared
{
    using System;
    using UnityEngine.EventSystems;

    public static class UIExtensions
    {
        public static void AddEvent(this EventTrigger trigger, Action action, EventTriggerType type)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(ed => action());
            trigger.triggers.Add(entry);
        }

        public static void AddEvent<T>(this EventTrigger trigger, Action<T> action, EventTriggerType type, T actionParam)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(ed => action(actionParam));
            trigger.triggers.Add(entry);
        }
    }
}
