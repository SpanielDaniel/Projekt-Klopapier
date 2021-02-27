// File     : UIUnitManager.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System;
using Assets.Code.Scripts.Unit_Scripts;
using System.Collections.Generic;
using Code.Scripts;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Code.Scripts.UI_Scripts
{
    public class UIUnitManager : MonoBehaviour
    {
        public static event Action OnButtonGather;
        private UnitData Data;
        [SerializeField] private GameObject UnitUI;
        [SerializeField] private TMP_Text NameUI;
        [SerializeField] private TMP_Text NumberOfUnitsUI;
        [SerializeField] private Image UnitIcon;
        [SerializeField] private Image GroupUnitIcon;
        [SerializeField] private TMP_Text AttackDamageUI;
        [SerializeField] private TMP_Text AttackSpeedUI;
        [SerializeField] private TMP_Text VerteidigungUI;
        [SerializeField] private TMP_Text SpeedUI;
        [SerializeField] private TMP_Text HealthPointsUI;
        [SerializeField] private GameObject UnitGroupUI;

        [SerializeField] private BuildManager BuildManager;

        private void Awake()
        {
            Unit.OnSelection += UnitSelected;
            UnitSelector.SelectUnit += UnitSelected;
            UnitSelector.SelectedUnitGroup += UnitGroupSelected;
            UnitSelector.NoUnitSelected += CloseHud;
        }

        private Unit Unit;
        
        public void UnitSelected(Unit _unit)
        {
            Unit = _unit;
            UnitUI.SetActive(true);
            UnitGroupUI.SetActive(false);
            
            UnitIcon.sprite = _unit.GetUnitData.Icon;
            NameUI.text = _unit.GetUnitData.Name;
            AttackDamageUI.text = _unit.GetUnitData.AttackPoints.ToString();
            AttackSpeedUI.text = _unit.GetUnitData.AttackSpeed.ToString();
            VerteidigungUI.text = _unit.GetUnitData.Defence.ToString();
            SpeedUI.text = _unit.GetUnitData.MoveSpeed.ToString();
            HealthPointsUI.text = _unit.GetCurrentHealth.ToString();
        }

        private void CloseHud()
        {
            UnitUI.SetActive(false);
            UnitGroupUI.SetActive(false);
        }

        public void UnitGroupSelected(List<Unit> _units)
        {
            UnitGroupUI.SetActive(true);
            UnitUI.SetActive(false);
            int numberOfUnits = 0;

            foreach (Unit unit in _units)
            {
                numberOfUnits++;
            }
            NumberOfUnitsUI.text = numberOfUnits.ToString();
        }

        public void OnButtonClick_Build()
        {
            BuildManager.OpenHudFromUnit(Unit);
        }

        public void OnButtonClick_Gater()
        {
            OnButtonGather?.Invoke();
        }
        

    }
}
