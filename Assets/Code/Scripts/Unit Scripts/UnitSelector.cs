﻿// File     : UnitSelector.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.UI_Scripts;
using Buildings;
using System;
using System.Collections.Generic;
using Code.Scripts;
using Code.Scripts.Grid.DanielB;
using Code.Scripts.Map;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    
    public static event Action<Unit> SelectUnit;
    public static event Action<List<Unit>> SelectedUnitGroup;
    public static event Action NoUnitSelected;
    public static event Action SelectionChanged;
    
    private static List<Unit> SelectedUnits = new List<Unit>();
    
    
    [SerializeField] private Vector2 testEndNode;
    public static List<Unit> SelectedUnitsH
    {
        get => SelectedUnits;
        set 
        {
            
            // if (SelectedUnits != value)
            // {
            //     SelectionChanged?.Invoke();
            // } 
            SelectedUnits = value;
            
        }
    }
    [SerializeField] private Camera CameraMain;
    [SerializeField] private RectTransform SelectionBox;
    [SerializeField] private UIUnitManager UIUnitManager;
    [SerializeField] private UnitManager UnitManager;
    [SerializeField] private BuildManager BuildManager;
    
    private Vector2 startPos;

    private Ground CurrentGround;

    public RectTransform GetSelectionBox => SelectionBox;

    private void Start()
    {
        SelectedUnitsH = new List<Unit>();

        Unit.OnSelection += AddUnit;
    }

    private void AddUnit(Unit obj)
    {
        ClearSelectedUnit();
        SelectedUnitsH.Add(obj);
        CheckUnitsCount();
    }

    private void Update()
    {

        if (SelectedUnitsH.Count > 0)
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            LayerMask mask = LayerMask.GetMask("Ground");

            if (Physics.Raycast(ray, out hit, 100, mask)) ;
            {
                if (hit.transform)
                {
                    Ground ground = hit.transform.GetComponent<Ground>();
                    if (ground != null)
                    {
                        if (ground != CurrentGround && CurrentGround != null) CurrentGround.SetUnitMeshActive(false);
                        CurrentGround = ground;
                        CurrentGround.SetUnitMeshActive(true);
                    }
                }
            }
        }

        
        if (Input.GetMouseButtonDown(0))
        {
            if (!UIPointerInHudManager.GetIsInHut && !BuildManager.GetIsBuilding)
            {
                ClearSelectedUnit();
                if (CurrentGround != null)
                {
                    CurrentGround.SetUnitMeshActive(false);
                    CurrentGround = null;
                }
            }
            
            startPos = Input.mousePosition;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
             ReleaseSelectionBox();
        }
        
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedUnitsH.Count > 0)
            {
                Ray ray;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                LayerMask mask = LayerMask.GetMask("Ground") + LayerMask.GetMask("Building");
                
                if (Physics.Raycast(ray, out hit,100,mask))
                {
                    Ground ground = hit.transform.GetComponent<Ground>();
                    if (ground != null)
                    {
                        foreach (Unit unit in SelectedUnitsH)
                        {
                            unit.CancelMovingIntoBuilding();

                        }
                        MoveUnits(ground.GetWidth, ground.GetHeight);
                    }
                    Building building = hit.transform.GetComponent<Building>();
                   
                    if (building != null)
                    {
                        if(building.GetUnitCanEnter) MoveAllUnitsIntoBuilding(building);
                        else FindObjectOfType<AudioManager>().Play("CantBuild");                   
                    }
                }
            }
        }
    }

    [SerializeField] private MapGenerator MapGenerator;
    public void MoveUnitsIntoBuilding(Unit _unit,Building _building)
    {
        
        _unit.MoveIntoBuilding(_building);
        Ground ground = MapGenerator.GetGroundFromGlobalPosition(_building.GetEntrancePoss());
        if (ground != null)
        {
            MoveUnitToGround(_unit, ground);
        }
    }

    private void MoveAllUnitsIntoBuilding(Building _building)
    {
        foreach (Unit unit in SelectedUnitsH)
        {
            MoveUnitsIntoBuilding(unit,_building);
        }
    }

    private void MoveUnitToGround(Unit _unit, Ground _ground)
    {
        UnitManager.FindPathForUnit(_unit, _ground.GetWidth,_ground.GetHeight);
    }

    public void MoveUnits(int _endPosX,int _endPosZ)
    {
        if (SelectedUnitsH[0].GetXPosition == _endPosX && SelectedUnitsH[0].GetZPosition == _endPosZ) return;
        //UnitManager.FindPathForUnit(SelectedUnitsH[0],_endPosX,_endPosZ);

        //ToDo: Einheiten am Ende verteilen
        //Idee: die endPos am anfang für jede Unit nochmal addieren 
        //Problem: Wenn man zu der neuen Pos nicht hin kann wird sich die Unit nicht bewegen

        //List<Vector3> endPositionList = GroupUnitPositionList(new Vector3(_endPosX,0.1f, _endPosZ), 0.5f, SelectedUnitsH.Count);


        for (int i = 0; i < SelectedUnitsH.Count; i++)
        {
            UnitManager.FindPathForUnit(SelectedUnitsH[i], _endPosX, _endPosZ);
        }
    }

    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!SelectionBox.gameObject.activeInHierarchy)
            SelectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        SelectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    void ReleaseSelectionBox()
    {
        SelectionBox.gameObject.SetActive(false);
        
        Vector2 min = SelectionBox.anchoredPosition - (SelectionBox.sizeDelta / 2);
        Vector2 max = SelectionBox.anchoredPosition + (SelectionBox.sizeDelta / 2);

        foreach (Unit units in Unit.Units)
        {
            units.IsSelected = false;
            
            if (units.isActiveAndEnabled)
            {
                Vector3 screenPos = CameraMain.WorldToScreenPoint(units.transform.position);

                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
                {
                    SelectedUnitsH.Add(units);
                    units.IsSelected = true;
                }
            }
        }
        
        CheckUnitsCount();
    }

    private void CheckUnitsCount()
    {
        if (SelectedUnitsH.Count == 1)
        {
            SelectUnit?.Invoke(SelectedUnitsH[0]);
        }
        else
        {
            NoUnitSelected?.Invoke();
            if (SelectedUnitsH.Count > 1)
            {
                SelectedUnitGroup?.Invoke(SelectedUnitsH);
            }
        }


        if (SelectedUnitsH.Count > 0)
        {
            AudioManager.GetInstance.Play("YesSir");

            foreach (Unit unit in SelectedUnitsH)
            {
                unit.SetSelectedGround(true);
            }
        }
    }

    private void ClearSelectedUnit()
    {
        
        foreach (Unit _unit in SelectedUnitsH)
        {
            _unit.SetSelectedGround(false);
        }
        SelectedUnitsH.Clear();
        SelectionChanged?.Invoke();
    }


    
}
