using System;
using TMPro;
using UnityEngine;

namespace Code.Scripts
{
    public class UIUnitGather : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Text;
        [SerializeField] private UnitGather UnitGather;

        private float Timer = 0;

        
        public void Init(UnitGather _gather)
        {
            UnitGather = _gather;
            Timer = UnitGather.GetTimerToGoToRes;
            Debug.Log("Timer sert " + Timer);

            UpdateText();
        }

        private void UpdateText()
        {
            SetText(((int) Timer).ToString());
        }
        public void SetText(string _text)
        {
            Text.text = _text;
        }

        private bool IsStarted = false;

        public void StartMoving()
        {
            IsStarted = true;
        }

        private float counter = 0;
        private void Update()
        {
            if (IsStarted)
            {
                Timer -= Time.deltaTime;
                UpdateText();
            }
        }
    }
}