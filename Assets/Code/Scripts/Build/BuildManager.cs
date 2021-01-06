// File     : BuildManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier


using System;
using Buildings;
using Code.Scripts;
using Code.Scripts.Grid.DanielB;
using Player;
using UI_Scripts;
using UnityEditorInternal;
using UnityEngine;

namespace Build
{
    public class BuildManager : UIVisibilityEvent
    {
       
        [SerializeField] private GameObject BuildingPrefab;
        [SerializeField] private GameObject BuildUI;
        [SerializeField] private PlayerData PlayerData;
        [SerializeField] private BuildMap Map;



        private Ground GroundCurrent;
        private bool IsBuilding = false;
        private bool CanBuild;
        public bool GetIsBuilding => IsBuilding;
        
        private GameObject CurrentBuilding;
        
        private void Awake()
        {
            BuildSlot.OnMouseClick += OnMouseClickedBuildSlot;
        }

        private void Start()
        {
            IsHudOpenH = false;
        }

        private void OnMouseClickedBuildSlot(GameObject _building)
        {
            if (IsBuilding)
            {
                DestroyCurrentBuilding();
            }
            
            CreateBuilding(_building);
            IsBuilding = true;
        }

        private void DestroyCurrentBuilding()
        {
            Destroy(CurrentBuilding);
            CurrentBuilding = null;
        }

        private void CreateBuilding(GameObject _building)
        {
            CurrentBuilding = Instantiate(_building);
            CurrentBuilding.GetComponent<Building>().Init(_building.GetComponent<Building>().GetComponent<Building>().GetBuildingData);
        }

        public void BuildBuilding()
        {
            if (!IsBuilding) return;
            
            foreach (var building in Building.Buildings)
            {
                building.SetColliderActive(false);
            }
            
            Vector3 mousePos = Input.mousePosition;
            
            if (mousePos.x > 0 && mousePos.x < Screen.width && mousePos.y > 0 && mousePos.y < Screen.height)
            {
                SetBuildingToMousePos();
                
                HandleMouseInput();
                
                foreach (var building in Building.Buildings)
                {
                    building.SetColliderActive(true);
                }
                
            }
        }

        private void SetBuildingToMousePos()
        {
            Vector3 mousePos;
            Vector3 cameraDirection;
            RaycastHit hit;
           
            Ray ray;
            Ground ground;
            bool isHit;
            bool isGround;

            cameraDirection = Camera.main.transform.forward;
            mousePos = Input.mousePosition;
            ray = Camera.main.ScreenPointToRay(mousePos);
            isHit = Physics.Raycast(ray, out hit);
            
            if (isHit)
            {
                ground = hit.transform.GetComponent<Ground>();

                if (ground != null)
                {
                    bool isBlocked = false;
                    
                    for (int i = 0; i < CurrentBuilding.GetComponent<Building>().GetBuildingData.ObjectSize.Y; i++)
                    {
                        for (int j = 0; j < CurrentBuilding.GetComponent<Building>().GetBuildingData.ObjectSize.X; j++)
                        {
                            
                            isBlocked = Map.IsGroundBlocked((j + ground.GetWidth) ,(i + ground.GetHeight));
                            if (isBlocked == true) break;
                        }
                        if (isBlocked == true) break;
                        
                    }
                    
                    if (isBlocked == true)
                    {
                        CanBuild = false;
                    }
                    else
                    {
                        CanBuild = true;
                    }

                    isGround = true;
                    GroundCurrent = ground;

                }
                else isGround = false;
                
                
                
                
                
                
                // while (ground == null)
                // {
                //     isGround = false;
                //     RaycastHit nextHit;
                //     ray = new Ray(hit.point + cameraDirection * 0.01f,cameraDirection);
                //     isHit = Physics.Raycast(ray, out nextHit);
                //     if (!isHit) break;
                //     
                //     hit = nextHit;
                //     
                //     ground = hit.transform.GetComponent<Ground>();
                //     if (ground != null)
                //     {
                //         isGround = true;
                //     }
                // }
                if (isGround)
                {
                    CurrentBuilding.transform.position = ground.transform.position;
                }
                if (isGround && CanBuild)
                {
                    int index = hit.triangleIndex;
                    //CurrentBuilding.transform.position = hit.point;

                    EGround groundSignature = ground.GetGroundSignature;

                    switch (groundSignature)
                    {

                        case EGround.Gras:
                            
                            CanBuild = true;
                            CurrentBuilding.GetComponent<Building>().SetBuildMaterial();
                            break;
                        default:
                            CurrentBuilding.GetComponent<Building>().SetCantBuildMaterial();
                            CanBuild = false;
                            break;
                    }
                    
                }
                else if(!CanBuild)
                {
                    CurrentBuilding.GetComponent<Building>().SetCantBuildMaterial();
                }
            }
        }
        
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0) && !CurrentBuilding.GetComponent<Building>().IsCollison && CanBuild)
            {
                BuildingData data = CurrentBuilding.GetComponent<Building>().GetBuildingData;

                if (PlayerData.IsPlayerHavingEnoughResources(
                    0,
                    data.Levels[CurrentBuilding.GetComponent<Building>().CurrentLevelH].WoodCosts,
                    data.Levels[CurrentBuilding.GetComponent<Building>().CurrentLevelH].StoneCosts,
                    data.Levels[CurrentBuilding.GetComponent<Building>().CurrentLevelH].SteelCosts))
                {
                    PlayerData.ReduceResources(0,
                        data.Levels[CurrentBuilding.GetComponent<Building>().CurrentLevelH].WoodCosts,
                        data.Levels[CurrentBuilding.GetComponent<Building>().CurrentLevelH].StoneCosts,
                        data.Levels[CurrentBuilding.GetComponent<Building>().CurrentLevelH].SteelCosts);
                
                    
                    CurrentBuilding.GetComponent<Building>().SetBuildingMaterial();
                    

                    for (int i = 0; i < CurrentBuilding.GetComponent<Building>().GetBuildingData.ObjectSize.Y; i++)
                    {
                        for (int j = 0; j < CurrentBuilding.GetComponent<Building>().GetBuildingData.ObjectSize.X; j++)
                        {
                            Map.SetGroundBlocked(GroundCurrent.GetWidth + j,GroundCurrent.GetHeight + i,true);
                        }
                    }
                    
                    BuildUI.SetActive(false);
                    IsBuilding = false;

                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                IsBuilding = false;
                BuildUI.SetActive(false);
                foreach (Street street in Street.Streets)
                {
                    street.SetAllArrowsActive(false);
                }
                
                Destroy(CurrentBuilding);
                CurrentBuilding = null;
            }
        }

        public void OnButton_BuildMenu()
        {
            IsHudOpenH = true;
        }

        public override void SetUIActive(bool _isActive)
        {
            BuildUI.SetActive(_isActive);
        }
    }
}