// File     : PlayerRessources.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts.Map;
using UnityEngine;

namespace Player
{
    public class PlayerData : Singleton<PlayerData>
    {
        #region Init

        public static event Action<int> ToiletPaperAmountChanged;
        public static event Action<int> WoodAmountChanged;
        public static event Action<int> StoneAmountChanged;
        public static event Action<int> SteelAmountChanged;
        public static event Action<float> FoodAmountChanged;
        public static event Action<int> StorageCapacityChanged;
        public static event Action<int,int> PopulationChanged;

        [SerializeField] private int ToiletPaperStartAmount;
        [SerializeField] private int WoodStartAmount;
        [SerializeField] private int StoneStartAmount;
        [SerializeField] private int SteelStartAmount;
        [SerializeField] private float FoodStartAmount;
        [SerializeField] private int StartStorageCapacity;

        public static event Action OnBaseIsUnderConstruction;
 
        public bool BaseIsUnderConstruction;
        public bool BaseIsBuild;
        
        private int ToiletPaperAmount;
        private int WoodAmount;
        private int StoneAmount;
        private int SteelAmount;
        private float FoodAmount;
        private int StorageCapacity;
        private int Population;
        private int MaxPopulation = 0;

        private bool IsWoodOnMax = false;
        private bool IsStoneOnMax = false;
        private bool IsSteelOnMax = false;
        private bool IsToilettePaperOnMax = false;
        private bool IsFoodOnMax = false;

        public bool GetIsWoodOnMax => IsWoodOnMax;
        public bool GetIsStoneOnMax => IsStoneOnMax;
        public bool GetIsSteelOnMax => IsSteelOnMax;
        public bool GetIsToilettePaperOnMax => IsToilettePaperOnMax;
        public bool GetIsFoodOnMax => IsFoodOnMax;
        

        public int PopulationH
        {
            get => Population;
            set
            {
                Population = value;
                PopulationChanged?.Invoke(Population,MaxPopulation);
            }
        }

        public int PopulationCapacityH
        {
            get => MaxPopulation;
            set
            {
                MaxPopulation = value;
                PopulationChanged?.Invoke(Population,MaxPopulation);
            } 
        }
        public int StorageCapacityH
        {
            get => StorageCapacity;
            set
            {
                
                StorageCapacity = value;
                
                WoodAmountH = WoodAmountH;
                StoneAmountH = StoneAmountH;
                SteelAmountH = SteelAmountH;
                ToiletPaperAmountH = ToiletPaperAmountH;
                
                StorageCapacityChanged?.Invoke(StorageCapacity);
            }
        }

        public int ToiletPaperAmountH
        {
            get => ToiletPaperAmount;
            set
            {
                ToiletPaperAmount = GetValue(value);
                if (ToiletPaperAmount == StorageCapacityH) IsToilettePaperOnMax = true;
                else IsToilettePaperOnMax = false;
                ToiletPaperAmountChanged?.Invoke(ToiletPaperAmount);
            }
        }

        public int WoodAmountH
        {
            get => WoodAmount;
            set
            {
                WoodAmount = GetValue(value);
                if (WoodAmount == StorageCapacityH) IsWoodOnMax = true;
                else IsWoodOnMax = false;
                WoodAmountChanged?.Invoke(WoodAmount);
            }
        }
        
        public int StoneAmountH
        {
            get => StoneAmount;
            set
            {
                StoneAmount = GetValue(value);
                if (StoneAmount == StorageCapacityH) IsStoneOnMax = true;
                else IsStoneOnMax = false;
                StoneAmountChanged?.Invoke(StoneAmount);
            }
        }
        
        public int SteelAmountH
        {
            get => SteelAmount;
            set
            {
                SteelAmount = GetValue(value);
                if (SteelAmount == StorageCapacityH) IsSteelOnMax = true;
                else IsSteelOnMax = false;
                SteelAmountChanged?.Invoke(SteelAmount);
                
            }
        }

        public float FoodAmountH
        {
            get => FoodAmount;
            set
            {
                FoodAmount = GetValue(value);
                if (FoodAmount == StorageCapacityH) IsFoodOnMax = true;
                else IsFoodOnMax = false;
                
                if (FoodAmount < 0) FoodAmount = 0;
                FoodAmountChanged?.Invoke(FoodAmount);
            }
        }

        private void Awake()
        {
            MapGenerator.MapIsBuild += UpdateRes;
        }

        private void Start()
        {
            StorageCapacityH = StartStorageCapacity;
            ToiletPaperAmountH = ToiletPaperStartAmount;
            WoodAmountH = WoodStartAmount;
            StoneAmountH = StoneStartAmount;
            SteelAmountH = SteelStartAmount;
            FoodAmountH = FoodStartAmount;
            UpdateRes();
        }

        private int GetValue(int _value)
        {
            if (_value >= StorageCapacity) return StorageCapacity;
            return _value;
        }
        
        private float GetValue(float _value)
        {
            if (_value >= StorageCapacity) return StorageCapacity;
            return _value;
        }

        public bool IsPlayerHavingEnoughResources(int _toiletPaper, int _wood, int _stone, int _steel)
        {
            if (ToiletPaperAmount - _toiletPaper >= 0
                && WoodAmount - _wood >= 0
                && StoneAmount - _stone >= 0
                && SteelAmount - _steel >= 0)
            {
                return true;
            }

            return false;
        }

        public void ReduceResources(int _toiletPaper, int _wood, int _stone, int _steel)
        {
            ToiletPaperAmountH -= _toiletPaper;
            WoodAmountH -= _wood;
            StoneAmountH -= _stone;
            SteelAmountH -= _steel;
        }

        public void IncreaseWood(int _amount)
        {
            WoodAmountH += _amount;
        }
        
        public void IncreaseStone(int _amount)
        {
            StoneAmountH += _amount;
        } 
        
        public void IncreaseSteel(int _amount)
        {
            SteelAmountH += _amount;
        } 
        
        public void IncreaseToiletPaper(int _amount)
        {
            ToiletPaperAmountH += _amount;
        }
        
        public void IncreaseFood (int _amount)
        {
            FoodAmountH += _amount;
        }

        private void UpdateRes()
        {
            StorageCapacityChanged?.Invoke(StorageCapacity);
            PopulationChanged?.Invoke(Population,MaxPopulation);
            PopulationChanged?.Invoke(Population,MaxPopulation);
            ToiletPaperAmountChanged?.Invoke(ToiletPaperAmount);
            WoodAmountChanged?.Invoke(WoodAmount);
            StoneAmountChanged?.Invoke(StoneAmount);
            SteelAmountChanged?.Invoke(SteelAmount);
            FoodAmountChanged?.Invoke(FoodAmount);
        }

        #endregion
    }
}