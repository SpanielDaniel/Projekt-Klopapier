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
        [SerializeField] private GameObject BuildingObj;
        [SerializeField] private int CurrentWidth;
        [SerializeField] private int CurrentHeight;
        

       public int GetCurrentHeight
       {
           get => CurrentHeight;
           set => CurrentHeight = value;
       }
       public int GetCurrentWidth
       {
           get => CurrentWidth;
           set => CurrentWidth = value;
       }
        
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

        #endregion
        
        private void Start()
        {
            Init();
            HealthBar.SetActive(false);
        }
        
        private void Init()
        {
            MainCamera = Camera.main;
            SetHealthToCurrentLevelHealth();
            AddBuilding();

            CurrentWidth = buildingData.ObjectSize.X;
            CurrentHeight = buildingData.ObjectSize.Y;
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

        private int i = 0;
        public void Turn()
        {
            i += 1;
            if (i > 3) i = 0;

            transform.localRotation = Quaternion.Euler(0,90 * i,0);//RotateAround(transform.position,new Vector3(0,1,0), 90f );

            //int buffer = GetCurrentWidth;
            //GetCurrentWidth = Curr
            
            //BuildingObj.transform.localPosition = new Vector3(0.25f,0,0.25f);
            //
            int x = GetCurrentHeight;
            int y = -GetCurrentWidth;
            
            if (x < 0)
            {
                x = -x;
                BuildingObj.transform.position = new Vector3(
                    BuildingObj.transform.position.x + 0.5f * (x - 1),
                    BuildingObj.transform.position.y,
                    BuildingObj.transform.position.z);
            }
            
            if (y < 0)
            {
                y = -y;
                BuildingObj.transform.position = new Vector3(
                    BuildingObj.transform.position.x ,
                    BuildingObj.transform.position.y,
                    BuildingObj.transform.position.z+ 0.5f * (y - 1));
            }

            
            GetCurrentWidth = x;
            GetCurrentHeight = y;
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
            if (IsNextLevelGreaterThanMaxLevel()) return;
            
            CurrentLevelH++;
            SetHealthToCurrentLevelHealth();
        }

        private bool IsNextLevelGreaterThanMaxLevel()
        {
            return CurrentLevelH + 1 > buildingData.Levels.Length;
        }

        private void SetHealthToCurrentLevelHealth()
        {
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

        #endregion

        #region Events
        
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