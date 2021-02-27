// File     : Scrap.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts;
using NUnit.Framework.Constraints;
using Player;
using TMPro;
using UnityEngine;

namespace Buildings
{
    public class Scrap : Building
    {

        [SerializeField] private EResource Resource;
        [SerializeField] private int AmountOfResource;
        [SerializeField] private float ReduceAmountPerUnit  = 1;
        private float Counter;
        private float resourceAmount = 0;

        public int GetAmountOfResource => AmountOfResource;
        public EResource GetResource;

        protected override void OnBuildEffect()
        {
            UnitCanEnter = true;
            base.OnBuildEffect();
        }
        
        public void Update()
        {
            if (UnitAmount == 0) return;
            Debug.Log(IsResourceMax());
            if (IsResourceMax()) return;
            
                resourceAmount += ReduceAmountPerUnit * UnitAmount * Time.deltaTime;

                if (resourceAmount >= AmountOfResource) resourceAmount = AmountOfResource;
                

                if (resourceAmount >= 1)
                {
                    AmountOfResource -= (int) resourceAmount;

                    switch (Resource)
                    {
                        case EResource.Wood:
                            PlayerData.GetInstance.IncreaseWood((int) resourceAmount);
                            break;
                        case EResource.Stone:
                            PlayerData.GetInstance.IncreaseStone((int) resourceAmount);
                            break;
                        case EResource.Steel:
                            PlayerData.GetInstance.IncreaseSteel((int) resourceAmount);
                            break;
                        case EResource.Toilette:
                            PlayerData.GetInstance.IncreaseToiletPaper((int) resourceAmount);
                            break;
                        case EResource.Food:
                            PlayerData.GetInstance.IncreaseFood((int) resourceAmount);
                            break;
                    }

                    resourceAmount -= (int) resourceAmount;


                    StartOnValueChanged();
                    if (AmountOfResource == 0)
                    {
                        DestroyEffect();
                        EntranceGround.IsBlockedH = false;
                        Destroy(gameObject);
                    }
                }
        }

        private bool IsResourceMax()
        {
            switch (Resource)
            {
                case EResource.Wood: 
                    return PlayerData.GetInstance.GetIsWoodOnMax;
                case EResource.Stone:
                    PlayerData.GetInstance.IncreaseStone((int) resourceAmount);
                    break;
                case EResource.Steel:
                    PlayerData.GetInstance.IncreaseSteel((int) resourceAmount);
                    break;
                case EResource.Toilette:
                    PlayerData.GetInstance.IncreaseToiletPaper((int) resourceAmount);
                    break;
                case EResource.Food:
                    PlayerData.GetInstance.IncreaseFood((int) resourceAmount);
                    break;
            }

            return true;
        }
        
    }
}