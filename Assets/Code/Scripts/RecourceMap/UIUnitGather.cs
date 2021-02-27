using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace Code.Scripts
{
    public class UIUnitGather : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Text;
        private UnitGather UnitGather;

        private float Timer = 0;

        private bool IsOnResource = false;

        private float Counter = 0;

        public void Init(UnitGather _gather)
        {
            UnitGather = _gather;
            Timer = UnitGather.GetTimerToGoToRes;
            Debug.Log("Timer sert " + Timer);

            UpdateText();
        }
        

        private void Update()
        {
            if (IsStarted)
            {
                Timer -= Time.deltaTime;
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
                    Debug.Log("Empty");
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
            Text.text = _text;
        }

        private bool IsStarted = false;

        public void StartMoving()
        {
            IsStarted = true;
        }

        private float counter = 0;
        
    }
}