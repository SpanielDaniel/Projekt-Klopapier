// File     : BuildManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier


using System;
using Buildings;
using Code.Scripts;
using Code.Scripts.Grid.DanielB;
using Code.Scripts.Map;
using Player;
using UI_Scripts;
using UnityEditorInternal;
using UnityEngine;

namespace Build
{
    public class BuildManager : UIVisibilityEvent
    {
        [SerializeField] private GameObject PrefScrapBuilding;
        [SerializeField] private GameObject BuildUI;
        [SerializeField] private PlayerData PlayerData;
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private Camera MainCamera;
        
        private Ground CurrentGround;
        private bool IsBuilding = false;
        private bool CanBuild;
        private GameObject CurrentBuildingObject;
        private Building CurrentBuilding;
        
        public bool GetIsBuilding => IsBuilding;

        private void Awake()
        {
            BuildSlot.OnMouseClick += OnMouseClickedBuildSlot;
            Building.IsDestroied += OnBuildingDestried;
        }

        private void Start()
        {
            if(MainCamera == null) MainCamera = Camera.main;
            CloseHud();
        }

        private void OnMouseClickedBuildSlot(GameObject _building)
        {
            
            if (IsBuilding)
            {
                DestroySelectedBuildingObject();
            }
            FindObjectOfType<AudioManager>().Play("BuildSlotClicked");
            
            
            SetMeshActiveOfAllGrounds(true);
            CreateBuilding(_building);
            
            IsBuilding = true;
        }

        private void DestroySelectedBuildingObject()
        {
            Destroy(CurrentBuildingObject);
            CurrentBuildingObject = null;
        }

        private void CreateBuilding(GameObject _building)
        {
            CurrentBuildingObject = Instantiate(_building);
            CurrentBuilding = CurrentBuildingObject.GetComponent<Building>();
        }

        public void BuildBuilding()
        {
            
            if (!IsBuilding) return;
            
            SetColliderActiveOfAllBuildings(false);
            
            
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x > 0 && mousePos.x < Screen.width && mousePos.y > 0 && mousePos.y < Screen.height)
            {
                SetBuildingToMousePos();
                
                HandleMouseInput();
            }
        }

        private void SetColliderActiveOfAllBuildings(bool _value)
        {
            foreach (var building in Building.Buildings)
            {
                building.SetColliderActive(_value);
            }
        }

        private void SetMeshActiveOfAllGrounds(bool _value)
        {
            foreach (var ground in Ground.Grounds)
            {
                ground.SetMeshActive(_value);
            }
        }
        
        private void SetBuildingToMousePos()
        {
            Vector3 mousePos;
            Vector3 cameraDirection;
            RaycastHit hit;
            Ground ground;
            Ray ray;
            bool isHit;
           

            cameraDirection = MainCamera.transform.forward;
            mousePos = Input.mousePosition;
            ray = MainCamera.ScreenPointToRay(mousePos);
            isHit = Physics.Raycast(ray, out hit);
            
            
            if (isHit && IsMouseOnGround(hit, out ground))
            {
                CurrentGround = ground;
                SetBuildingPositionToGroundPosition(ground);

                CanBuild = CanBuildingBuildOnGround(ground);
                
                if (CanBuild) CurrentBuilding.SetBuildMaterial();
                else CurrentBuilding.SetCantBuildMaterial();
            }
        }

        private void SetBuildingPositionToGroundPosition(Ground _ground)
        {
            CurrentBuildingObject.transform.position = _ground.transform.position;
        }

        private bool IsMouseOnGround(RaycastHit _hit, out Ground _ground)
        {
            Ground ground = _hit.transform.GetComponent<Ground>();

            if (ground != null)
            {
                _ground = ground;
                return true;
            }
            else
            {
                _ground = null;
                return false;
            }
        }
        
        private bool CanBuildingBuildOnGround(Ground _ground)
        {
            BuildingData data = CurrentBuilding.GetBuildingData;
                    
            for (int i = 0; i < CurrentBuilding.CurrentHeightH; i++)
            {
                for (int j = 0; j < CurrentBuilding.CurrentWidthH; j++)
                {
                    int x = j + _ground.GetWidth;
                    int y = i + _ground.GetHeight;

                    
                    
                    bool isBlocked = mapGenerator.IsGroundBlocked(x ,y);
                    if (isBlocked == true) return false;
                    
                    
                    // TO_DO: Besser machen
                    switch (mapGenerator.GetGroundSignature(x,y))
                    {
                        case EGround.None:
                            return false;
                        case EGround.Street:
                            return false;
                        case EGround.Gras:
                            break;
                        default:
                            return false;
                    }
                }
            }

            return true;
        }

        private void HandleMouseInput()
        {
            // Left Mouse Button
            if (Input.GetMouseButtonDown(0)) LeftMouseButtonClicked();
            
            // Right Mouse Button
            if (Input.GetMouseButtonDown(1)) RightMouseButtonClicked();

            if (Input.GetMouseButtonDown(3)) MiddleMouseButtonClicked();

        }

        private void LeftMouseButtonClicked()
        {
            if (CurrentBuilding.IsCollison || CanBuild == false)
            {
                FindObjectOfType<AudioManager>().Play("CantBuild");
                return;
            }
            
            
            BuildingData data = CurrentBuilding.GetBuildingData;
            int currentBuildingLevel = CurrentBuilding.CurrentLevelH;

            int woodCosts = data.Levels[currentBuildingLevel].WoodCosts;
            int stoneCosts = data.Levels[currentBuildingLevel].StoneCosts;
            int steelCosts = data.Levels[currentBuildingLevel].SteelCosts;
                
            if (PlayerData.IsPlayerHavingEnoughResources(0, woodCosts,stoneCosts, steelCosts))
            {
                FindObjectOfType<AudioManager>().Play("Build");
                PlayerData.ReduceResources(0, woodCosts, stoneCosts, steelCosts);
                CurrentBuilding.SetBuildingMaterial();
                
                
                CurrentBuilding.SetPos(CurrentGround.GetWidth,CurrentGround.GetHeight);

                for (int i = 0; i < CurrentBuilding.CurrentHeightH; i++)
                {
                    for (int j = 0; j < CurrentBuilding.CurrentWidthH; j++)
                    {
                        mapGenerator.SetGroundBlocked(CurrentGround.GetWidth + j,CurrentGround.GetHeight + i,true);
                    }
                }
                    
                BuildUI.SetActive(false);
                IsBuilding = false;
                    
                SetColliderActiveOfAllBuildings(true);
                SetMeshActiveOfAllGrounds(false);
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("CantBuild");
            }
        }

        private void RightMouseButtonClicked()
        {
            IsBuilding = false;
            BuildUI.SetActive(false);

            Destroy(CurrentBuildingObject);
            CurrentBuildingObject = null;
                
            SetColliderActiveOfAllBuildings(true);
            SetMeshActiveOfAllGrounds(false);
        }
        
        private void MiddleMouseButtonClicked()
        {
            CurrentBuilding.Turn();
        }

        public void OnButton_BuildMenu()
        {
            OpenHud();
        }

        public override void SetUIActive(bool _isActive)
        {
            BuildUI.SetActive(_isActive);
        }

        public void OnBuildingDestried(Building _building,int _x, int _y)
        {
            for (int i = 0; i < _building.CurrentHeightH; i++)
            {
                for (int j = 0; j < _building.CurrentWidthH; j++)
                {
                    GameObject scrap = Instantiate(PrefScrapBuilding);
                    scrap.transform.position = mapGenerator.GetGroundFromPosition(_x + j, _y + i).transform.position;
                }
            }
            
        }
        
    }
}