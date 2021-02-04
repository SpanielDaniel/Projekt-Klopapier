// File     : UIUnitManager.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using Assets.Code.Scripts.Unit_Scripts;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Code.Scripts.UI_Scripts
{
    public class UIUnitManager : MonoBehaviour
    {
        private UnitData Data;
        [SerializeField] private GameObject UnitUI;
        [SerializeField] private TMP_Text NameUI;
        [SerializeField] private Image UnitIcon; 
        [SerializeField] private TMP_Text AttackDamageUI;
        [SerializeField] private TMP_Text AttackSpeedUI;
        [SerializeField] private TMP_Text VerteidigungUI;
        [SerializeField] private TMP_Text SpeedUI;
        [SerializeField] private TMP_Text HealthPointsUI;
        [SerializeField] private GameObject UnitGroupUI;
        [SerializeField] private ScrollRect UnitGroupRect;

        private void Awake()
        {
            Unit.OnSelection += UnitSelected;
            UnitSelector.SelectUnit += UnitSelected;
            UnitSelector.SelectedUnitGroup += UnitGroupSelected;
            UnitSelector.NoUnitSelected += CloseHud;
        }

        public void UnitSelected(Unit _unit)
        {
            UnitUI.SetActive(true);
            UnitGroupUI.SetActive(false);
            
            UnitIcon.sprite = _unit.GetUnitData.Icon;
            NameUI.text = _unit.GetUnitData.Name;
            AttackDamageUI.text = _unit.GetUnitData.AttackPoints.ToString();
            AttackSpeedUI.text = _unit.GetUnitData.AttackSpeed.ToString();
            VerteidigungUI.text = _unit.GetUnitData.Defence.ToString();
            SpeedUI.text = _unit.GetUnitData.MoveSpeed.ToString();
            HealthPointsUI.text = _unit.GetUnitData.MaxHealthPoints.ToString();
        }

        private void CloseHud()
        {
            UnitUI.SetActive(false);

        }

        public void UnitGroupSelected(List<Unit> _units)
        {
            UnitGroupUI.SetActive(true);
        }

    }
}
