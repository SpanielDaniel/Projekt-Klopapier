using System;
using Buildings;
using UI_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.Buildings.UIElements
{
    public class UIHouse : MonoBehaviour
    {
        public static Action<UISlot> OnDragStarted; 
        public static Action<UISlot> OnEndDragStarted; 
        public static Action<UISlot> OnDropStarted; 
        
        [SerializeField] private UISlot[] FrontSlots;
        [SerializeField] private UISlot[] BackSlots;
        [SerializeField] private UISlot[] LeftSlots;
        [SerializeField] private UISlot[] RightSlots;


        public void UpdateUI(House _house)
        {
            
            for (int i = 0; i < _house.GetFrontSideUnitIDs.Length; i++)
            {
                if(i >= _house.GetMaxFrontAmount) FrontSlots[i].SetSlotActive(false);
                else
                {
                    FrontSlots[i].SetSlotActive(true);
                }
                
                FrontSlots[i].SetDefaultSprite();

                int unitID = _house.GetFrontSideUnitIDs[i];
                if (unitID >= 0)
                {
                    FrontSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
                }
            }
            
            for (int i = 0; i < _house.GetBackSideUnitIDs.Length; i++)
            {
                if(i >= _house.GetMaxBackUnitAmount) BackSlots[i].SetSlotActive(false);
                else BackSlots[i].SetSlotActive(true);
                
                BackSlots[i].SetDefaultSprite();
                
                int unitID = _house.GetBackSideUnitIDs[i];
                if (unitID >= 0 ) BackSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
            
            for (int i = 0; i < _house.GetLeftSideUnitIDs.Length; i++)
            {
                if(i >= _house.GetMaxLeftUnitAmount) LeftSlots[i].SetSlotActive(false);
                else LeftSlots[i].SetSlotActive(true);
                
                LeftSlots[i].SetDefaultSprite();
                
                
                int unitID = _house.GetLeftSideUnitIDs[i];
                if (unitID >= 0 ) LeftSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
            
            for (int i = 0; i < _house.GetRightSideUnitIDs.Length; i++)
            {
                if(i >= _house.GetMaxRightUnitAmount) RightSlots[i].SetSlotActive(false);
                else RightSlots[i].SetSlotActive(true);
                
                RightSlots[i].SetDefaultSprite();
                
                int unitID = _house.GetRightSideUnitIDs[i];
                if (unitID >= 0 ) RightSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
        }

        
        
    }
}