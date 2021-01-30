using System;
using Buildings;
using Code.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public class UIBuildingManager : UIVisibilityEvent
    {


        #region ----- Init -----

        [SerializeField] private GameObject BuildingUI;
        [SerializeField] private GameObject BuildUI;
        
        
        
        [SerializeField] private Text BuildingNameText;
        [SerializeField] private Text BuildingLevelText;
        [SerializeField] private Image BuildingImage;
        [SerializeField] private UISlot[] BuildingSlots;
        [SerializeField] private Slider LifeBar;
        
        [SerializeField] private Text BuildingNameText2;
        [SerializeField] private Image BuildingImage2;
        [SerializeField] private UISlot[] BuildingSlots2;
        [SerializeField] private Slider LifeBar2;


        private Building CurrentSelectedBuilding;

        private void Awake()
        {
            Building.OnClick += OnBuildingClicked;
            Building.ValueChanged += UpdateUI;
            Building.IsFinished += OnFinisched;
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
            
            BuildingNameText2.text = CurrentSelectedBuilding.GetBuildingData.Name;
            BuildingImage2.sprite = CurrentSelectedBuilding.GetBuildingData.BuldingTexture;
            
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
            
            LifeBar2.maxValue = CurrentSelectedBuilding.GetBuildingData.Levels[level].MaxLife;
            LifeBar2.value = CurrentSelectedBuilding.CurrentHealthH;
        }
        
        /// <summary>
        /// Close the Building UI
        /// </summary>
        private void CloseBuildingUI()
        {
            IsHudOpenH = false;
            CurrentSelectedBuilding = null;
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

        public void OnButton_Destroy()
        {
            FindObjectOfType<AudioManager>().Play("BuildingDestroy");
            
            CurrentSelectedBuilding.DestroyEffect();
            GameObject objBuffer = CurrentSelectedBuilding.gameObject;
            SetUIActive(false);
            Destroy(objBuffer);
        }

        private void OnBuildingClicked(Building _building)
        {
            if(CurrentSelectedBuilding != _building) FindObjectOfType<AudioManager>().Play("BuildingClicked");

            CurrentSelectedBuilding = _building;
            IsHudOpenH = true;
            UpdateUI();
        }

        public override void SetUIActive(bool _isActive)
        {

            if (CurrentSelectedBuilding == null)
            {
                BuildUI.SetActive(false);
                BuildingUI.SetActive(false);
                return;
            }
            
            if (CurrentSelectedBuilding.GetIsBuilding)
            {
                BuildUI.SetActive(_isActive);
                if (BuildingUI.activeSelf == true)
                {
                    BuildingUI.SetActive(false);
                }
            }
            else
            {
                BuildingUI.SetActive(_isActive);
                if (BuildUI.activeSelf == true)
                {
                    BuildUI.SetActive(false);
                }
            }
        }
        
        private void OnFinisched(Building obj)
        {
            if (CurrentSelectedBuilding != obj) return;
            BuildUI.SetActive(false);
            BuildingUI.SetActive(true);
            UpdateUI();
        }

        #endregion
    }
}