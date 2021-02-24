// File     : Slot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        private int UnitID;

        private void Awake()
        {
            if(Button != null) Button.SetActive(false);
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


        public void Init(Sprite _sprite, int _unitID)
        {
            SetImage(_sprite);
            SetUnitId(_unitID);
        }
        protected void SetImage(Sprite _sprite)
        {
            CurrentImage.sprite = _sprite;
        }

        private void SetUnitId(int _id)
        {
            UnitID = _id;
        }

        public void SetDefaultSprite()
        {
            CurrentImage.sprite = DefaultSprite;
        }

        #endregion
        
        #region Events

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(Button != null) Button.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(Button != null) Button.SetActive(false);
        }

        public virtual void ButtonAction()
        {
            if (UnitID == null) return;
            Unit unit = Unit.Units[UnitID];
            OnUnitRelease?.Invoke(unit);
        }

        #endregion
    }
}