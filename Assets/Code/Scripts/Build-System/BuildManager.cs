// File     : BuildManager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

/* 
 * TODO: Beschreibung der Klasse
 */


using System;
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
        private Unit Unit;


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
            Building.OnBaseIsUnderConstruction += OnBaseCreated;
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
        /// Deactivates the base build slot, when the building is placed on ground.
        /// </summary>
        /// <param name="obj">Base class.</param>
        private void OnBaseCreated(Base obj)
        {
            DeactivateBaseSlot();
        }
        
        /// <summary>
        /// It triggers when the base ist built.  
        /// </summary>
        private void OnBaseCreated()
        {
            ActivateAllBuildSlots();
            DeactivateBaseSlot();
        }
        
        // Buttons
        
        /// <summary>
        /// Opens the build hud.
        /// </summary>
        public void OnButton_BuildMenu()
        {
            OpenHud();
        }

        
        // Mouse Handling ----------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Event when player clicked od a build slot.
        /// </summary>
        /// <param name="_buildingPrefab">GameObject to Instantiate in the scene.</param>
        private void OnMouseClickedBuildSlot(GameObject _buildingPrefab)
        {
            AudioManager.GetInstance.PlaySound("BuildSlotClicked");

            DestroySelectedBuildingObject();
            SetMeshActiveOfAllGrounds(true);
            
            CreateBuilding(_buildingPrefab);
            
            IsBuilding = true;
        }
        
        /// <summary>
        /// Handles the Mouse Inputs
        /// </summary>
        private void UpdateHandleMouseInput()
        {
            // Left Mouse Button
            if (Input.GetMouseButtonDown(0)) LeftMouseButtonClicked();
            
            // Right Mouse Button
            if (Input.GetMouseButtonDown(1)) RightMouseButtonClicked();

            // Extra Mouse Button Clicked
            if (Input.GetMouseButtonDown(3) || Input.GetKeyDown(KeyCode.R)) MiddleMouseButtonClicked();

        }
        
        // Left Mouse Button
        // TODO: Code verbessern
        
        /// <summary>
        /// If the play can build a building on the ground, then placed it on the ground position.
        /// </summary>
        private void LeftMouseButtonClicked()
        {
            if (CurrentBuilding.GetIsCollision || CanBuild == false)
            {
                AudioManager.GetInstance.PlaySound("CantBuild");
                return;
            }
            
            
            BuildingData data = CurrentBuilding.GetData;
            int currentBuildingLevel = CurrentBuilding.CurrentLevelH;

            int woodCosts = data.Levels[currentBuildingLevel].WoodCosts;
            int stoneCosts = data.Levels[currentBuildingLevel].StoneCosts;
            int steelCosts = data.Levels[currentBuildingLevel].SteelCosts;
                
            if (PlayerData.IsPlayerHavingEnoughResources(0, woodCosts,stoneCosts, steelCosts))
            {
                AudioManager.GetInstance.PlaySound("Build");
                PlayerData.ReduceResources(0, woodCosts, stoneCosts, steelCosts);
                CurrentBuilding.SetEntranceActive(false);

                CurrentBuilding.SetPosition(CurrentGround.GetWidth,CurrentGround.GetHeight);
                CurrentBuilding.SetEntranceGround(MapGenerator.GetGroundFromGlobalPosition(CurrentBuilding.GetEntrancePoss()));
                CurrentBuilding.SetConstructionActive(true);
                CurrentBuilding.SetBaseMaterial();

                for (int i = 0; i < CurrentBuilding.CurrentHeightH; i++)
                {
                    for (int j = 0; j < CurrentBuilding.CurrentWidthH; j++)
                    {
                        if (CurrentBuilding is BarbedWire)
                        {
                            UnitManager.GetNodes[CurrentGround.GetWidth + j, CurrentGround.GetHeight + i].IsWalkable = true;
                        }
                        else
                        {
                        
                            UnitManager.GetNodes[CurrentGround.GetWidth + j, CurrentGround.GetHeight + i].IsWalkable = false;
                        }
                        MapGenerator.SetGroundBlocked(CurrentGround.GetWidth + j,CurrentGround.GetHeight + i,true);
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
                AudioManager.GetInstance.PlaySound("CantBuild");
            }
        }

        // Right Mouse button
        
        /// <summary>
        /// Closes the building hud.
        /// </summary>
        private void RightMouseButtonClicked()
        {
            
            BuildUI.SetActive(false);

            if(IsBuilding)Destroy(CurrentBuildingObject);
            
            CurrentBuildingObject = null;
            IsBuilding = false;
            SetColliderActiveOfAllBuildings(true);
            SetMeshActiveOfAllGrounds(false);
        }
        
        /// <summary>
        /// Sets the Collider of all grounds active. If the player is building, the mouse raycast do not hit the building.
        /// </summary>
        /// <param name="_value">Active value for all collider.</param>
        private void SetColliderActiveOfAllBuildings(bool _value)
        {
            foreach (var building in Building.Buildings)
            {
                building.SetColliderActive(_value);
            }
        }

        /// <summary>
        /// Activates or deactivates the ground meshes.
        /// </summary>
        /// <param name="_value">Active value for all grounds.</param>
        private void SetMeshActiveOfAllGrounds(bool _value)
        {
            foreach (var ground in Ground.Grounds)
            {
                ground.SetMeshActive(_value);
            }
        }
        
        // Extra Mouse Button
        
        /// <summary>
        /// Turns the Current Building
        /// </summary>
        private void MiddleMouseButtonClicked()
        {
            CurrentBuilding.TurnRight();
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
                
                UpdateHandleMouseInput();
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
            if (CurrentBuilding is BarbedWire)
            {
                if (IsBuildingOnStreet(_ground))
                {
                    isGroundBlocked = false;
                }
                else
                {
                    isGroundBlocked = true;
                }
            }
            bool isEntranceOnStreet = IsEntranceOnStreet(_building);

            
            
            if (!isGroundBlocked && isEntranceOnStreet) return true;
            
            return false;
        }

        private bool IsBuildingOnStreet(Ground _ground)
        {
            for (int i = 0; i < CurrentBuilding.CurrentHeightH; i++)
            {
                for (int j = 0; j < CurrentBuilding.CurrentWidthH; j++)
                {
                    Ground ground = MapGenerator.GetGroundFromPosition(_ground.GetWidth + j, _ground.GetHeight + i);
                    if (ground.GetGroundSignature != EGround.Street) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks that one of all ground position of a building are blocked.
        /// </summary>
        /// <param name="_building">Building to check.</param>
        /// <param name="_ground">Ground, where the building is right now.</param>
        /// <returns>Returns true if one ground of the building is blocked.</returns>
        private bool IsBuildingGroundsBlocked(Building _building, Ground _ground)
        {
            for (int y = 0; y < _building.CurrentHeightH; y++)
            {
                for (int x = 0; x < _building.CurrentWidthH; x++)
                {
                    // current x and y position
                    int xPosition = x + _ground.GetWidth;
                    int yPosition = y + _ground.GetHeight;




                    bool isUnitOnGround = IsUnitOnGround(xPosition, yPosition);

                    if (MapGenerator.IsGroundBlocked(xPosition ,yPosition) || isUnitOnGround) return true;
                }
            }

            return false;
        }

        private bool IsUnitOnGround(int _x, int _y)
        {
            
            
            if (_x < 0 || _y < 0 || _x >=  UnitManager.GetNodes.GetLength(0) || _y >= UnitManager.GetNodes.GetLength(1)) return true;
            if (UnitManager.GetNodes.GetLength(1) < _y +1 || UnitManager.GetNodes.GetLength(0) < _x + 1)
            {
                return true;
            }

            return UnitManager.GetNodes[_x, _y].IsUnit;
        }
        
        /// <summary>
        /// Checks that the entrance position is on a ground with the signature street.
        /// </summary>
        /// <param name="_building">Hands over a building to check the entrance.</param>
        /// <returns>Returns true if the entrance of the building is on a ground with street signature.</returns>
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
        
        /// <summary>
        /// Event. When a building is destroyed, create scrap on the building ground positins.
        /// </summary>
        /// <param name="_building"></param>
        public void OnBuildingDestroyed(Building _building)
        {

            int amountOfScrap = _building.CurrentHeightH * _building.CurrentHeightH;
            int buildingLevel = _building.CurrentLevelH;


            int woodAmount = 0;
            int stoneAmount = 0;
            int steelAmount = 0;
            for (int i = buildingLevel; i >= 0; i--)
            {
                woodAmount  +=  _building.GetData.Levels[i].WoodCosts;
                stoneAmount += _building.GetData.Levels[i].StoneCosts;
                steelAmount += _building.GetData.Levels[i].SteelCosts;
            }

            woodAmount /= amountOfScrap;
            stoneAmount /= amountOfScrap;
            steelAmount /= amountOfScrap;
            for (int i = 0; i < _building.CurrentHeightH; i++)
            {
                for (int j = 0; j < _building.CurrentWidthH; j++)
                {
                    
                    
                    SetScrapOnPos(_building.GetXPos + j,_building.GetYPOs + i ,woodAmount,stoneAmount,steelAmount);
                }
            }
        }
        
        // UI ----------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Opens the hud from a unit. 
        /// </summary>
        /// <param name="_unit">Hands over a unit, where the player opens the hud.</param>
        public void OpenHudFromUnit(Unit _unit)
        {
            Unit = _unit;
            OpenHud();
        }

        /// <summary>
        /// Sets the UI active state.
        /// </summary>
        /// <param name="_isActive">State of the UI.</param>
        public override void SetUIActive(bool _isActive)
        {
            BuildUI.SetActive(_isActive);
        }

        // -------------------------------------------------------------------------------------------------------------

        // Generates TODO: Verbessern
        public void SetScrapOnPos(int _x, int _y, int _wood, int _stone, int _steel)
        {
            GameObject scrap = Instantiate(PrefScrapBuilding);
            
            scrap.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
            scrap.GetComponent<Scrap>().SetPosition(_x,_y);
            scrap.GetComponent<Scrap>().AddRes(EResource.Wood,_wood);
            scrap.GetComponent<Scrap>().AddRes(EResource.Stone,_stone);
            scrap.GetComponent<Scrap>().AddRes(EResource.Steel,_steel);
            
            MapGenerator.SetGroundBlocked(_x,_y,true);
            UnitManager.GetNodes[_x, _y].IsWalkable = true;
            scrap.GetComponent<Scrap>().SetEntranceGround(MapGenerator.GetGroundFromGlobalPosition( scrap.GetComponent<Scrap>().GetEntrancePoss()));

        }

        public void SetDestroyedHouseOnPos(int _x, int _y, int _wood, int _stone, int _steel)
        {
            GameObject destroyedHouse = Instantiate(PrefDestroyedHouse);

             int rand = Random.Range(0, 4);
             
             for (int i = 0; i < rand; i++)
             {
                 destroyedHouse.GetComponent<Building>().TurnRight();
                 
                 
             }

            destroyedHouse.transform.position = MapGenerator.GetGroundFromPosition(_x, _y).transform.position;
            bool canBuild = CanBuildingBuildOnGround(destroyedHouse.GetComponent<Building>(), MapGenerator.GetGroundFromPosition(_x, _y));

            
            for (int i = 0; i < 3; i++)
            {
                if(canBuild) break;
                
                destroyedHouse.GetComponent<Building>().TurnRight();
                canBuild = CanBuildingBuildOnGround(destroyedHouse.GetComponent<Building>(), MapGenerator.GetGroundFromPosition(_x, _y));
            }
            

            if(canBuild)
            {
                
                destroyedHouse.GetComponent<Building>().SetPosition(_x,_y);
                
                for (int i = 0; i < destroyedHouse.GetComponent<Building>().CurrentHeightH; i++)
                {
                    for (int j = 0; j < destroyedHouse.GetComponent<Building>().CurrentWidthH; j++)
                    {
                        MapGenerator.SetGroundBlocked(_x + j,_y + i,true);
                        UnitManager.GetNodes[_x + j, _y + i].IsWalkable = false;
                    }
                }
                
                destroyedHouse.GetComponent<Building>().SetEntranceGround(MapGenerator.GetGroundFromGlobalPosition( destroyedHouse.GetComponent<Building>().GetEntrancePoss()));
                
                destroyedHouse.GetComponent<Scrap>().AddRes(EResource.Wood,_wood);
                destroyedHouse.GetComponent<Scrap>().AddRes(EResource.Stone,_stone);
                destroyedHouse.GetComponent<Scrap>().AddRes(EResource.Steel,_steel);
                
                return;
            }
            Destroy(destroyedHouse.gameObject);
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