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
        }

        public void UnitSelected(Unit _unit)
        {
            UnitUI.SetActive(true);
            AttackDamageUI.SetText(Data.AttackPoints.ToString());
            AttackSpeedUI.SetText(Data.AttackSpeed.ToString());
            VerteidigungUI.SetText(Data.Defence.ToString());
            SpeedUI.SetText(Data.MoveSpeed.ToString());
            HealthPointsUI.SetText(Data.MaxHealthPoints.ToString());
        }

        public void UnitGroupSelected(List<Unit> _units)
        {
            UnitGroupUI.SetActive(true);
            for (int i = 0; i < _units.Count; i++)
            {
                Image go = Instantiate(UnitIcon, UnitGroupRect.transform, false);
            }
        }

    }
}
