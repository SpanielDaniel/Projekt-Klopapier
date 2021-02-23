// File     : EventWindow.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using TMPro;
using UnityEngine;

namespace Code.Scripts.Events
{
    public class EventWindow : MonoBehaviour
    {
        public static event Action<string,int> OnEventWindowClosed;
        [SerializeField] private TextMeshProUGUI Header;
        [SerializeField] private TextMeshProUGUI Description;
        [SerializeField] private TextMeshProUGUI Effect;

        
        private int NextEventID;
        private string NextFileName;

        public void SetNextID(int _nextId)
        {
            NextEventID = _nextId;
        }

        public void SetNextFileName(string _fileName)
        {
            NextFileName = _fileName;
        }
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
            OnEventWindowClosed?.Invoke(NextFileName,NextEventID);
            Destroy(gameObject);
        }
    }
}