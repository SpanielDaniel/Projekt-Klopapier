// File     : UIResourcesManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts.Map;
using Player;
using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    public class UIResourcesManager : MonoBehaviour
    {

        public static Action OnButton_MapRes;
        [SerializeField] private TextMeshProUGUI ToiletPaper;
        [SerializeField] private TextMeshProUGUI Wood;
        [SerializeField] private TextMeshProUGUI Stone;
        [SerializeField] private TextMeshProUGUI Steel;
        [SerializeField] private TextMeshProUGUI Food;
        [SerializeField] private TextMeshProUGUI Population;

        private int StorageCapacity;

        private void Awake()
        {
            PlayerData.ToiletPaperAmountChanged += SetTextToToiletPaperAmount;
            PlayerData.WoodAmountChanged += SetTextToWoodAmount;
            PlayerData.StoneAmountChanged += SetTextToStoneAmount;
            PlayerData.SteelAmountChanged += SetTextToSteelAmount;
            PlayerData.FoodAmountChanged += SetTextToFoodAmount;
            PlayerData.StorageCapacityChanged += SetStorageCapacity;
            PlayerData.PopulationChanged += SetTextPopulation;
            

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
            ToiletPaper.text = _toiletAmount.ToString() ;
        }
        private void SetTextToWoodAmount(int _woodAmount)
        {
            Wood.text = _woodAmount.ToString() + "/"+ AddStorageCapacity();
        }
        private void SetTextToStoneAmount(int _stoneAmount)
        {
            Stone.text = _stoneAmount.ToString()+ "/"+ AddStorageCapacity();
        }
        private void SetTextToSteelAmount(int _steelAmount)
        {
            Steel.text = _steelAmount.ToString()+ "/"+ AddStorageCapacity();
        }
        private void SetTextToFoodAmount(float _foodAmount)
        {
            Food.text = Math.Round(_foodAmount).ToString() + "/"+ AddStorageCapacity();
        }

        private void SetTextPopulation(int _amount, int _max)
        {
            Population.text = "(" + _amount + "/" + _max + ")";
        }

        private string AddStorageCapacity()
        {
            return "|" + StorageCapacity;
        }

        public void OnButtonClick_MapRes()
        {
            OnButton_MapRes?.Invoke();
        }
        
    }
}