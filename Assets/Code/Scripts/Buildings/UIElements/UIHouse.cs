using Buildings;
using UI_Scripts;
using UnityEngine;

namespace Code.Scripts.Buildings.UIElements
{
    public class UIHouse : MonoBehaviour
    {
        [SerializeField] private UISlot[] FrontSlots;
        [SerializeField] private UISlot[] BackSlots;
        [SerializeField] private UISlot[] LeftSlots;
        [SerializeField] private UISlot[] RightSlots;


        public void UpdateUI(House _house)
        {
            foreach (var slot in FrontSlots)
            {
                slot.SetDefaultSprite();
            }
            
            foreach (var slot in BackSlots)
            {
                slot.SetDefaultSprite();
            }
            
            foreach (var slot in LeftSlots)
            {
                slot.SetDefaultSprite();
            }
            foreach (var slot in RightSlots)
            {
                slot.SetDefaultSprite();
            }
            for (int i = 0; i < _house.GetFrontSideUnitIDs.Length; i++)
            {
                
                int unitID = _house.GetFrontSideUnitIDs[i];
                if (unitID >= 0 ) FrontSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
            
            for (int i = 0; i < _house.GetBackSideUnitIDs.Length; i++)
            {
                BackSlots[i].SetDefaultSprite();
                int unitID = _house.GetBackSideUnitIDs[i];
                if (unitID >= 0 ) BackSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
            
            for (int i = 0; i < _house.GetLeftSideUnitIDs.Length; i++)
            {
                LeftSlots[i].SetDefaultSprite();
                int unitID = _house.GetLeftSideUnitIDs[i];
                if (unitID >= 0 ) LeftSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
            
            for (int i = 0; i < _house.GetRightSideUnitIDs.Length; i++)
            {
                RightSlots[i].SetDefaultSprite();
                int unitID = _house.GetRightSideUnitIDs[i];
                if (unitID >= 0 ) RightSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
        }

    }
}