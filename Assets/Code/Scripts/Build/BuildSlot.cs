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
        [SerializeField] private BuildingStats BuildingStats;
        [SerializeField] private Image BorderImage;
        [SerializeField] private Image IconImage;
        [SerializeField] private Text BuildingName;
        [SerializeField] private Color NormalColor;
        [SerializeField] private Color MouseEnterColor;

        public static event Action<BuildingStats> OnMouseClick;

        private void Start()
        {
            IconImage.sprite = BuildingStats.BuldingTexture;
            BuildingName.text = BuildingStats.Name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnMouseClick?.Invoke(BuildingStats);
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