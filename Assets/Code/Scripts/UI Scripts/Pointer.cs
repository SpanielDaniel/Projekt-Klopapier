using System;
using Code.Scripts.Buildings.UIElements;
using UI_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.UI_Scripts
{
    public class Pointer : MonoBehaviour
    {
        public static Action<int, int> OnUnitChanged;
        [SerializeField] private GameObject Icon;

        private UISlot UISlotFirst;

        private void Awake()
        {
            UISlot.OnDragStarted += SetUI;
            UISlot.OnDropStarted += NewUI;
            UISlot.OnEndDragStarted += EndDrag;
            Icon.SetActive(false);

        }

        private void EndDrag(UISlot _slot)
        {
            Icon.SetActive(false);

        }

        private void NewUI(UISlot _slot)
        {
            if(_slot == UISlotFirst || _slot == null || UISlotFirst == null) return;
            
            if (_slot.GetID >= 0 )
            {
            }
            else
            {
                _slot.Init(UISlotFirst.GetSprite,UISlotFirst.GetID);
                UISlotFirst.RemoveUnit();
                UISlotFirst.SetDefaultSprite();
                OnUnitChanged?.Invoke(UISlotFirst.GetSlotId,_slot.GetSlotId);
            }
        }

        private void SetUI(UISlot _slot)
        {
            UISlotFirst = _slot;
            Icon.SetActive(true);
            Icon.GetComponent<Image>().sprite = _slot.GetSprite;
        }

        private void Update()
        {
            Icon.transform.position = Input.mousePosition;
        }

        
    }
}