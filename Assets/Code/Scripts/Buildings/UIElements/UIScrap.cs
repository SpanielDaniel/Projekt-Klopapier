// File     : UIScrap.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Buildings;
using TMPro;
using UI_Scripts;
using UnityEngine;

namespace Code.Scripts.UI_Scripts
{
    public class UIScrap : MonoBehaviour
    {
        [SerializeField] private UISlot[] Slots;
        [SerializeField] private TextMeshProUGUI WoodText;
        [SerializeField] private TextMeshProUGUI AmountWood;
        
        [SerializeField] private TextMeshProUGUI StoneText;
        [SerializeField] private TextMeshProUGUI AmountStone;
        
        [SerializeField] private TextMeshProUGUI SteelText;
        [SerializeField] private TextMeshProUGUI AmountSteel;

        public void UpdateUI(Scrap _scrap)
        {
            
            foreach (UISlot slot in Slots)
            {
                slot.SetDefaultSprite();
            }
            for (int i = 0; i < _scrap.GetUnitIDs.Count; i++)
            {
                Slots[i].Init(Unit.Units[_scrap.GetUnitIDs[i]].GetUnitData.Icon  ,  _scrap.GetUnitIDs[i]);
            }

            AmountWood.text = _scrap.GetAmountOfWood.ToString();
            AmountStone.text = _scrap.GetAmountOfStone.ToString();
            AmountSteel.text = _scrap.GetAmountOfSteel.ToString();
        }
    }
}