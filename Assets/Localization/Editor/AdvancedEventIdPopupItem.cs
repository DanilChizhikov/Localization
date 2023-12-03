using UnityEditor.IMGUI.Controls;

namespace MbsCore.Localization.Editor
{
    internal sealed class AdvancedEventIdPopupItem : AdvancedDropdownItem
    {
        public string EventId { get; }

        public AdvancedEventIdPopupItem(string name, string eventId) : base(name)
        {
            EventId = eventId;
        }
    }
}