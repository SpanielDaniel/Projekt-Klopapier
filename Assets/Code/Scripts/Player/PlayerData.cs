// File     : PlayerRessources.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
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
        public static event Action<int> FoodAmountChanged;
        public static event Action<int> StorageCapacityChanged;

        [SerializeField] private int ToiletPaperStartAmount;
        [SerializeField] private int WoodStartAmount;
        [SerializeField] private int StoneStartAmount;
        [SerializeField] private int SteelStartAmount;
        [SerializeField] private int FoodStartAmount;
        [SerializeField] private int StartStorageCapacity;


        public static event Action OnBaseIsUnderConstruction;
 
        public bool BaseIsUnderConstruction;
        public bool BaseIsBuild;
        
        private int ToiletPaperAmount;
        private int WoodAmount;
        private int StoneAmount;
        private int SteelAmount;
        private int FoodAmount;
        private int StorageCapacity;


        public int StorageCapacityH
        {
            get => StorageCapacity;
            set
            {
                StorageCapacity = value;
                StorageCapacityChanged?.Invoke(StorageCapacity);
                WoodAmountH = WoodAmountH;
                StoneAmountH = StoneAmountH;
                SteelAmountH = SteelAmountH;
                ToiletPaperAmountH = ToiletPaperAmountH;
            }
        }

        public int ToiletPaperAmountH
        {
            get => ToiletPaperAmount;
            set
            {
                ToiletPaperAmount = GetValue(value);
                ToiletPaperAmountChanged?.Invoke(ToiletPaperAmount);
            }
        }

        public int WoodAmountH
        {
            get => WoodAmount;
            set
            {
                WoodAmount = GetValue(value);
                WoodAmountChanged?.Invoke(WoodAmount);
            }
        }
        
        public int StoneAmountH
        {
            get => StoneAmount;
            set
            {
                StoneAmount = GetValue(value);
                StoneAmountChanged?.Invoke(StoneAmount);
            }
        }
        
        public int SteelAmountH
        {
            get => SteelAmount;
            set
            {
                SteelAmount = GetValue(value);
                SteelAmountChanged?.Invoke(SteelAmount);
            }
        }

        public int FoodAmountH
        {
            get => FoodAmount;
            set
            {
                FoodAmount = GetValue(value);
                FoodAmountChanged?.Invoke(FoodAmount);
            }
        }

        private void Start()
        {
            StorageCapacityH = StartStorageCapacity;
            ToiletPaperAmountH = ToiletPaperStartAmount;
            WoodAmountH = WoodStartAmount;
            StoneAmountH = StoneStartAmount;
            SteelAmountH = SteelStartAmount;
            FoodAmountH = FoodStartAmount;
        }

        private int GetValue(int _value)
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

        #endregion
    }
}