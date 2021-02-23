// File     : MapResource.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using System.Runtime.CompilerServices;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Code.Scripts
{
    public enum EMapResource
    {
        Wood,
        Stone,
        Steel,
        City,
    }
    
    public enum EResource
    {
        Wood,
        Stone,
        Steel,
        Toilette,
        Food,
    }
    
    public class MapResource : MonoBehaviour
    , IPointerEnterHandler
    {
        public static event Action<GameObject,EMapResource> ResourceIsEmpty;
        [SerializeField] private float TimeToEarnResourcesInSeconds;
        [SerializeField] private int MinAmount;
        [SerializeField] private int MaxAmount;
        [SerializeField] private bool IsRegeneration;
        [SerializeField] private float ResourceRegenerationAmountInSeconds;


        [SerializeField] private EMapResource Res;

        public EMapResource GetRes => Res;
        
        private float RegenerationCounter;
        
        private int WoodAmount;
        private int StoneAmount;
        private int SteelAmount;
        private int ToilettePaperAmount;
        private int FoodAmount;
        private void Awake()
        {
            RandomizeResourceAmount();
        }

        private void Update()
        {
            if (IsRegeneration)
            {
                RegenerationCounter += Time.deltaTime * ResourceRegenerationAmountInSeconds;
                if (RegenerationCounter >= 1)
                {
                    WoodAmount += 1;
                    if (WoodAmount > MaxAmount) WoodAmount = MaxAmount;
                    
                    StoneAmount += 1;
                    if (WoodAmount > MaxAmount) StoneAmount = MaxAmount;
                    
                    SteelAmount += 1;
                    if (WoodAmount > MaxAmount) SteelAmount = MaxAmount;
                    
                    ToilettePaperAmount += 1;
                    if (WoodAmount > MaxAmount) ToilettePaperAmount = MaxAmount;
                    
                    FoodAmount += 1;
                    if (WoodAmount > MaxAmount) FoodAmount = MaxAmount;
                }
            }
        }
        
        private void RandomizeResourceAmount()
        {
            Random random = new Random();
            WoodAmount = random.Next(MinAmount, MaxAmount + 1);
        }
        
        public int GetResource(EResource _resource,int _reduceAmount)
        {

            switch (_resource)
            {
                case EResource.Wood:
                    return GetWood(_reduceAmount);
                case EResource.Stone:
                    return GetStone(_reduceAmount);
                case EResource.Steel:
                    return GetSteel(_reduceAmount);
                case EResource.Toilette:
                    return GetToilettePaper(_reduceAmount);
                case EResource.Food:
                    return GetFood(_reduceAmount);
                    break;
                default:
                    Debug.Log("GetResError");
                    break;
            }

            return 0;
        }

        private int GetWood(int _reduceAmount)
        {
            if (WoodAmount - _reduceAmount < 0)
            {
                int resource = WoodAmount;
                WoodAmount = 0;
                return resource;
            }
            
            WoodAmount -= _reduceAmount;
            return _reduceAmount;
        }
        
        private int GetStone(int _reduceAmount)
        {
            if (StoneAmount - _reduceAmount < 0)
            {
                int resource = StoneAmount;
                StoneAmount = 0;
                return resource;
            }
            
            StoneAmount -= _reduceAmount;
            return _reduceAmount;
        }
        
        private int GetSteel(int _reduceAmount)
        {
            if (SteelAmount - _reduceAmount < 0)
            {
                int resource = SteelAmount;
                SteelAmount = 0;
                return resource;
            }
            
            SteelAmount -= _reduceAmount;
            return _reduceAmount;
        }

        private int GetToilettePaper(int _reduceAmount)
        {
            if (ToilettePaperAmount - _reduceAmount < 0)
            {
                int resource = ToilettePaperAmount;
                ToilettePaperAmount = 0;
                return resource;
            }
            
            ToilettePaperAmount -= _reduceAmount;
            return _reduceAmount;
        }
        
        private int GetFood(int _reduceAmount)
        {
            if (FoodAmount - _reduceAmount < 0)
            {
                int resource = FoodAmount;
                FoodAmount = 0;
                return resource;
            }
            
            FoodAmount -= _reduceAmount;
            return _reduceAmount;
        }

        public void RemoveResource()
        {
            ResourceGenerator.Resources.Remove(this.gameObject);
            Destroy(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (UIPointerInHudManager.GetIsInHut)
            {
                Debug.Log("isInHud");
                return;
            }
            Debug.Log("PointerEnter");
        }

    }


    
}