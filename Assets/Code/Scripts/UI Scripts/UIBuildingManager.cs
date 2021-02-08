﻿using System;
using Build;
using Buildings;
using Code.Scripts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts
{
    public enum EBuilding
    {
        Base,
        House,
        Hospital,
        Storage,
        Farm,
        Scrap,
        DestroyedHouse,
    }
    public class UIBuildingManager : UIVisibilityEvent
    {


        #region ----- Init -----

        [SerializeField] private GameObject BuildingUI;
        [SerializeField] private GameObject Level;
        [SerializeField] private GameObject BuildUI;
        [SerializeField] private GameObject UpgradeButton;
        [SerializeField] private GameObject DemolishButton;
        [SerializeField] private GameObject RepairButton;
        
        
        [SerializeField] private Text BuildingNameText;
        [SerializeField] private Text BuildingLevelText;
        [SerializeField] private Image BuildingImage;
        [SerializeField] private UISlot[] BuildingSlots;
        [SerializeField] private Slider LifeBar;
        
        
        [SerializeField] private Text BuildingNameText2;
        [SerializeField] private Image BuildingImage2;
        [SerializeField] private UISlot[] BuildingSlots2;
        [SerializeField] private Slider LifeBar2;


        [SerializeField] private GameObject[] UIBuildingElements;

        private Building CurrentSelectedBuilding;

        private void Awake()
        {
            Building.OnClick += OnBuildingClicked;
            Building.ValueChanged += UpdateUI;
            Building.IsFinished += OnFinished;
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
            
            
            BuildingNameText.text = CurrentSelectedBuilding.GetData.Name;
            BuildingImage.sprite = CurrentSelectedBuilding.GetData.BuldingTexture;
            
            BuildingNameText2.text = CurrentSelectedBuilding.GetData.Name;
            BuildingImage2.sprite = CurrentSelectedBuilding.GetData.BuldingTexture;
            UpdateLevel();
            if (!CurrentSelectedBuilding.IsBuiltHandler)
            {

                foreach (UISlot slot in BuildingSlots2)
                {
                    slot.SetDefaultSprite();
                }
                
                for (int i = 0; i < CurrentSelectedBuilding.GetUnitIDs.Count; i++)
                {
                    
                    
                    BuildingSlots2[i].SetImage(Unit.Units[CurrentSelectedBuilding.GetUnitIDs[i]].GetUnitData.Icon);
                }
            }

            UpdateLifeBar();
            
            
            foreach (var HudElement in UIBuildingElements)
            {
                
                HudElement.SetActive(false);
            }
           

            if (!CurrentSelectedBuilding.IsBuiltHandler) return;
            
            if(CurrentSelectedBuilding is Storage) UIBuildingElements[(int)EBuilding.Storage].SetActive(true);
            if(CurrentSelectedBuilding is Farm) UIBuildingElements[(int)EBuilding.Farm].SetActive(true);
            if(CurrentSelectedBuilding is Base)
            {
                
                UIBuildingElements[(int)EBuilding.Base].SetActive(true);
                UIBuildingElements[(int)EBuilding.Base].GetComponent<UIBase>().SetSlotEntrance(CurrentSelectedBuilding.GetEntrancePosition);
            }

            if (CurrentSelectedBuilding is Scrap)
            {
                UIBuildingElements[(int)EBuilding.Scrap].SetActive(true);
            }
            
            
        }

        private void UpdateLevel()
        {
            BuildingLevelText.text = "Level " + (CurrentSelectedBuilding.CurrentLevelH + 1);
        }
        
        /// <summary>
        /// Updates the Life Bar with the values of the current selected building.
        /// </summary>
        private void UpdateLifeBar()
        {
            int level = CurrentSelectedBuilding.CurrentLevelH;
            
            LifeBar.maxValue = CurrentSelectedBuilding.GetData.Levels[level].MaxLife;
            LifeBar.value = CurrentSelectedBuilding.CurrentHealthH;
            
            LifeBar2.maxValue = CurrentSelectedBuilding.GetData.Levels[level].MaxLife;
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
            FindObjectOfType<AudioManager>().Play("BuildSlotClicked");
            CloseBuildingUI();
        }

        public void OnButton_Upgrade()
        {
            FindObjectOfType<AudioManager>().Play("BuildSlotClicked");
            CurrentSelectedBuilding.Upgrade();
        }


        [SerializeField] private BuildManager BuildManager;
        public void OnButton_Destroy()
        {
            PlayDestroySound();
            BuildManager.OnBuildingDestried(CurrentSelectedBuilding);
            CurrentSelectedBuilding.DestroyEffect();
            Destroy(CurrentSelectedBuilding.gameObject);
            SetUIActive(false);
        }

        private void PlayDestroySound()
        {
            FindObjectOfType<AudioManager>().Play("BuildingDestroy");

        }
        
        private void OnBuildingClicked(Building _building)
        {
            if(CurrentSelectedBuilding != _building) FindObjectOfType<AudioManager>().Play("BuildingClicked");
            CurrentSelectedBuilding = _building;
            
            
            IsHudOpenH = true;
            
            if (CurrentSelectedBuilding.GetData.IsUpgradable)
            {
                Level.SetActive(true);
                UpgradeButton.SetActive(true);
            }
            else
            {
                UpgradeButton.SetActive(false);
                Level.SetActive(false);
            }

            if (CurrentSelectedBuilding.GetData.CanBeDemolished)
            {
                DemolishButton.SetActive(true);
            }
            else
            {
                DemolishButton.SetActive(false);
            }
            
            if (CurrentSelectedBuilding.GetData.CanBeRepaired)
            {
                RepairButton.SetActive(true);
            }
            else
            {
                RepairButton.SetActive(false);
            }
            
            
            
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
            
            if (!CurrentSelectedBuilding.IsBuiltHandler)
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
        
        private void OnFinished(Building obj)
        {
            if (CurrentSelectedBuilding != obj) return;
            BuildUI.SetActive(false);
            BuildingUI.SetActive(true);
            UpdateUI();
        }

        #endregion
    }
}