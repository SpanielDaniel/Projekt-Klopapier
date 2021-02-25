using System;
using Buildings;
using TMPro;
using UI_Scripts;
using UnityEngine;

namespace Code.Scripts.Buildings.UIElements
{
    public class UIFarm : MonoBehaviour
    {
        [SerializeField] private UISlot[] Slots;
        
        [SerializeField] private TextMeshProUGUI FoodPerDay;

        public void UpdateUI(Farm _farm)
        {
            foreach (UISlot slot in Slots)
            {
                slot.SetDefaultSprite();
                
            }
            for (int i = 0; i < _farm.GetUnitIDs.Count; i++)
            {
                Slots[i].Init(Unit.Units[_farm.GetUnitIDs[i]].GetUnitData.Icon  ,  _farm.GetUnitIDs[i]);
            }

            FoodPerDay.text = "+ " + Math.Round(_farm.GetFoodPerDay,1) + "/Tag";
        }
    }
}