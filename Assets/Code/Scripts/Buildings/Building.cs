using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class Building : MonoBehaviour
    , IMouseEnter
    , IMouseStay
    , IMouseExit
    , IMouseLeftClick
    {
        #region Init

        public static event Action ValueChanged;
        public static event Action<float,float> LifeChanged;
        public static event Action<Building> OnClick;
        public static List<Building> Buildings = new List<Building>();
        public bool IsCollison;
        
        
        [SerializeField] private BuildingStats BuildingStats;
        [SerializeField] private int CurrentLevel = 0;
        [SerializeField] private int CurrentHealth = 0;
        [SerializeField] private GameObject HealthBar;
        [SerializeField] private Slider HealthBarSlider;


        [SerializeField] private MeshRenderer MeshRenderer;
        [SerializeField] private Material BuildMaterial;
        [SerializeField] private Material BuildingMaterial;
        [SerializeField] private Material CantBuildMaterial;
        public BuildingStats GetBuildingStats => BuildingStats;
        
        
        
        public int CurrenthealthH
        {
            set
            {
                CurrentHealth = Mathf.Clamp(value, 0, BuildingStats.Levels[CurrentLevel].MaxLife);
                ValueChanged?.Invoke();
                LifeChanged?.Invoke(CurrentHealth,BuildingStats.Levels[CurrentLevel].MaxLife);
            }
            get => CurrentHealth;
        }

        public int CurrentLevelH
        {
            set
            {
                CurrentLevel = Mathf.Clamp(value, 0, BuildingStats.Levels.Length - 1);
                ValueChanged?.Invoke();
            }
            get => CurrentLevel;
        }

        public void Init(BuildingStats _buildingStats)
        {
            BuildingStats = _buildingStats;
            CurrenthealthH = BuildingStats.Levels[CurrentLevel].MaxLife;
            AddBuilding();
        }

        #endregion
        
        
        
        private void Start()
        {
            HealthBar.SetActive(false);
        }

        private void LateUpdate()
        {
            Quaternion cameraRotation = Camera.main.transform.rotation; 
            HealthBar.transform.localRotation = new Quaternion(cameraRotation.x,cameraRotation.y,cameraRotation.z,cameraRotation.w);
        }

        #region Functions

        private void AddBuilding()
        {
            Buildings.Add(this);
        }
        private void RemoveBuilding()
        {
            Buildings.Remove(this);
        }
        public void Upgrade()
        {
            int nextLevel = CurrentLevelH + 1;
            int maxLevel = BuildingStats.Levels.Length - 1;

            if (nextLevel > maxLevel) return;
            
            CurrentLevelH = nextLevel;
            CurrenthealthH = BuildingStats.Levels[CurrentLevel].MaxLife;
        }

        public void SetBuildMaterial()
        {
            MeshRenderer.material = BuildMaterial;

        }
        
        public void SetCantBuildMaterial()
        {
            MeshRenderer.material = CantBuildMaterial;

        }

        public void SetBuildingMaterial()
        {
            MeshRenderer.material = BuildingMaterial;
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.transform.GetComponent<Building>() == null) return;
            IsCollison = true;
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.GetComponent<Building>() == null) return;
            IsCollison = false;
        }

        #endregion

        #region Events
        
        private void OnDestroy()
        {
            Buildings.Remove(this);
        }

        private void OnValidate()
        {
            CurrentLevelH = CurrentLevelH;
            CurrenthealthH = CurrenthealthH;
        }

        #endregion

        
        
        public void OnMouseEnterAction()
        {
            HealthBar.SetActive(true);
        }

        public void OnMouseStayAction()
        {
            HealthBarSlider.maxValue = BuildingStats.Levels[CurrentLevel].MaxLife;
            HealthBarSlider.value = CurrentHealth;
        }

        public void OnMouseExitAction()
        {
            HealthBar.SetActive(false);
        }

        public void OnMouseLeftClickAction()
        {
            OnClick?.Invoke(this);
        }
    }
}