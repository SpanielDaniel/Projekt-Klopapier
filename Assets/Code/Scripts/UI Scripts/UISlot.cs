// File     : Slot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts;
using Interfaces;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI_Scripts
{
    public class UISlot : MonoBehaviour 
        , IPointerEnterHandler
        , IPointerExitHandler
    {
        #region Init

        public static Action<Unit> OnUnitRelease;

        [SerializeField] private Image CurrentImage;
        [SerializeField] private Sprite DefaultSprite;
        [SerializeField] private GameObject Button;
        [SerializeField] private GameObject Cross;

        private int UnitID;

        private void Awake()
        {
            if(Button != null) Button.SetActive(false);
            UnitID = -1;
        }

        private void Start()
        {
            StartEffect();
        }

        protected virtual void StartEffect()
        {
        
        }

        #endregion
        
        #region Function

        public void SetSlotActive(bool _isActive )
        {
            Cross.SetActive(!_isActive);
        }

        public void RemoveUnit()
        {
            SetUnitId(-1);
            //Button.SetActive(false);
        }
        
        public void Init(Sprite _sprite, int _unitID)
        {
            SetImage(_sprite);
            SetUnitId(_unitID);
        }
        protected void SetImage(Sprite _sprite)
        {
            CurrentImage.gameObject.SetActive(true);
            CurrentImage.sprite = _sprite;
            
        }

        private void SetUnitId(int _id)
        {
            UnitID = _id;
            if(UnitID >= 0 && IsMouseEntered) Button.SetActive(true); 
        }

        public void SetDefaultSprite()
        {
            CurrentImage.sprite = DefaultSprite;
            CurrentImage.gameObject.SetActive(false);
        }

        #endregion
        
        #region Events

        private bool IsMouseEntered;

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsMouseEntered = true;
            if (Button != null && UnitID >= 0) Button.SetActive(true);
        }
        
        

        public void OnPointerExit(PointerEventData eventData)
        {
            IsMouseEntered = false;
            if(Button != null) Button.SetActive(false);
        }

        public virtual void ButtonAction()
        {
            if (UnitID < 0) return;
            Button.SetActive(false);
            Unit unit = Unit.Units[UnitID];
            OnUnitRelease?.Invoke(unit);
        }

        #endregion

    }
}