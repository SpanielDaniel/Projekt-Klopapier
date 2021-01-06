// File     : UIResourcesManager.cs
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

        private void Awake()
        {
            PlayerData.ToiletPaperAmountChanged += SetTextToToiletPaperAmount;
            PlayerData.WoodAmountChanged += SetTextToWoodAmount;
            PlayerData.StoneAmountChanged += SetTextToStoneAmount;
            PlayerData.SteelAmountChanged += SetTextToSteelAmount;
            PlayerData.FoodAmountChanged += SetTextToFoodAmount;
        }

        private void SetTextToToiletPaperAmount(int _toiletAmount)
        {
            ToiletPaper.text = _toiletAmount.ToString();
        }
        private void SetTextToWoodAmount(int _woodAmount)
        {
            Wood.text = _woodAmount.ToString();
        }
        private void SetTextToStoneAmount(int _stoneAmount)
        {
            Stone.text = _stoneAmount.ToString();
        }
        private void SetTextToSteelAmount(int _steelAmount)
        {
            Steel.text = _steelAmount.ToString();
        }
        private void SetTextToFoodAmount(int _foodAmount)
        {
            Food.text = _foodAmount.ToString();
        }
        
    }
}