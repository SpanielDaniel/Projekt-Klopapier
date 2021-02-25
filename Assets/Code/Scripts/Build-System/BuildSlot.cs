// File     : BuildSlot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Buildings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts
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
        [SerializeField] private GameObject Cross;

        public static event Action<GameObject> OnMouseClick;

        private void Awake()
        {
            Cross.SetActive(false);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            BuildingData data = Building.GetComponent<Building>().GetData;
            IconImage.sprite = data.BuldingTexture;
            BuildingName.text = data.Name;
            Cross.SetActive(false);
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