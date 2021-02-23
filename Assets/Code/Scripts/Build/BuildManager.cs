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
using UnityEngine;
using Random = UnityEngine.Random;

namespace Build
{
    public class BuildManager : UIVisibilityEvent
    {
        [SerializeField] private GameObject PrefScrapBuilding;
        [SerializeField] private GameObject BuildUI;
        [SerializeField] private PlayerData PlayerData;
        [SerializeField] private MapGenerator MapGenerator;
        [SerializeField] private Camera MainCamera;

        [SerializeField] private GameObject[] BuildSlots;
        [SerializeField] private GameObject PrefDestroyedHouse;
        [SerializeField] private GameObject PrefHouse;
        [SerializeField] private UnitManager UnitManager;
        
        
        private Ground CurrentGround;
        private bool IsBuilding = false;
        private bool CanBuild;
        private GameObject CurrentBuildingObject;
        private Building CurrentBuilding;
        
        public bool GetIsBuilding => IsBuilding;

        private void Awake()
        {
            BuildSlot.OnMouseClick += OnMouseClickedBuildSlot;
            Base.OnBaseCreated += EnableSlots;
            Building.OnBaseIsUNderConstruction += DisableBaseSlot;
        }

        private void DisableBaseSlot()
        {
            BuildSlots[0].SetActive(false);
        }

        private void EnableSlots()
        {
            foreach (var slot in BuildSlots)
            {
                slot.SetActive(true);
            }
            BuildSlots[0].SetActive(false);
        }

        private void Start()
        {
            if(MainCamera == null) MainCamera = Camera.main;
            foreach (GameObject slot in BuildSlots)
            {
                slot.SetActive(false);
            }
            BuildSlots[0].SetActive(true);
            
            CloseHud();
        }

        private void OnMouseClickedBuildSlot(GameObject _building)
        {
            
            if (IsBuilding)
            {
                DestroySelectedBuildingObject();
            }
            AudioManager.GetInstance.Play("BuildSlotClicked");
            
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

                CanBuild = CanBuildingBuildOnGround(CurrentBuilding,ground);
                
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
        
        public bool CanBuildingBuildOnGround(Building _building,Ground _ground)
        {
            BuildingData data = _building.GetData;
                    
            for (int i = 0; i < _building.CurrentHeightH; i++)
            {
                for (int j = 0; j < _building.CurrentWidthH; j++)
                {
                    int x = j + _ground.GetWidth;
                    int y = i + _ground.GetHeight;

                    
                    
                    bool isBlocked = MapGenerator.IsGroundBlocked(x ,y);
                    if (isBlocked == true) return false;
                    
                    
                    
                    
                    // TO_DO: Besser machen
                    switch (MapGenerator.GetGroundSignature(x,y))
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
            
            if (MapGenerator.IsGroundBlocked(
                _ground.GetWidth + (int) (_building.GetEntrancePoss().x ),
                _ground.GetHeight + (int) (_building.GetEntrancePoss().y )))
            {
                Ground ground = MapGenerator.GetGroundFromPosition(
                    _ground.GetWidth + (int) (_building.GetEntrancePoss().x),
                    _ground.GetHeight + (int) (_building.GetEntrancePoss().y));

                if (ground != null)
                {
                    Debug.Log(ground.GetGroundSignature);
                    if (ground.GetGroundSignature == EGround.Street)
                    {
                        return true;
                    }
                }
                Debug.Log("ground is nul");
                
                
                return false;
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
            if (CurrentBuilding.GetIsCollision || CanBuild == false)
            {
                AudioManager.GetInstance.Play("CantBuild");
                return;
            }
            
            
            BuildingData data = CurrentBuilding.GetData;
            int currentBuildingLevel = CurrentBuilding.CurrentLevelH;

            int woodCosts = data.Levels[currentBuildingLevel].WoodCosts;
            int stoneCosts = data.Levels[currentBuildingLevel].StoneCosts;
            int steelCosts = data.Levels[currentBuildingLevel].SteelCosts;
                
            if (PlayerData.IsPlayerHavingEnoughResources(0, woodCosts,stoneCosts, steelCosts))
            {
                AudioManager.GetInstance.Play("Build");
                PlayerData.ReduceResources(0, woodCosts, stoneCosts, steelCosts);
                CurrentBuilding.SetBaseMaterial();
                
                
                CurrentBuilding.SetPosition(CurrentGround.GetWidth,CurrentGround.GetHeight);

                for (int i = 0; i < CurrentBuilding.CurrentHeightH; i++)
                {
                    for (int j = 0; j < CurrentBuilding.CurrentWidthH; j++)
                    {
                        MapGenerator.SetGroundBlocked(CurrentGround.GetWidth + j,CurrentGround.GetHeight + i,true);
                    }
                }
                    
                BuildUI.SetActive(false);
                IsBuilding = false;
                    
                SetColliderActiveOfAllBuildings(true);
                SetMeshActiveOfAllGrounds(false);
            }
            else
            {
                AudioManager.GetInstance.Play("CantBuild");
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
            CurrentBuilding.TurnRight();
        }

        public void OnButton_BuildMenu()
        {
            OpenHud();
        }

        public override void SetUIActive(bool _isActive)
        {
            BuildUI.SetActive(_isActive);
        }

        public void OnBuildingDestroyed(Building _building)
        {
            for (int i = 0; i < _building.CurrentHeightH; i++)
            {
                for (int j = 0; j < _building.CurrentWidthH; j++)
                {
                    SetScrapOnPos(_building.GetXPos + j,_building.GetYPOs + i);
                }
            }
        }

        public void SetScrapOnPos(int _x, int _y)
        {
            GameObject scrap = Instantiate(PrefScrapBuilding);
            scrap.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
            scrap.GetComponent<Scrap>().SetPosition(_x,_y);
            MapGenerator.SetGroundBlocked(_x,_y,true);
            UnitManager.GetNodes[_x, _y].IsWalkable = false;
        }

        public void SetDestroyedHouseOnPos(int _x, int _y)
        {
            GameObject DestroyedHouse = Instantiate(PrefDestroyedHouse);

            int rand = Random.Range(0, 2);

            
            if(rand >= 1) DestroyedHouse.GetComponent<Building>().TurnRight();
            
            
            if(CanBuildingBuildOnGround(DestroyedHouse.GetComponent<Building>() ,MapGenerator.GetGroundFromPosition(_x, _y)))
            {
                
                DestroyedHouse.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
                DestroyedHouse.GetComponent<Building>().SetPosition(_x,_y);
                
                for (int i = 0; i < DestroyedHouse.GetComponent<Building>().CurrentHeightH; i++)
                {
                    for (int j = 0; j < DestroyedHouse.GetComponent<Building>().CurrentWidthH; j++)
                    {
                        MapGenerator.SetGroundBlocked(_x + j,_y + i,true);
                        UnitManager.GetNodes[_x + j, _y + i].IsWalkable = false;
                    }
                }
                return;
            }
            Destroy(DestroyedHouse.gameObject);
            
        }

        
        public void SetHouseOnPos(int _x, int _y)
        {
            GameObject house = Instantiate(PrefHouse);

            int rand = Random.Range(0, 2);

            if(rand >= 1) house.GetComponent<Building>().TurnRight();
            
            
            if(CanBuildingBuildOnGround(house.GetComponent<Building>() ,MapGenerator.GetGroundFromPosition(_x, _y)))
            {
                
                house.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
                house.GetComponent<Building>().SetPosition(_x,_y);
                
                for (int i = 0; i < house.GetComponent<Building>().CurrentHeightH; i++)
                {
                    for (int j = 0; j < house.GetComponent<Building>().CurrentWidthH; j++)
                    {
                        MapGenerator.SetGroundBlocked(_x + j,_y + i,true);
                        UnitManager.GetNodes[_x + j, _y + i].IsWalkable = false;

                    }
                }
                    
                house.GetComponent<Building>().CurrentHealthH = house.GetComponent<Building>().GetData.Levels[house.GetComponent<Building>().CurrentLevelH].MaxLife;
                return;
            }
            Destroy(house.gameObject);
        }
    }
}