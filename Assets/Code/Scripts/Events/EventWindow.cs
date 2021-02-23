// File     : EventWindow.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using TMPro;
using UnityEngine;

namespace Code.Scripts.Events
{
    public class EventWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Header;
        [SerializeField] private TextMeshProUGUI Description;
        [SerializeField] private TextMeshProUGUI Effect;

        public void SetHeaderText(string _headerText)
        {
            Header.text = _headerText;
        }

        public void SetDescriptionText(string _descriptionText)
        {
            Description.text = _descriptionText;
        }

        public void SetEffectText(string _effectText)
        {
            Effect.text = _effectText;
        }

        public void OnButtonClick_Accept()
        {
            AudioManager.GetInstance.Play("BuildSlotClicked");
            Destroy(gameObject);
        }
    }
}