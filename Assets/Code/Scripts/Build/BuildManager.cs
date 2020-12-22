// File     : BuildManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier


using Buildings;
using UI_Scripts;
using UnityEngine;

namespace Build
{
    public class BuildManager : HudBase
    {
       
        [SerializeField] private GameObject BuildingPrefab;
        [SerializeField] private GameObject BuildUI;
       
        
        private bool IsBuilding = false;
        public bool GetIsBuilding => IsBuilding;
        
        private GameObject CurrentBuilding;
        
        private void Awake()
        {
            BuildSlot.OnMouseClick += CreateBuilding;
        }

        private void Start()
        {
            IsHudOpenH = false;
        }

        private void CreateBuilding(BuildingStats _buildingStats)
        {
            if (IsBuilding)
            {
                Destroy(CurrentBuilding);
                CurrentBuilding = null;
            }
            CurrentBuilding = Instantiate(BuildingPrefab);
            CurrentBuilding.GetComponent<Building>().Init(_buildingStats);
            IsBuilding = true;
        }

        public void BuildBuilding()
        {
            Vector3 mousePos = Input.mousePosition;
            
            if (IsBuilding && mousePos.x > 0 && mousePos.x < Screen.width && mousePos.y > 0 && mousePos.y < Screen.height)
            {
                
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (  Physics.Raycast(ray, out hit) )
                {
                    bool isPlane = true;
                    
                    while (hit.transform.gameObject.name != "Plane")
                    {
                        isPlane = false;
                        Vector3 cameraDirection = Camera.main.transform.forward;
                        ray = new Ray(hit.point + cameraDirection * 0.01f,cameraDirection);
                        RaycastHit hit2;
                        if (!Physics.Raycast(ray, out hit2))
                        {
                            break;
                        }
//
                        if (hit2.transform.name == "Plane")
                        {
                            isPlane = true;
                        }
                        hit = hit2;
                    }
                    
                    if (isPlane)
                    {
                        CurrentBuilding.transform.position = hit.point;
                        if (CurrentBuilding.GetComponent<Building>().IsCollison)
                        {
                            CurrentBuilding.GetComponent<Building>().SetCantBuildMaterial();
                        }
                        else
                        {
                            CurrentBuilding.GetComponent<Building>().SetBuildMaterial();
                        }
                    }
                }

                if (Input.GetMouseButtonDown(0) && !CurrentBuilding.GetComponent<Building>().IsCollison)
                {
                    IsBuilding = false;
                    CurrentBuilding.GetComponent<Building>().SetBuildingMaterial();
                    BuildUI.SetActive(false);
                    
                }
            }
        }

        public void OnButton_Buildmenue()
        {
            IsHudOpenH = true;
        }

        public override void SetUIActive(bool _isActive)
        {
            BuildUI.SetActive(_isActive);
        }
    }
}