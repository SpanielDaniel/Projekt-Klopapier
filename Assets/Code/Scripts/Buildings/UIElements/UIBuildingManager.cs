using System;
using Buildings;
using Code.Scripts;
using Code.Scripts.Buildings.UIElements;
using Code.Scripts.Map;
using Code.Scripts.UI_Scripts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
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
        
        
        [SerializeField] private TextMeshProUGUI BuildingNameText;
        [SerializeField] private TextMeshProUGUI BuildingLevelText;
        [SerializeField] private Image BuildingImage;
        [SerializeField] private Slider LifeBar;
        
        
        [SerializeField] private TextMeshProUGUI BuildingNameText2;
        [SerializeField] private Image BuildingImage2;
        [SerializeField] private UISlot[] BuildingSlots;
        [SerializeField] private Slider LifeBar2;


        [SerializeField] private GameObject[] UIBuildingElements;

        [SerializeField] private MapGenerator MapGenerator;
        
        
        private Building CurrentSelectedBuilding;
        private int BuildingID;
        
        

        private void Awake()
        {
            Building.OnClick += OnBuildingClicked;
            Building.ValueChanged += UpdateUI;
            Building.IsFinished += OnFinished;
            Building.DestroyDestroyed += OnBuildingDestroyDestroyed;
            UISlot.OnUnitRelease += ReleaseUnit;
            Pointer.OnUnitChanged += ChangeUnit;
        }

        private void ChangeUnit(int _firstSlotId, int _secondSlotID)
        {
            if(CurrentSelectedBuilding is House) CurrentSelectedBuilding.GetComponent<House>().ChangeUnitSlot(_firstSlotId,_secondSlotID);
            if(CurrentSelectedBuilding is Hospital) CurrentSelectedBuilding.GetComponent<Hospital>().ChangeUnitSlot(_firstSlotId,_secondSlotID);
        }

        private void OnBuildingDestroyDestroyed()
        {
            SetUIActive(false);
        }

        private void ReleaseUnit(Unit _unit)
        {
            Ground pos = CurrentSelectedBuilding.GetEntranceGround;
            CurrentSelectedBuilding.RemoveUnit(_unit,pos);
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
                foreach (UISlot slot in BuildingSlots)
                {
                    slot.SetDefaultSprite();
                    slot.RemoveUnit();
                }
                
                for (int i = 0; i < CurrentSelectedBuilding.GetUnitIDs.Count; i++)
                {
                    BuildingSlots[i].Init(Unit.Units[CurrentSelectedBuilding.GetUnitIDs[i]].GetUnitData.Icon,CurrentSelectedBuilding.GetUnitIDs[i]);
                }
            }

            UpdateLifeBar();
            
            
            foreach (var HudElement in UIBuildingElements)
            {
                HudElement.SetActive(false);
            }
           

            if (!CurrentSelectedBuilding.IsBuiltHandler) return;
            
            if(CurrentSelectedBuilding is Storage) UIBuildingElements[(int)EBuilding.Storage].SetActive(true);
            if (CurrentSelectedBuilding is Farm)
            {
                UIBuildingElements[(int)EBuilding.Farm].SetActive(true);
                UIBuildingElements[(int)EBuilding.Farm].GetComponent<UIFarm>().UpdateUI(CurrentSelectedBuilding as Farm);
            }
            if(CurrentSelectedBuilding is Base)
            {
                UIBuildingElements[(int)EBuilding.Base].SetActive(true);
                UIBuildingElements[(int)EBuilding.Base].GetComponent<UIBase>().SetSlotEntrance( MapGenerator.GetGroundFromGlobalPosition(CurrentSelectedBuilding.GetEntrancePoss()));
            }

            if (CurrentSelectedBuilding is Scrap)
            {
                UIBuildingElements[(int)EBuilding.Scrap].SetActive(true);
                UIBuildingElements[(int)EBuilding.Scrap].GetComponent<UIScrap>().UpdateUI(CurrentSelectedBuilding as Scrap);
            }

            if (CurrentSelectedBuilding is House)
            {
                UIBuildingElements[(int)EBuilding.House].SetActive(true);
                UIBuildingElements[(int)EBuilding.House].GetComponent<UIHouse>().UpdateUI(CurrentSelectedBuilding as House);
            }

            if (CurrentSelectedBuilding is Hospital)
            {
                UIBuildingElements[(int)EBuilding.Hospital].SetActive(true);
                UIBuildingElements[(int)EBuilding.Hospital].GetComponent<UIHospital>().UpdateUI(CurrentSelectedBuilding as Hospital);
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
            if(CurrentSelectedBuilding != null)CurrentSelectedBuilding.IsBuildingHudOpen = false;
            CurrentSelectedBuilding = null;
            BuildingID = 0;
        }

        #endregion

        #region ----- Events -----
        
        public void OnButton_Exit()
        {
            //EventSystem.current.SetSelectedGameObject(null);
            AudioManager.GetInstance.Play("BuildSlotClicked");
            SetUIActive(false);
            UIPointerInHudManager.SetPointerInHud(false);
        }

        public void OnButton_Upgrade()
        {
            EventSystem.current.SetSelectedGameObject(null);
            AudioManager.GetInstance.Play("BuildSlotClicked");
            CurrentSelectedBuilding.Upgrade();
        }


        [SerializeField] private BuildManager BuildManager;
        
        public void OnButton_Destroy()
        {
            EventSystem.current.SetSelectedGameObject(null);

            PlayDestroySound();
            SetUIActive(false);
            BuildManager.OnBuildingDestroyed(CurrentSelectedBuilding);
            CurrentSelectedBuilding.DestroyEffect();
            
            Destroy(CurrentSelectedBuilding.gameObject);
            UIPointerInHudManager.SetPointerInHud(false);

        }

        private void PlayDestroySound()
        {
            FindObjectOfType<AudioManager>().Play("BuildingDestroy");

        }
        
        private void OnBuildingClicked(Building _building)
        {
            if(CurrentSelectedBuilding != _building) FindObjectOfType<AudioManager>().Play("BuildingClicked");
            CurrentSelectedBuilding = _building;
            CurrentSelectedBuilding.IsBuildingHudOpen = true;
            
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