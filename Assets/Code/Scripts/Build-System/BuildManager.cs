// File     : BuildManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

/* 
 * TODO: Beschreibung der Klasse
 */


using Assets.Code.Scripts.Unit_Scripts;
using Buildings;
using Code.Scripts.Map;
using Player;
using UI_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    public class BuildManager : UIVisibilityEvent
    {
        // -------------------------------------------------------------------------------------------------------------

        #region Init
        
        // Serialize Fields --------------------------------------------------------------------------------------------
        
        [SerializeField] private Camera MainCamera;
        [SerializeField] private PlayerData PlayerData;
        [Space]
        [Header("Manager")]
        [SerializeField] private UnitManager UnitManager;
        [SerializeField] private MapGenerator MapGenerator;
        [SerializeField] private UnitSelector UnitSelector;

        [Space]
        [Header("Prefabs")]
        [SerializeField] private GameObject PrefScrapBuilding;
        [SerializeField] private GameObject PrefDestroyedHouse;
        [SerializeField] private GameObject PrefHouse;
        [Space]
        [Header("UI")]
        [SerializeField] private GameObject BuildUI;
        [SerializeField] private GameObject[] BuildSlots;

        // private -----------------------------------------------------------------------------------------------------

        private GameObject CurrentBuildingObject;
        private Building CurrentBuilding;
        private Ground CurrentGround;
        private bool IsBuilding = false;
        private bool CanBuild;

        // public ------------------------------------------------------------------------------------------------------
        // Get properties ----------------------------------------------------------------------------------------------
        public bool GetIsBuilding => IsBuilding;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            AddEvents();
        }

        private void Start()
        {
            SetMainCamera();

            /*
             * The player can only build the base until the base is built.
             * So only the base build slot is active.
             */
            DeactivateAllBuildSlots();
            ActivateBaseBuildSlot();
            
            CloseHud();
        }
        
        // -------------------------------------------------------------------------------------------------------------

        #region Functions
        
        // Awake -------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Adding events.
        /// </summary>
        private void AddEvents()
        {
            Base.OnBaseCreated += OnBaseCreated;
            Base.OnBaseCreated += DeactivateBaseSlot;
            BuildSlot.OnMouseClick += OnMouseClickedBuildSlot;
            Building.OnBaseIsUnderConstruction += DeactivateBaseSlot;
            UnitSelector.SelectionChanged += RightMouseButtonClicked;
        }


        // Start -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Sets the main camera.
        /// </summary>
        private void SetMainCamera()
        {
            if(MainCamera == null) MainCamera = Camera.main;
        }

        // Events ------------------------------------------------------------------------------------------------------

        /// <summary>
        /// It triggers when the base ist built.  
        /// </summary>
        private void OnBaseCreated()
        {
            ActivateAllBuildSlots();
            DeactivateBaseSlot();
        }
        
        /// <summary>
        /// Event when player clicked od a build slot.
        /// </summary>
        /// <param name="_buildingPrefab">GameObject to Instantiate in the scene.</param>
        private void OnMouseClickedBuildSlot(GameObject _buildingPrefab)
        {
            AudioManager.GetInstance.Play("BuildSlotClicked");

            DestroySelectedBuildingObject();
            SetMeshActiveOfAllGrounds(true);
            
            CreateBuilding(_buildingPrefab);
            
            IsBuilding = true;
        }
        
        // Slots -------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Activates all build slots.
        /// </summary>
        private void ActivateAllBuildSlots()
        {
            foreach (var slot in BuildSlots)
            {
                slot.SetActive(true);
            }
        }
        
        /// <summary>
        /// Deactivates all build slots.
        /// </summary>
        private void DeactivateAllBuildSlots()
        {
            foreach (GameObject slot in BuildSlots)
            {
                slot.SetActive(false);
            }
        }

        /// <summary>
        /// Activates the base build slot.
        /// </summary>
        private void ActivateBaseBuildSlot()
        {
            BuildSlots[0].SetActive(true);
        }

        /// <summary>
        /// Deactivates base slot.
        /// </summary>
        private void DeactivateBaseSlot()
        {
            BuildSlots[0].SetActive(false);
        }

        // Build Phase -------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Starts the building phase with the selected
        /// </summary>
        public void UpdateBuildBuilding()
        {
            //TODO: gehört hier nichtm, da es eine update funktion ist.
            CurrentBuilding.SetEntranceActive(true);
            SetColliderActiveOfAllBuildings(false);
            
            // The mouse must be inside the map.
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x > 0 && mousePos.x < Screen.width && mousePos.y > 0 && mousePos.y < Screen.height)
            {
                SetBuildingToMousePosGround();
                
                HandleMouseInput();
            }
        }
        
        /// <summary>
        /// Sets the building on the ground where the mouse is pointing
        /// </summary>
        private void SetBuildingToMousePosGround()
        {
            Vector3 mousePos = Input.mousePosition;;
            Ray ray = MainCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            
            // Raycast from screen to a ground
            bool isHit = Physics.Raycast(ray, out hit);
            if (!isHit) return;
            
            Ground ground;
            if (IsMouseHitPointingGround(hit, out ground))
            {
                CurrentGround = ground;
                CurrentBuildingObject.transform.position = ground.transform.position;

                CanBuild = CanBuildingBuildOnGround(CurrentBuilding,ground);
                
                // Changes the material of the building depend of the player can build the building on the ground.
                if (CanBuild) CurrentBuilding.SetBuildMaterial();
                else CurrentBuilding.SetCantBuildMaterial();
            }
        }
        
        /// <summary>
        /// Checks that the mouse raycast hit has the ground component
        /// </summary>
        /// <param name="_hit">Mouse hit.</param>
        /// <param name="_ground">Setting null, if the mouse hit is not a ground.</param>
        /// <returns>Returns true if the mouse hit is a ground.</returns>
        private bool IsMouseHitPointingGround(RaycastHit _hit, out Ground _ground)
        {
            Ground ground = _hit.transform.GetComponent<Ground>();

            if (ground != null)
            {
                _ground = ground;
                return true;
            }
            _ground = null;
            return false;
        }
        
        // Building ----------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Instantiate a building object. 
        /// </summary>
        /// <param name="_buildingPrefab">Building object to instantiate.</param>
        private void CreateBuilding(GameObject _buildingPrefab)
        {
            CurrentBuildingObject = Instantiate(_buildingPrefab);
            CurrentBuilding = CurrentBuildingObject.GetComponent<Building>();
        }

        /// <summary>
        /// Destroys the current building that the player want to build.
        /// </summary>
        private void DestroySelectedBuildingObject()
        {
            // The building cant be destroyed, if no building object is in the scene.
            if (!IsBuilding) return;
            
            Destroy(CurrentBuildingObject);
            ClearCurrentBuilding();
        }

        /// <summary>
        /// Clears the building data.
        /// </summary>
        private void ClearCurrentBuilding()
        {
            CurrentBuildingObject = null;
            CurrentBuilding = null;
        }

        /// <summary>
        /// Checks that the building can build on the ground.
        /// </summary>
        /// <param name="_building">Building to check that it can build on ground.</param>
        /// <param name="_ground">Ground where the building want to build on.</param>
        /// <returns>Returns true, if the building can build on the ground.</returns>
        private bool CanBuildingBuildOnGround(Building _building,Ground _ground)
        {
            bool isGroundBlocked = IsBuildingGroundsBlocked(_building, _ground);
            bool isEntranceOnStreet = IsEntranceOnStreet(_building);

            if (!isGroundBlocked && isEntranceOnStreet) return true;
            
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_building"></param>
        /// <param name="_ground"></param>
        /// <returns></returns>
        private bool IsBuildingGroundsBlocked(Building _building, Ground _ground)
        {
            for (int y = 0; y < _building.CurrentHeightH; y++)
            {
                for (int x = 0; x < _building.CurrentWidthH; x++)
                {
                    // current x and y position
                    int xPosition = x + _ground.GetWidth;
                    int yPosition = y + _ground.GetHeight;

                    if (MapGenerator.IsGroundBlocked(xPosition ,yPosition)) return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_building"></param>
        /// <returns></returns>
        private bool IsEntranceOnStreet(Building _building)
        {
            Ground g = MapGenerator.GetGroundFromGlobalPosition(_building.GetEntrancePoss());
            
            if(g != null)
            {
                if (g.GetGroundSignature == EGround.Street)
                {
                    return true;
                }
            }

            return false;
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
                CurrentBuilding.SetEntranceActive(false);
                
                CurrentBuilding.SetPosition(CurrentGround.GetWidth,CurrentGround.GetHeight);
                CurrentBuilding.SetEntranceGround(MapGenerator.GetGroundFromGlobalPosition(CurrentBuilding.GetEntrancePoss()));
                CurrentBuilding.SetConstructionActive(true);

                for (int i = 0; i < CurrentBuilding.CurrentHeightH; i++)
                {
                    for (int j = 0; j < CurrentBuilding.CurrentWidthH; j++)
                    {
                        MapGenerator.SetGroundBlocked(CurrentGround.GetWidth + j,CurrentGround.GetHeight + i,true);
                        
                        UnitManager.GetNodes[CurrentGround.GetWidth + j, CurrentGround.GetHeight + i].IsWalkable = false;
                    }
                }
                
                UnitSelector.MoveUnitsIntoBuilding(Unit,CurrentBuilding);

                Unit.MoveIntoBuilding(CurrentBuilding);

                BuildUI.SetActive(false);
                    
                SetColliderActiveOfAllBuildings(true);
                SetMeshActiveOfAllGrounds(false);
                CurrentBuilding = null;
                IsBuilding = false;
            }
            else
            {
                AudioManager.GetInstance.Play("CantBuild");
            }
        }

        private void RightMouseButtonClicked()
        {
            
            BuildUI.SetActive(false);

            if(IsBuilding)Destroy(CurrentBuildingObject);
            
            CurrentBuildingObject = null;
            IsBuilding = false;
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

        private Unit Unit;
        public void OpenHudFromUnit(Unit _unit)
        {
            Unit = _unit;
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
            UnitManager.GetNodes[_x, _y].IsWalkable = true;
            scrap.GetComponent<Scrap>().SetEntranceGround(MapGenerator.GetGroundFromGlobalPosition( scrap.GetComponent<Scrap>().GetEntrancePoss()));

        }

        public void SetDestroyedHouseOnPos(int _x, int _y)
        {
            GameObject DestroyedHouse = Instantiate(PrefDestroyedHouse);

             int rand = Random.Range(0, 4);
             
             for (int i = 0; i < rand; i++)
             {
                 DestroyedHouse.GetComponent<Building>().TurnRight();
             }

            DestroyedHouse.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
            bool canBuild = CanBuildingBuildOnGround(DestroyedHouse.GetComponent<Building>(), MapGenerator.GetGroundFromPosition(_x, _y));

            
            for (int i = 0; i < 3; i++)
            {
                if(canBuild) break;
                
                DestroyedHouse.GetComponent<Building>().TurnRight();
                canBuild = CanBuildingBuildOnGround(DestroyedHouse.GetComponent<Building>(), MapGenerator.GetGroundFromPosition(_x, _y));
            }
            

            if(canBuild)
            {
                
                DestroyedHouse.GetComponent<Building>().SetPosition(_x,_y);
                
                for (int i = 0; i < DestroyedHouse.GetComponent<Building>().CurrentHeightH; i++)
                {
                    for (int j = 0; j < DestroyedHouse.GetComponent<Building>().CurrentWidthH; j++)
                    {
                        MapGenerator.SetGroundBlocked(_x + j,_y + i,true);
                        UnitManager.GetNodes[_x + j, _y + i].IsWalkable = false;
                    }
                }
                
                DestroyedHouse.GetComponent<Building>().SetEntranceGround(MapGenerator.GetGroundFromGlobalPosition( DestroyedHouse.GetComponent<Building>().GetEntrancePoss()));
                return;
            }
            Destroy(DestroyedHouse.gameObject);
        }
        
        public void SetHouseOnPos(int _x, int _y)
        {
            GameObject house = Instantiate(PrefHouse);

            int rand = Random.Range(0, 4);
             
            for (int i = 0; i < rand; i++)
            {
                house.GetComponent<Building>().TurnRight();
            }

            house.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
            bool canBuild = CanBuildingBuildOnGround(house.GetComponent<Building>(), MapGenerator.GetGroundFromPosition(_x, _y));

            
            for (int i = 0; i < 3; i++)
            {
                if(canBuild) break;
                
                house.GetComponent<Building>().TurnRight();
                canBuild = CanBuildingBuildOnGround(house.GetComponent<Building>(), MapGenerator.GetGroundFromPosition(_x, _y));
            }

           //bool canBuld = CanBuildingBuildOnGround(house.GetComponent<Building>(), MapGenerator.GetGroundFromPosition(_x, _y));
            if(canBuild)
            {
                
                //house.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
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
                house.GetComponent<Building>().SetEntranceGround(MapGenerator.GetGroundFromGlobalPosition( house.GetComponent<Building>().GetEntrancePoss()));
                return;
            }
            Destroy(house.gameObject);
        }
        
        #endregion
    }
}