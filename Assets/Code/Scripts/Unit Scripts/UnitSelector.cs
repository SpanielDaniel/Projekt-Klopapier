// File     : UnitSelector.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.UI_Scripts;
using Buildings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static event Action<Unit> SelectUnit;
    public static event Action<List<Unit>> SelectedUnitGroup;
    public static List<Unit> SelectedUnits = new List<Unit>();
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
    private Vector2 startPos;

    private void Start()
    {
        SelectedUnitsH = new List<Unit>();
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
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
    }

    public static void MoveUnits()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            foreach (Unit unit in SelectedUnits)
            {
                unit.MoveToPosition((int)ray.origin.x, (int)ray.origin.z);
                unit.SetTarget(ray.origin);
                // Unit.IsEnterBuilding = true;
                // Unit.MoveTo(grid.Position)
                //{geht zum Gebäude hin if Distance <= 1}
            }
        }
    }

    void UpdateSelectionBox(Vector2 curMousePos)
    {
        SelectedUnitsH.Clear();
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

        SelectedUnits.Clear();
        foreach (Unit units in SelectedUnitsH)
        {
            Vector3 screenPos = CameraMain.WorldToScreenPoint(units.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectedUnitsH.Add(units);
            }
            else
            {
                //units.fPath = false;
            }
        }
    }
}
