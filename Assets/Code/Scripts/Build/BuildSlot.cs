// File     : BuildSlot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Build
{
    public class BuildSlot : MonoBehaviour
        , IPointerClickHandler
        , IPointerEnterHandler
        , IPointerExitHandler
    {
        [SerializeField] private BuildingData buildingData;
        [SerializeField] private Image BorderImage;
        [SerializeField] private Image IconImage;
        [SerializeField] private Text BuildingName;
        [SerializeField] private Color NormalColor;
        [SerializeField] private Color MouseEnterColor;

        public static event Action<BuildingData> OnMouseClick;

        private void Start()
        {
            IconImage.sprite = buildingData.BuldingTexture;
            BuildingName.text = buildingData.Name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnMouseClick?.Invoke(buildingData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            BorderImage.color = MouseEnterColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            BorderImage.color = NormalColor;
        }
    }
}