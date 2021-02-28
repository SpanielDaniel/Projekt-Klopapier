// File     : Slot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Code.Scripts;
using Code.Scripts.UI_Scripts;
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
        , IPointerClickHandler
        , IPointerExitHandler
        , IDragHandler
        , IDropHandler
        , IEndDragHandler
    {
        #region Init

        public static Action<Unit> OnUnitRelease;
        public static Action<UISlot> OnDragStarted; 
        public static Action OnEndDragStarted; 
        public static Action<UISlot> OnDropStarted; 

        [SerializeField] private Image CurrentImage;
        [SerializeField] private Sprite DefaultSprite;
        [SerializeField] private GameObject RedWoodBackground;
        [SerializeField] private GameObject Cross;
        [SerializeField] private bool CanDrag = false;
        [SerializeField] private bool CanDrop = false;
        [SerializeField] private int SlotId;
        public int GetSlotId => SlotId;
        private int UnitID;

        public int GetID => UnitID;

        public Sprite GetSprite => CurrentImage.sprite;

        private void Awake()
        {
            //if(Button != null) Button.SetActive(false);
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
            CanDrop = _isActive;
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
            CanDrag = true;
            CanDrop = false;
        }
        protected void SetImage(Sprite _sprite)
        {
            CurrentImage.gameObject.SetActive(true);
            CurrentImage.sprite = _sprite;
            
        }

        private void SetUnitId(int _id)
        {
            UnitID = _id;
            //if(UnitID >= 0 && IsMouseEntered) Button.SetActive(true); 
        }

        public void SetDefaultSprite()
        {
            CurrentImage.sprite = DefaultSprite;
            CurrentImage.gameObject.SetActive(false);
            UnitID = -1;
            CanDrag = false;
        }

        #endregion
        
        #region Events
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (RedWoodBackground == null) return;
            RedWoodBackground.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (RedWoodBackground == null) return;
            RedWoodBackground.SetActive(false);
        }

        public virtual void ButtonAction()
        {
            if (UnitID < 0) return;
            //Button.SetActive(false);
            Unit unit = Unit.Units[UnitID];
            UnitID = -1;
            OnUnitRelease?.Invoke(unit);
        }

        #endregion

        public void OnDrag(PointerEventData eventData)
        {
            if(CanDrag) OnDragStarted?.Invoke(this);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragStarted?.Invoke();
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            
            if(CanDrop) OnDropStarted?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ButtonAction();
        }
    }
}