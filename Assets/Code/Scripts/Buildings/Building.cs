

using System;
using System.Collections.Generic;
using Code.Scripts.Map;
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
        public static event Action<Building> IsFinished;
        public static event Action<Building,int,int> IsDestroied;
        
        public static List<Building> Buildings = new List<Building>();
        public bool IsCollison;

        private Camera MainCamera;
        private int i = 0;
        private bool IsBuilding;
        private float TimerCounter = 0;
        private int HealthPerSecond = 1;
        public bool GetIsBuilding => IsBuilding; 
        
        [SerializeField] private BuildingData BuildingData;
        [SerializeField] private GameObject HealthBar;
        [SerializeField] private GameObject BuildingObj;
        [SerializeField] private Slider HealthBarSlider;
        [SerializeField] private BoxCollider BoxCollider;
        [SerializeField] private MeshRenderer MeshRenderer;
        [SerializeField] private Material BuildMaterial;
        [SerializeField] private Material BuildingMaterial;
        [SerializeField] private Material CantBuildMaterial;
        [SerializeField] private int CurrentLevel = 0;
        [SerializeField] private int CurrentHealth = 0;
        [SerializeField] private int CurrentWidth;
        [SerializeField] private int CurrentHeight;
        [SerializeField] private int MaxUnitAmount = 5;
        [SerializeField] private int UnitAmount = 0;

        private int XPosition;
        private int YPosition;
        
        
        
        
        public BuildingData GetBuildingData => BuildingData;

        public int CurrentHeightH
        {
            get => CurrentHeight;
            set => CurrentHeight = value;
        }
        
        public int CurrentWidthH
        {
            get => CurrentWidth;
            set => CurrentWidth = value;
        }
        
        public int CurrentHealthH
        {
            set
            {
                CurrentHealth = Mathf.Clamp(value, 0, BuildingData.Levels[CurrentLevel].MaxLife);
                ValueChanged?.Invoke();
                LifeChanged?.Invoke(CurrentHealth,BuildingData.Levels[CurrentLevel].MaxLife);
                if(CurrentHealth <= 0) DestroyEffect();
            }
            get => CurrentHealth;
        }

        public int CurrentLevelH
        {
            set
            {
                CurrentLevel = Mathf.Clamp(value, 0, BuildingData.Levels.Length - 1);
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
        
        private void Update()
        {
            if (IsBuilding)
            {
                TimerCounter += Time.deltaTime;
                if (TimerCounter >= 1)
                {
                    TimerCounter--;
                    CurrentHealthH += UnitAmount * HealthPerSecond;
                    if (CurrentHealthH == BuildingData.Levels[CurrentLevel].MaxLife)
                    {
                        IsFinished?.Invoke(this);
                        OnBuildEffect();
                        IsBuilding = false;
                    }
                }
            }
        }
        
        private void Init()
        {
            MainCamera = Camera.main;
            CurrentHealthH = 1;
            AddBuilding();

            CurrentWidth = BuildingData.ObjectSize.X;
            CurrentHeight = BuildingData.ObjectSize.Y;
            
            IsBuilding = true;
        }

        private void LateUpdate()
        {
            RotateHealthBarToCamera();
        }
        
        #region Functions

        private void RotateHealthBarToCamera()
        {
            Quaternion cameraRotation = MainCamera.transform.rotation; 
            HealthBar.transform.rotation = new Quaternion(cameraRotation.x,cameraRotation.y,cameraRotation.z,cameraRotation.w);
        }

        public void Turn()
        {
            i += 1;
            if (i > 3) i = 0;

            transform.localRotation = Quaternion.Euler(0,90 * i,0);//RotateAround(transform.position,new Vector3(0,1,0), 90f );

            
            int x = CurrentHeightH;
            int y = -CurrentWidthH;
            
            if (x < 0)
            {
                

                x = -x;
                BuildingObj.transform.position = new Vector3(
                    BuildingObj.transform.position.x + 0.5f * (x),
                    BuildingObj.transform.position.y,
                    BuildingObj.transform.position.z);
               
                
            }
            
            if (y < 0)
            {
                y = -y;
                BuildingObj.transform.position = new Vector3(
                    BuildingObj.transform.position.x ,
                    BuildingObj.transform.position.y,
                    BuildingObj.transform.position.z+ 0.5f * (y));
            }

            CurrentWidthH = x;
            CurrentHeightH = y;
            
            
            UpdateBoxCollider();
        }

        private void UpdateBoxCollider()
        {
            if (i % 2 != 0)
            {
                Vector3 boxCenter = BoxCollider.center;
                BoxCollider.center = new Vector3(-boxCenter.x  ,boxCenter.y,boxCenter.z);
            }
            else
            {
                Vector3 boxCenter = BoxCollider.center;
                BoxCollider.center = new Vector3(boxCenter.x  ,boxCenter.y,-boxCenter.z);
            }
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
            return CurrentLevelH + 1 > BuildingData.Levels.Length;
        }

        private void SetHealthToCurrentLevelHealth()
        {
            CurrentHealthH = BuildingData.Levels[CurrentLevel].MaxLife;
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
            Debug.Log("Stay");
            if (other.transform.GetComponent<Building>() == null) return;
            IsCollison = true;
        }

        private void OnCollisionExit(Collision other)
        {
            Debug.Log("kollisionExit");
            if (other.transform.GetComponent<Building>() == null) return;
            IsCollison = false;
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
            HealthBarSlider.maxValue = BuildingData.Levels[CurrentLevel].MaxLife;
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

        private void OnDestroy()
        {
            Buildings.Remove(this);
        }

        public virtual void OnBuildEffect(){}

        public void SetPos(int _x, int _y)
        {
            XPosition = _x;
            YPosition = _y;
        }

        public virtual void DestroyEffect()
        {
            IsDestroied?.Invoke(this,XPosition,YPosition);
            Buildings.Remove(this);
        }
        
    }

    
}