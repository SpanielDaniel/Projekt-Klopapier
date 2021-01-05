// File     : BuildManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier


using System;
using Buildings;
using Code.Scripts;
using UI_Scripts;
using UnityEngine;

namespace Build
{
    public class BuildManager : UIVisibilityEvent
    {
       
        [SerializeField] private GameObject BuildingPrefab;
        [SerializeField] private GameObject BuildUI;
       
        
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

        private void OnMouseClickedBuildSlot(BuildingData buildingData)
        {
            if (IsBuilding)
            {
                DestroyCurrentBuilding();
            }
            
            CreateBuilding(buildingData);
            IsBuilding = true;
        }

        private void DestroyCurrentBuilding()
        {
            Destroy(CurrentBuilding);
            CurrentBuilding = null;
        }

        private void CreateBuilding(BuildingData buildingData)
        {
            CurrentBuilding = Instantiate(BuildingPrefab);
            CurrentBuilding.GetComponent<Building>().Init(buildingData);
        }

        public void BuildBuilding()
        {
            if (!IsBuilding) return;

            foreach (Street street in Street.Streets)
            {
                street.SetAllArrowsActive(true);
            }
            
            Vector3 mousePos = Input.mousePosition;
            
            if (mousePos.x > 0 && mousePos.x < Screen.width && mousePos.y > 0 && mousePos.y < Screen.height)
            {
                SetBuildingToMousePos();
                HandleMouseInput();
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
                isGround = true;
                ground = hit.transform.GetComponent<Ground>();
                while (ground == null)
                {
                    isGround = false;
                    RaycastHit nextHit;
                    ray = new Ray(hit.point + cameraDirection * 0.01f,cameraDirection);
                    isHit = Physics.Raycast(ray, out nextHit);
                    if (!isHit) break;
                    
                    hit = nextHit;
                    
                    ground = hit.transform.GetComponent<Ground>();
                    if (ground != null)
                    {
                        isGround = true;
                    }
                }
                
                if (isGround)
                {
                    int index = hit.triangleIndex;
                    CurrentBuilding.transform.position = hit.point;

                    EGround groundSignature = ground.GetGroundSignature;

                    switch (groundSignature)
                    {

                        case EGround.Street:
                            CurrentBuilding.GetComponent<Building>().SetBuildMaterial();
                            Street street = ground as Street;
                            
                            CanBuild = true;

                            switch (index)
                            {
                                case 0:
                                    if (street.GetIsTopStreet)
                                    {
                                        CanBuild = false;
                                    }
                                    else
                                    {
                                        CurrentBuilding.transform.position = street.GetPosTop;
                                    }
                                    break;
                                case 1:
                                    if (street.GetIsRightStreet)
                                    {
                                        CanBuild = false;
                                    }
                                    else
                                    {
                                        CurrentBuilding.transform.position = street.GetPosRight;
                                    }

                                    break;
                                case 2:
                                    if (street.GetIsLeftStreet)
                                    {
                                        CanBuild = false;
                                    }
                                    else
                                    {
                                        CurrentBuilding.transform.position = street.GetPosLeft;
                                    }

                                    break;
                                case 3:
                                    if (street.GetIsDownStreet)
                                    {
                                        CanBuild = false;
                                    }
                                    else
                                    {
                                        CurrentBuilding.transform.position = street.GetPosDown;
                                    }

                                    break;
                                default:
                                    break;
                            }
                            
                            
                            
                            break;
                        default:
                            CurrentBuilding.GetComponent<Building>().SetCantBuildMaterial();
                            CanBuild = false;
                            break;
                    }

                    if (!CanBuild)
                    {
                        CurrentBuilding.GetComponent<Building>().SetCantBuildMaterial();
                    }
                }
            }
        }
        
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0) && !CurrentBuilding.GetComponent<Building>().IsCollison && CanBuild)
            {
                IsBuilding = false;
                CurrentBuilding.GetComponent<Building>().SetBuildingMaterial();
                BuildUI.SetActive(false);
                foreach (Street street in Street.Streets)
                {
                    street.SetAllArrowsActive(false);
                }
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