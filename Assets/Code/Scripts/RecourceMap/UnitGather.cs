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