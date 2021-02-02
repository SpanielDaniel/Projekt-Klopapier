﻿// File     : UIResourcesManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Player;
using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    public class UIResourcesManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ToiletPaper;
        [SerializeField] private TextMeshProUGUI Wood;
        [SerializeField] private TextMeshProUGUI Stone;
        [SerializeField] private TextMeshProUGUI Steel;
        [SerializeField] private TextMeshProUGUI Food;

        private int StorageCapacity;

        private void Awake()
        {
            PlayerData.ToiletPaperAmountChanged += SetTextToToiletPaperAmount;
            PlayerData.WoodAmountChanged += SetTextToWoodAmount;
            PlayerData.StoneAmountChanged += SetTextToStoneAmount;
            PlayerData.SteelAmountChanged += SetTextToSteelAmount;
            PlayerData.FoodAmountChanged += SetTextToFoodAmount;
            PlayerData.StorageCapacityChanged += SetStorageCapacity;
        }

        private void SetStorageCapacity(int _storageCapacity)
        {
            StorageCapacity = _storageCapacity;
            PlayerData data = PlayerData.GetInstance;
            
            SetTextToFoodAmount(data.FoodAmountH);
            SetTextToSteelAmount(data.StoneAmountH);
            SetTextToStoneAmount(data.StoneAmountH);
            SetTextToWoodAmount(data.WoodAmountH);
            SetTextToToiletPaperAmount(data.ToiletPaperAmountH);
        }

        private void SetTextToToiletPaperAmount(int _toiletAmount)
        {
            ToiletPaper.text = _toiletAmount.ToString() + AddStorageCapacity();
        }
        private void SetTextToWoodAmount(int _woodAmount)
        {
            Wood.text = _woodAmount.ToString()+ AddStorageCapacity();
        }
        private void SetTextToStoneAmount(int _stoneAmount)
        {
            Stone.text = _stoneAmount.ToString()+ AddStorageCapacity();
        }
        private void SetTextToSteelAmount(int _steelAmount)
        {
            Steel.text = _steelAmount.ToString()+ AddStorageCapacity();
        }
        private void SetTextToFoodAmount(int _foodAmount)
        {
            Food.text = _foodAmount.ToString() + AddStorageCapacity();
        }

        private string AddStorageCapacity()
        {
            return "|" + StorageCapacity;
        }
        
    }
}