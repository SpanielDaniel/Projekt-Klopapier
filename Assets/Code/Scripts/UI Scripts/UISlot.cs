// File     : Slot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
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

        [SerializeField] private Image CurrentImage;
        [SerializeField] private Sprite DefaultSprite;
        [SerializeField] private GameObject Button;

        private void Awake()
        {
            if(Button != null) Button.SetActive(false);
        }
        

        #endregion
        
        #region Function

        private void SetImage(Sprite _sprite)
        {
            CurrentImage.sprite = _sprite;
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

        #endregion
    }
}