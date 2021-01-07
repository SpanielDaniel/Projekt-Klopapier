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

        private Camera MainCamera;
        
        
        [SerializeField] private BuildingData buildingData;
        [SerializeField] private int CurrentLevel = 0;
        [SerializeField] private int CurrentHealth = 0;
        [SerializeField] private GameObject HealthBar;
        [SerializeField] private Slider HealthBarSlider;

        [SerializeField] private BoxCollider BoxCollider;

        [SerializeField] private MeshRenderer MeshRenderer;
        [SerializeField] private Material BuildMaterial;
        [SerializeField] private Material BuildingMaterial;
        [SerializeField] private Material CantBuildMaterial;
        public BuildingData GetBuildingData => buildingData;
        
        public int CurrentHealthH
        {
            set
            {
                CurrentHealth = Mathf.Clamp(value, 0, buildingData.Levels[CurrentLevel].MaxLife);
                ValueChanged?.Invoke();
                LifeChanged?.Invoke(CurrentHealth,buildingData.Levels[CurrentLevel].MaxLife);
            }
            get => CurrentHealth;
        }

        public int CurrentLevelH
        {
            set
            {
                CurrentLevel = Mathf.Clamp(value, 0, buildingData.Levels.Length - 1);
                ValueChanged?.Invoke();
            }
            get => CurrentLevel;
        }

        public void Init(BuildingData buildingData)
        {
            this.buildingData = buildingData;
            CurrentHealthH = this.buildingData.Levels[CurrentLevel].MaxLife;
            AddBuilding();
        }

        #endregion
        
        private void Start()
        {
            HealthBar.SetActive(false);
            MainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            RotateHealthBarToCamera();
        }


        #region Functions

        private void RotateHealthBarToCamera()
        {
            Quaternion cameraRotation = MainCamera.transform.rotation; 
            HealthBar.transform.localRotation = new Quaternion(cameraRotation.x,cameraRotation.y,cameraRotation.z,cameraRotation.w);
        }

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
            int maxLevel = buildingData.Levels.Length - 1;

            if (nextLevel > maxLevel) return;
            
            CurrentLevelH = nextLevel;
            CurrentHealthH = buildingData.Levels[CurrentLevel].MaxLife;
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
            CurrentHealthH = CurrentHealthH;
        }

        #endregion
        
        public void OnMouseEnterAction()
        {
            HealthBar.SetActive(true);
        }

        public void OnMouseStayAction()
        {
            HealthBarSlider.maxValue = buildingData.Levels[CurrentLevel].MaxLife;
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

        public void SetColliderActive(bool _isActive)
        {
            BoxCollider.enabled = _isActive;
        }
        
        
        
    }
}