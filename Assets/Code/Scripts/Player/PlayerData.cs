// File     : PlayerRessources.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

namespace Player
{
    public class PlayerData : MonoBehaviour
    {
        #region Init

        public static event Action<int> ToiletPaperAmountChanged;
        public static event Action<int> WoodAmountChanged;
        public static event Action<int> StoneAmountChanged;
        public static event Action<int> SteelAmountChanged;
        public static event Action<int> FoodAmountChanged;

        [SerializeField] private int ToiletPaperStartAmount;
        [SerializeField] private int WoodStartAmount;
        [SerializeField] private int StoneStartAmount;
        [SerializeField] private int SteelStartAmount;
        [SerializeField] private int FoodStartAmount;

        private int ToiletPaperAmount;
        private int WoodAmount;
        private int StoneAmount;
        private int SteelAmount;
        private int FoodAmount;

        private int ToiletPaperAmountH
        {
            get => ToiletPaperAmount;
            set
            {
                ToiletPaperAmount = value;
                ToiletPaperAmountChanged?.Invoke(ToiletPaperAmount);
            }
        }

        private int WoodAmountH
        {
            get => WoodAmount;
            set
            {
                WoodAmount = value;
                WoodAmountChanged?.Invoke(WoodAmount);
            }
        }
        
        private int StoneAmountH
        {
            get => StoneAmount;
            set
            {
                StoneAmount = value;
                StoneAmountChanged?.Invoke(StoneAmount);
            }
        }
        
        private int SteelAmountH
        {
            get => SteelAmount;
            set
            {
                SteelAmount = value;
                SteelAmountChanged?.Invoke(SteelAmount);
            }
        }

        private int FoodAmountH
        {
            get => FoodAmount;
            set
            {
                FoodAmount = value;
                FoodAmountChanged?.Invoke(FoodAmount);
            }
        }

        private void Start()
        {
            ToiletPaperAmountH = ToiletPaperStartAmount;
            WoodAmountH = WoodStartAmount;
            StoneAmountH = StoneStartAmount;
            SteelAmountH = SteelStartAmount;
            FoodAmountH = FoodStartAmount;
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
        
        #endregion
    }
}