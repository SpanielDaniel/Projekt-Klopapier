﻿using Code.Scripts;
using Player;
using UnityEngine;

namespace Buildings
{
    public class DestroyedHouse : Building
    {

        [SerializeField] private EResource CurrentResource;
        [SerializeField] private int AmountOfWood = 0;
        [SerializeField] private int AmountOfStone = 0;
        [SerializeField] private int AmountOfSteel = 0;
        [SerializeField] private float ReduceAmountPerUnit  = 1;
        private float Counter;
        private float resourceAmount = 0;

        
        public int GetAmountOfWood => AmountOfWood ;
        public int GetAmountOfStone => AmountOfStone;
        public int GetAmountOfSteel => AmountOfSteel;
        
        public EResource GetResource;


        public void AddRes(EResource _res, int _amount)
        {
            switch (_res)
            {
                case EResource.Wood:
                    AmountOfWood += _amount; 
                    break;
                case EResource.Stone:
                    AmountOfStone += _amount;
                    break;
                case EResource.Steel:
                    AmountOfSteel += _amount;
                    break;
            }
        }
        protected override void OnBuildEffect()
        {
            UnitCanEnter = true;
            base.OnBuildEffect();
        }
        
        public void Update()
        {
            if (UnitAmount == 0) return;

            if (IsEmpty())
            {
                DestroyEffect();
                EntranceGround.IsBlockedH = false;
                Destroy(gameObject);
            }

            if (IsResourceMax()) return;
            
            resourceAmount += ReduceAmountPerUnit * UnitAmount * Time.deltaTime;
            if (resourceAmount >= 1)
            {
                
                if (!IsEmpty())
                {
                    switch (CurrentResource)
                    {
                        case EResource.Wood:
                            AmountOfWood -= (int) resourceAmount;
                            PlayerData.GetInstance.IncreaseWood( (int) resourceAmount);
                            break;
                        case EResource.Stone:
                            AmountOfStone -= (int) resourceAmount;
                            PlayerData.GetInstance.IncreaseStone( (int) resourceAmount);
                            break;
                        case EResource.Steel:
                            AmountOfSteel -= (int) resourceAmount;
                            PlayerData.GetInstance.IncreaseSteel( (int) resourceAmount);
                            break;
                        default:
                            break;
                    }
                    
                    resourceAmount -= (int)resourceAmount;


                    StartOnValueChanged();
                    
                }
            }
        }
        private bool IsEmpty()
        {
            if (AmountOfWood <= 0)
            {
                AmountOfWood = 0;
                CurrentResource = EResource.Stone;
                if (AmountOfStone <= 0)
                {
                    AmountOfStone = 0;
                    CurrentResource = EResource.Steel;
                    
                    if (AmountOfSteel <= 0)
                    {
                        AmountOfSteel = 0;
                        CurrentResource = EResource.Wood;
                    }
                }
            }
            
            
            
            
            return AmountOfWood <= 0 && AmountOfStone <= 0 && AmountOfSteel <= 0;
        }

        private bool IsResourceMax()
        {
            switch (CurrentResource)
            {
                case EResource.Wood: 
                    bool isMax1 = PlayerData.GetInstance.GetIsWoodOnMax;
                    if (isMax1) CurrentResource = EResource.Stone;
                    return isMax1;
                case EResource.Stone:
                    bool isMax2 = PlayerData.GetInstance.GetIsStoneOnMax;
                    if (isMax2) CurrentResource = EResource.Steel;
                    return isMax2; 
                case EResource.Steel:
                    bool isMax3 = PlayerData.GetInstance.GetIsSteelOnMax;
                    if (isMax3) CurrentResource = EResource.Wood;
                    return isMax3;
                default:
                    break;
            }

            return true;
        }
        
    }
}