// File     : BuildSlot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Buildings;
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
        [SerializeField] private GameObject Building;
        [SerializeField] private Image BorderImage;
        [SerializeField] private Image IconImage;
        [SerializeField] private Text BuildingName;
        [SerializeField] private Color NormalColor;
        [SerializeField] private Color MouseEnterColor;

        public static event Action<GameObject> OnMouseClick;

        private void Start()
        {
            IconImage.sprite = Building.GetComponent<Building>().GetBuildingData.BuldingTexture;
            BuildingName.text = Building.GetComponent<Building>().GetBuildingData.Name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnMouseClick?.Invoke(Building);
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