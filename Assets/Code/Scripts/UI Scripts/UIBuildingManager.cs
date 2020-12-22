using System;
using Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class UIBuildingManager : HudBase
    {
        
        
        [SerializeField] private GameObject BuildingUI;
        [SerializeField] private Text BuildingNameText;
        [SerializeField] private Text BuildingLevelText;
        [SerializeField] private Image BuildingImage;
        [SerializeField] private UISlot[] BuildingSlots;
        [SerializeField] private Slider LifeBar;

        private Building CurrentSelectedBuilding;

        private void Awake()
        {
            Building.OnClick += OnBuildingClicked;
            Building.ValueChanged += UpdateUI;
        }
        
        private void Start()
        {
            CloseBuildingUI();
        }

        private void OnBuildingClicked(Building _building)
        {
            IsHudOpenH = true;
            CurrentSelectedBuilding = _building;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (CurrentSelectedBuilding == null) return;
            BuildingNameText.text = CurrentSelectedBuilding.GetBuildingStats.Name;
            BuildingImage.sprite = CurrentSelectedBuilding.GetBuildingStats.BuldingTexture;
            BuildingLevelText.text = "Level " + CurrentSelectedBuilding.CurrentLevelH;
            UpdateLifeBar();
        }
        
        private void UpdateLifeBar()
       {
           LifeBar.maxValue = CurrentSelectedBuilding.GetBuildingStats.Levels[CurrentSelectedBuilding.CurrentLevelH].MaxLife;
           LifeBar.value = CurrentSelectedBuilding.CurrenthealthH;
       }
    
        private void CloseBuildingUI()
        {
            CurrentSelectedBuilding = null;            
            IsHudOpenH = false;
        }

        public void OnButton_Exit()
        {
            CloseBuildingUI();
        }

        public void OnButton_Upgrade()
        {
            CurrentSelectedBuilding.Upgrade();
        }

        public override void SetUIActive(bool _isActive)
        {
            BuildingUI.SetActive(_isActive);

        }
    }
}