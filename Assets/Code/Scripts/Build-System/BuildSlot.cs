// File     : BuildSlot.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Buildings;
using TMPro;
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
        [SerializeField] private Image IconImage;
        [SerializeField] private Image Background;
        [SerializeField] private Image BackgroundRed;
        [SerializeField] private TextMeshProUGUI BuildingName;
        [SerializeField] private GameObject Cross;

        public static event Action<GameObject> OnMouseClick;

        private void Awake()
        {
            Cross.SetActive(false);
        }

        private void Start()
        {
            BackgroundRed.gameObject.SetActive(false);
            Background.gameObject.SetActive(true);
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
            BackgroundRed.gameObject.SetActive(true);
            Background.gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            BackgroundRed.gameObject.SetActive(false);
            Background.gameObject.SetActive(true);
        }
    }
}