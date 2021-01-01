using System;
using Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class UIBuildingManager : UIVisibilityEvent
    {


        #region ----- Init -----

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

        #endregion

        #region ----- Functions -----
        
        /// <summary>
        /// Update the whole user interface.
        /// </summary>
        private void UpdateUI()
        {
            // Don't update if the player didn't selected a building.
            if (CurrentSelectedBuilding == null) return;
            
            
            BuildingNameText.text = CurrentSelectedBuilding.GetBuildingData.Name;
            BuildingImage.sprite = CurrentSelectedBuilding.GetBuildingData.BuldingTexture;
            BuildingLevelText.text = "Level " + CurrentSelectedBuilding.CurrentLevelH;
            
            UpdateLifeBar();
        }
        
        /// <summary>
        /// Updates the Life Bar with the values of the current selected building.
        /// </summary>
        private void UpdateLifeBar()
        {
            int level = CurrentSelectedBuilding.CurrentLevelH;
            
            LifeBar.maxValue = CurrentSelectedBuilding.GetBuildingData.Levels[level].MaxLife;
            LifeBar.value = CurrentSelectedBuilding.CurrentHealthH;
        }
        
        /// <summary>
        /// Close the Building UI
        /// </summary>
        private void CloseBuildingUI()
        {
            CurrentSelectedBuilding = null;            
            IsHudOpenH = false;
        }

        #endregion

        #region ----- Events -----
        
        public void OnButton_Exit()
        {
            CloseBuildingUI();
        }

        public void OnButton_Upgrade()
        {
            CurrentSelectedBuilding.Upgrade();
        }

        private void OnBuildingClicked(Building _building)
        {
            // Triggers the UIActivity event.
            IsHudOpenH = true;
            CurrentSelectedBuilding = _building;
            
            UpdateUI();
        }

        public override void SetUIActive(bool _isActive)
        {
            BuildingUI.SetActive(_isActive);
        }

        #endregion
    }
}