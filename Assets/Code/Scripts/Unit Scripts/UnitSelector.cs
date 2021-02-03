﻿// File     : UnitSelector.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.UI_Scripts;
using Buildings;
using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static event Action<Unit> SelectUnit;
    public static event Action<List<Unit>> SelectedUnitGroup;
    public static List<Unit> SelectedUnits = new List<Unit>();
    [SerializeField] private Vector2 testEndNode;
    public static List<Unit> SelectedUnitsH
    {
        get => SelectedUnits;
        set 
        {
            SelectedUnits = value;
            if (SelectedUnits.Count == 1)
            {
                SelectUnit?.Invoke(SelectedUnits[0]);
            }
            else if (SelectedUnits.Count > 1)
            {
                SelectedUnitGroup?.Invoke(SelectedUnits);
            }
        } 
    }
    [SerializeField] private Camera CameraMain;
    [SerializeField] private RectTransform SelectionBox;
    [SerializeField] private UIUnitManager UIUnitManager;
    [SerializeField] private UnitManager UnitManager;
    private Vector2 startPos;

    private void Start()
    {
        SelectedUnitsH = new List<Unit>();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            SelectedUnitsH.Clear();
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
            if (SelectedUnits.Count > 0)
            {
                Ray ray;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Ground ground = hit.transform.GetComponent<Ground>();
                    if (ground != null)
                    {
                        Debug.Log(ground.GetWidth + "::" + ground.GetHeight);

                        MoveUnits(ground.GetWidth, ground.GetHeight);
                    }
                }
            }
        }
    }

    public void MoveUnits(int _endPosX,int _endPosZ)
    {
        if (SelectedUnitsH[0].GetXPosition == _endPosX && SelectedUnitsH[0].GetZPosition == _endPosZ) return;
        UnitManager.FindPathForUnit(SelectedUnitsH[0],_endPosX,_endPosZ);
        
        // //Ray ray;
        // //ray = Camera.main.ScreenPointToRay(_mousePosition);
        // //RaycastHit hit;
        // //if (Physics.Raycast(ray, out hit))
        // //{
        //     foreach (Unit unit in SelectedUnitsH)
        //     {
        //         //unit.MoveToPosition(_mousePosition);
        //         unit.SetTarget(_mousePosition);
        //         // Unit.IsEnterBuilding = true;
        //         // Unit.MoveTo(grid.Position)
        //         //{geht zum Gebäude hin if Distance <= 1}
        //     }
        // //}
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
            Vector3 screenPos = CameraMain.WorldToScreenPoint(units.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectedUnitsH.Add(units);
                units.IsSelected = true;
            }
            else
            {
                units.IsSelected = false;
            }
        }
    }
}
