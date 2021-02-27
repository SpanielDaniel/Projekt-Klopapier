// File     : MapResource.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

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
    , IPointerClickHandler
    , IPointerExitHandler
    {
        public static event Action<GameObject,EMapResource> ResourceIsEmpty;
        public static event Action<MapResource> OnClickRes;
        [SerializeField] private float TimeToEarnResourcesInSeconds;
        //[SerializeField] private float TimeMultiplicator;

        [SerializeField] private int MinAmount;
        [SerializeField] private int MaxAmount;
        [SerializeField] private bool IsRegeneration;
        [SerializeField] private float ResourceRegenerationAmountInSeconds;
        [SerializeField] private GameObject ResInfo;
        [SerializeField] private TextMeshProUGUI ResAmountText;


        [SerializeField] private EMapResource Res;

        public EMapResource GetRes => Res;

        private GameObject Line;
        
        private float RegenerationCounter;
        private float TimeToGoToResourcesInSeconds;

        private int WoodAmount;
        private int StoneAmount;
        private int SteelAmount;
        private int ToilettePaperAmount;
        private int FoodAmount;
        
        public int  GetWoodAmount => WoodAmount;
        public int  GetStoneAmount => StoneAmount;
        public int  GetSteelAmount => SteelAmount;
        public int  GetToilettePaperAmount => ToilettePaperAmount;
        public int  GetFoodAmount => FoodAmount;


        public float GetTimeToEarnResourcesInSeconds => TimeToEarnResourcesInSeconds;
        public float GetTimeToGoToResourcesInSeconds => TimeToGoToResourcesInSeconds;


        public float GetTime()
        {
            float time = WoodAmount * TimeToEarnResourcesInSeconds +
                         StoneAmount * TimeToEarnResourcesInSeconds +
                         SteelAmount * TimeToEarnResourcesInSeconds +
                         ToilettePaperAmount * TimeToEarnResourcesInSeconds +
                         FoodAmount + TimeToEarnResourcesInSeconds;
            return time;
        }
        private void Awake()
        {
            
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

                    UpdateResText();
                }
            }
        }

        private void UpdateResText()
        {
            switch (Res)
            {
                case EMapResource.Wood:
                    ResAmountText.text = WoodAmount.ToString();
                    break;
                case EMapResource.Stone:
                    ResAmountText.text = StoneAmount.ToString();
                    break;
                case EMapResource.Steel:
                    ResAmountText.text = SteelAmount.ToString();
                    break;
                case EMapResource.City:
                    break;
            }
            
        }

        private void Start()
        {
            RandomizeResourceAmount();
            ResInfo.SetActive(false);
            UpdateResText();
            
        }

        public void Init(float _time)
        {
            TimeToGoToResourcesInSeconds = _time;
        }

        public void SetLine(GameObject _line)
        {
            Line = _line;
        }
        
        private void RandomizeResourceAmount()
        {

            //int random = Random.Range(MinAmount, MaxAmount + 1);
            switch (GetRes)
            {
                case EMapResource.Wood:
                    WoodAmount =  Random.Range(MinAmount, MaxAmount + 1);
                    break;
                case EMapResource.Stone:
                    StoneAmount =  Random.Range(MinAmount, MaxAmount + 1);
                    break;
                case EMapResource.Steel:
                    SteelAmount =  Random.Range(MinAmount, MaxAmount + 1);
                    break;
                case EMapResource.City:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            UpdateResText();
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
            UpdateResText();
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
            UpdateResText();
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
            UpdateResText();
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
            UpdateResText();
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
            UpdateResText();
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
            UpdateResText();
            return _reduceAmount;
        }

        public void RemoveResource()
        {
            ResourceGenerator.Resources.Remove(this.gameObject);
            Destroy(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!UIPointerInHudManager.GetIsInHut)
            {
                Line.SetActive(true);
                ResInfo.SetActive(true);
                this.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
                return;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickRes?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Line.SetActive(false);
            ResInfo.SetActive(false);
        }
    }


    
}