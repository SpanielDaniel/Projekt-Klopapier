

using System;
using System.Collections.Generic;
using Code.Scripts.Map;
using Interfaces;
using Player;
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
        // -------------------------------------------------------------------------------------------------------------

        #region Init
        
        // Events ------------------------------------------------------------------------------------------------------
        public static event Action ValueChanged;
        public static event Action OnBaseIsUnderConstruction;
        public static event Action<Building> OnClick;
        public static event Action<Building> IsFinished;
        
        // Static Variables --------------------------------------------------------------------------------------------
        public static List<Building> Buildings = new List<Building>();
        
        // Serialize Field ---------------------------------------------------------------------------------------------
        
        [SerializeField] private BuildingData Data;
        
        [Space]
        
        [Header("Level")]
        [SerializeField] protected int Level;
        
        [Space]
        
        [Header("Health")]
        [SerializeField] private int Health;
        [Space]
        [SerializeField] private GameObject HealthBar;
        [SerializeField] private Slider HealthBarSlider;
        
        [Space]
        
        [Header("Units in Building")]
        [SerializeField] protected int UnitAmount = 0;
        
        [Space]
        
        [Header("Collider")]
        [SerializeField] private BoxCollider Collider;
        
        [Space]
        
        [Header("Model")]
        [SerializeField] private MeshRenderer MeshRenderer;
        [Space]
        [SerializeField] private Material BaseMaterial;
        [SerializeField] private Material BuildMaterial;
        [SerializeField] private Material CantBuildMaterial;
        
        [Space]
        
        [Header("Building Objects")]
        [SerializeField] private GameObject BuildingObj;
        // private -----------------------------------------------------------------------------------------------------

        private bool IsBuilt;
        private int XPosition;
        private int ZPosition;
        private int CurrentWidth;
        private int CurrentHeight;
        
        
        protected bool UnitCanEnter = true;
        private int MaxUnitAmount => Data.Levels[Level].MaxUnits;
        private List<int> UnitIDs = new List<int>();
        
        // Get properties ----------------------------------------------------------------------------------------------
        public BuildingData GetData => Data;
        public int GetXPos => XPosition;
        public int GetYPOs => ZPosition;
        public int CurrentHeightH => CurrentHeight;
        public int CurrentWidthH => CurrentWidth;
        public bool GetUnitCanEnter => UnitCanEnter;
        public List<int> GetUnitIDs => UnitIDs;

        // Handler properties ------------------------------------------------------------------------------------------
        
        public bool IsBuiltHandler
        {
            get => IsBuilt;
            set
            {
                IsBuilt = value;
                if (IsBuilt)
                {
                    OnBuildEffect();
                    IsFinished?.Invoke(this);
                }
            }
        }

        public int CurrentHealthH
        {
            get => Health;
            set
            {
                if (Data == null) return;
                Health = Mathf.Clamp(value, 0, Data.Levels[Level].MaxLife);
                ValueChanged?.Invoke();
                if(Health <= 0) DestroyEffect();
            }
        }

        public int CurrentLevelH
        {
            get => Level;
            private set
            {
                Level = Mathf.Clamp(value, 0, Data.Levels.Length - 1);
                ValueChanged?.Invoke();
            }
        }
        
        
        // TODO: umbenennen und an richtiger Stelle platzieren
        
        private int TurnIndex = 0;

        private int TurnIndexHandler
        {
            get => TurnIndex;
            set
            {
                TurnIndex = value;
                if (TurnIndex > 3) TurnIndex = 0;
                if (TurnIndex < 0) TurnIndex = 3;
            }
        }
        private float BuiltTimer = 0;
        private int HealthPerSecond = 1;

        private bool IsCollison;
        public bool GetIsCollision => IsCollison;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
        
        private void Init()
        {
            
            HealthBar.SetActive(false);
            AddBuilding();
        }

        private void Awake()
        {
            

            SetHealthToOne();
            
            UnitCanEnter = !Data.IsFinished;
            IsBuiltHandler = Data.IsFinished;
            SetStartObjectSize();
        }

        public Vector2 GetEntrancePoss()
        {
            return new Vector2((EntrancePos.transform.position.x - 0.25f),(EntrancePos.transform.position.z - 0.25f) );
        }

        private void Start()
        {
            Init();
        }
        
        private void Update()
        {
            if (!IsBuiltHandler) UpdateBuildProgress();
        }

        private void LateUpdate()
        {
            RotateHealthBarToCamera();
        }
        
        // -------------------------------------------------------------------------------------------------------------
        
        #region Functions
        
        // Init --------------------------------------------------------------------------------------------------------
        private void SetHealthToOne()
        {
            CurrentHealthH = 1;
        }
        private void SetStartObjectSize()
        {
            
            CurrentWidth = Data.ObjectSize.X;
            CurrentHeight = Data.ObjectSize.Y;
        }
        private void AddBuilding()
        {
            Buildings.Add(this);
        }
        
        // Update ------------------------------------------------------------------------------------------------------
        private void UpdateBuildProgress()
        {
            BuiltTimer += Time.deltaTime;
            if (BuiltTimer >= 1)
            {
                UpdateBuiltHealth();
                CheckThatBuildingIsFinished();
                BuiltTimer--;
            }
        }

        private void UpdateBuiltHealth() => CurrentHealthH += UnitAmount * HealthPerSecond;
        private void CheckThatBuildingIsFinished()
        {
            if (IsHealthEqualMaxHealth()) IsBuiltHandler = true;
        }
        private bool IsHealthEqualMaxHealth() => CurrentHealthH == Data.Levels[Level].MaxLife;

        // Late Update -------------------------------------------------------------------------------------------------
        private void RotateHealthBarToCamera()
        {
            Quaternion cameraRotation = Camera.main.transform.rotation; 
            HealthBar.transform.rotation = new Quaternion(cameraRotation.x,cameraRotation.y,cameraRotation.z,cameraRotation.w);
        }
        
        // Public Functions --------------------------------------------------------------------------------------------
        
        // Position ----------------------------------------------------------------------------------------------------
        public void SetPosition(int _x, int _y)
        {
            XPosition = _x;
            ZPosition = _y;
            
            //EntrancePosition[0] = new Vector2((EntrancePos.transform.localPosition.x * 4f),(EntrancePos.transform.localPosition.z * 4f));
        }

        
        
        // Rotation ----------------------------------------------------------------------------------------------------
        public void TurnRight()
        {
            RotateBuildingRight();
            
            UpdateBuildingPosition();
            
            UpdateBoxColliderPosition();
            
            
        }
        private void RotateBuildingRight()
        {
            TurnIndexHandler += 1;
            transform.localRotation = Quaternion.Euler(0,90 * TurnIndex,0);
        }
        private void UpdateBuildingPosition()
        {
            Vector3 position = BuildingObj.transform.position;

            int height = CurrentHeightH;
            if (height < 0)
            {
                height = -height;
                
                BuildingObj.transform.position = new Vector3(position.x + 0.5f * (height), position.y, position.z);

            }
            
            int width = -CurrentWidthH;
            if (width < 0)
            {
                width = -width;
                BuildingObj.transform.position = new Vector3(position.x,position.y,position.z+ 0.5f * (width));
                
            }
            CurrentWidth = height;
            CurrentHeight = width;

        }
        private void UpdateBoxColliderPosition()
        {
            if (TurnIndex % 2 != 0)
            {
                Vector3 boxCenter = Collider.center;
                Collider.center = new Vector3(-boxCenter.x  ,boxCenter.y,boxCenter.z);
            }
            else
            {
                Vector3 boxCenter = Collider.center;
                Collider.center = new Vector3(boxCenter.x  ,boxCenter.y,-boxCenter.z);
            }
        }

        // Upgrade -----------------------------------------------------------------------------------------------------
        public virtual void Upgrade()
        {
            if (IsNextLevelGreaterThanMaxLevel()) return;
            
            CurrentLevelH++;
            SetHealthToCurrentLevelHealth();
        }
        private bool IsNextLevelGreaterThanMaxLevel()
        {
            return CurrentLevelH + 1 > Data.Levels.Length;
        }
        private void SetHealthToCurrentLevelHealth()
        {
            CurrentHealthH = Data.Levels[Level].MaxLife;
        }
        
        // Units -------------------------------------------------------------------------------------------------------
        public bool AddUnit(int _unitId)
        {
            if (!UnitCanEnter){ return false; }
            UnitIDs.Add(_unitId);
            UnitAmount++;
            StartOnValueChanged();
            if (UnitAmount == MaxUnitAmount) UnitCanEnter = false;
            
            return true;
        }

        // Material Setter ---------------------------------------------------------------------------------------------
        public void SetBaseMaterial()
        {
            MeshRenderer.material = BaseMaterial;
            if (Buildings.Count == 1)
            {
                OnBaseIsUnderConstruction?.Invoke();
            }
        }
        public void SetBuildMaterial()
        {
            MeshRenderer.material = BuildMaterial;
            
        }
        public void SetCantBuildMaterial()
        {
            MeshRenderer.material = CantBuildMaterial;
        }
        
        // Collider Setter ---------------------------------------------------------------------------------------------
        public void SetColliderActive(bool _isActive)
        {
            Collider.enabled = _isActive;
        }

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------

        #region Events
        
        // Collision ---------------------------------------------------------------------------------------------------
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
        
        // Mouse -------------------------------------------------------------------------------------------------------
        public void OnMouseEnterAction()
        {
            HealthBar.SetActive(true);
        }

        public void OnMouseStayAction()
        {
            HealthBarSlider.maxValue = Data.Levels[Level].MaxLife;
            HealthBarSlider.value = Health;
        }

        public void OnMouseExitAction()
        {
            HealthBar.SetActive(false);
        }

        public void OnMouseLeftClickAction()
        {
            OnClick?.Invoke(this);
        }
        
        // On Built ----------------------------------------------------------------------------------------------------
        [SerializeField] private GameObject EntrancePos;
        protected virtual void OnBuildEffect()
        {
            
            
        }
        
        // On Destroy --------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            RemoveBuildingFromList();
        }
        private void RemoveBuildingFromList()
        {
            Buildings.Remove(this);
        }

        public virtual void DestroyEffect()
        {
            
        }

        // Inspector ---------------------------------------------------------------------------------------------------
        private void OnValidate()
        {
            if(Data != null) UpdateHealth();
        }
        private void UpdateHealth()
        {
            CurrentLevelH = CurrentLevelH;
            CurrentHealthH = CurrentHealthH;
        }

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
        
        protected void StartOnValueChanged()
        {
            ValueChanged?.Invoke();
        }
    }

    
}