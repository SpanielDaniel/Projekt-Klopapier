using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts
{
    public class UIUnitGather : MonoBehaviour
    {

        public static event Action<Unit> IsBack; 
        [SerializeField] private TextMeshProUGUI Text;
        [SerializeField] private GameObject ReturnButton;
        [SerializeField] private Image Image;
        private UnitGather UnitGather;

        private float Timer = 0;
        private float TimeTheUnitWent = 0;

        private bool IsOnResource = false;
        private bool IsReturning = false;

        private float Counter = 0;

        public void Init(UnitGather _gather)
        {
            UnitGather = _gather;
            Timer = UnitGather.GetTimerToGoToRes;
            Image.sprite = _gather.Unit.GetUnitData.Icon;
            UpdateText();
        }
        

        private void Update()
        {
            if (IsStarted)
            {
                float time = Time.deltaTime;
                TimeTheUnitWent += time;
                Timer -= time;
                
                if (Timer <= 0)
                {
                    Timer = 0;
                    IsOnResource = true;
                    IsStarted = false;
                    Timer = UnitGather.Resource.GetTime();
                    Counter = 0;
                }
                UpdateText();
            }

            if (IsOnResource)
            {
                float reduceTime = Time.deltaTime;
                Timer -= reduceTime;
                Counter += reduceTime;
                bool isEmpty = false;
                
                if (Counter >= UnitGather.Resource.GetTimeToEarnResourcesInSeconds)
                {
                    Counter -= UnitGather.Resource.GetTimeToEarnResourcesInSeconds;
                    if (UnitGather.Resource.GetWoodAmount > 0)
                    {
                        int amount = UnitGather.Resource.GetResource(EResource.Wood, 1);
                        UnitGather.AddResource(EResource.Wood, amount);
                    }
                    else if(UnitGather.Resource.GetStoneAmount > 0)
                    {
                        int amount = UnitGather.Resource.GetResource(EResource.Stone, 1);
                        UnitGather.AddResource(EResource.Stone, amount);
                    }
                    else if (UnitGather.Resource.GetSteelAmount > 0)
                    {
                        int amount = UnitGather.Resource.GetResource(EResource.Steel, 1);
                        UnitGather.AddResource(EResource.Steel, amount);
                    }
                    else if (UnitGather.Resource.GetFoodAmount > 0)
                    {
                        int amount = UnitGather.Resource.GetResource(EResource.Food, 1);
                        UnitGather.AddResource(EResource.Food, amount);
                    }
                    else if (UnitGather.Resource.GetToilettePaperAmount > 0)
                    {
                        int amount = UnitGather.Resource.GetResource(EResource.Toilette, 1);
                        UnitGather.AddResource(EResource.Toilette, amount);
                    }
                    else
                    {
                        isEmpty = true;
                    }
                }
                
                
                if (isEmpty)
                {
                    OnButton_Return();
                }
                
                
                UpdateText();
            }

            if (IsReturning)
            {
                
                
                float reduceTime = Time.deltaTime;
                Timer -= reduceTime;
                if (Timer <= 0)
                {
                    IsBack?.Invoke(UnitGather.Unit);
                }
                UpdateText();
            }
            
            
        }
        

        private void UpdateText()
        {
            SetText(((int) Timer).ToString());
        }
        public void SetText(string _text)
        {
            string text = "";
            
            if (IsStarted)
            {
                text = "Geht hin:";
            }
            
            if (IsReturning)
            {
                text = "Geht zurück: ";
            }

            if (IsOnResource)
            {
                text = "Am sammeln:";
            }

            
            Text.text = text + _text;
        }

        private bool IsStarted = false;

        public void StartMoving()
        {
            IsStarted = true;
        }

        private float counter = 0;

        public void OnButton_Return()
        {
            IsStarted = false;
            IsOnResource = false;
            IsReturning = true;
            Timer = TimeTheUnitWent;
            ReturnButton.SetActive(false);
        }
        
    }
}