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
        // -------------------------------------------------------------------------------------------------------------
        #region Init
        // Events ------------------------------------------------------------------------------------------------------
        public static Action<int, int> OnUnitChanged;
        
        // Serialized Fields -------------------------------------------------------------------------------------------
        [SerializeField] private GameObject Icon;
        // -------------------------------------------------------------------------------------------------------------
        private UISlot UISlotFirst;

        #endregion
        // -------------------------------------------------------------------------------------------------------------
        private void Awake()
        {
            UISlot.OnDragStarted += SetFirstUISlot;
            UISlot.OnDropStarted += NewUISlot;
            UISlot.OnEndDragStarted += EndDrag;
            Icon.SetActive(false);

        }
        private void Update()
        {
            Icon.transform.position = Input.mousePosition;
        }
        
        // -------------------------------------------------------------------------------------------------------------


        #region Functions
        
        // Events ------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Deactivates the image.
        /// </summary>
        /// <param name="_slot"></param>
        private void EndDrag()
        {
            Icon.SetActive(false);
        }
        
        /// <summary>
        /// Sets the first UISlot.
        /// </summary>
        /// <param name="_slot"></param>
        private void SetFirstUISlot(UISlot _slot)
        {
            UISlotFirst = _slot;
            Icon.SetActive(true);
            Icon.GetComponent<Image>().sprite = _slot.GetSprite;
        }

        /// <summary>
        /// Sets a unit to the new slot position from the first UI Slot.
        /// </summary>
        /// <param name="_slot">Hands over the new slot for the unit.</param>
        private void NewUISlot(UISlot _slot)
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


        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
    }
}