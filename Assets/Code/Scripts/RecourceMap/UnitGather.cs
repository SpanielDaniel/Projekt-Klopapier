using System;
using UnityEngine;

namespace Code.Scripts
{
    public class UnitGather 
    {
        public GameObject UnitUi;
        public Unit Unit;
        public bool IsGather { get; set; }

        public Ground _comeFromGround;

        public MapResource Resource;

        private float TimerToGoToRes = 0;
        private float TimerToGoBack = 0;

        public float GetTimerToGoToRes => TimerToGoToRes;
        public float GetTimerToGoBack => TimerToGoBack;
        
        
        private int WoodAmount = 0;
        private int StoneAmount =0 ;
        private int SteelAmount = 0;
        private int ToilettePaperAmount = 0;
        private int FoodAmount = 0;

        public void AddResource(EResource _res, int _amount)
        {
            switch (_res)
            {
                case EResource.Wood:
                    WoodAmount += _amount;
                    break;
                case EResource.Stone:
                    StoneAmount += _amount;
                    break;
                case EResource.Steel:
                    SteelAmount += _amount;
                    break;
                case EResource.Toilette:
                    ToilettePaperAmount += _amount;
                    break;
                case EResource.Food:
                    FoodAmount += _amount;
                    break;
            }
        }
        
        
        public UnitGather(GameObject _obj,Unit _unit ,bool _isGather)
        {
            UnitUi = _obj;
            Unit = _unit;
            IsGather = _isGather;
        }

        public void StartGather()
        {
            Debug.Log("ressec " + Resource.GetTimeToGoToResourcesInSeconds);
            TimerToGoToRes = Resource.GetTimeToGoToResourcesInSeconds;
            UnitUi.GetComponent<UIUnitGather>().Init(this);
            UnitUi.GetComponent<UIUnitGather>().StartMoving();
        }

        public void SetGround(Ground _ground)
        {
            _comeFromGround = _ground;
        }
    }
}