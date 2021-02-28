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
        // -------------------------------------------------------------------------------------------------------------
        
        #region Init
        // Events ------------------------------------------------------------------------------------------------------
        public static event Action<GameObject> OnMouseClick;

        // Serialize Fields---------------------------------------------------------------------------------------------
        [SerializeField] private GameObject Building;
        [SerializeField] private Image IconImage;
        [SerializeField] private Image Background;
        [SerializeField] private Image BackgroundRed;
        [SerializeField] private TextMeshProUGUI BuildingName;
        [SerializeField] private GameObject Cross;

        #endregion
        
        // -------------------------------------------------------------------------------------------------------------
        private void Awake()
        {
            Cross.SetActive(false);
        }

        private void Start()
        {
            Init();
        }
        
        // -------------------------------------------------------------------------------------------------------------
        
        #region Functions
        
        // Start -------------------------------------------------------------------------------------------------------
        
        /// <summary>
        ///  Initialize the build slot.
        /// </summary>
        private void Init()
        {
            BuildingData data = Building.GetComponent<Building>().GetData;
            IconImage.sprite = data.BuldingTexture;
            BuildingName.text = data.Name;
            BackgroundRed.gameObject.SetActive(false);
            Background.gameObject.SetActive(true);
            Cross.SetActive(false);
        }
        
        
        // Unity Events ------------------------------------------------------------------------------------------------
        // Pointer
        
        public void OnPointerClick(PointerEventData eventData)
        {
            // starts build phase
            OnMouseClick?.Invoke(Building);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // shows red background
            BackgroundRed.gameObject.SetActive(true);
            Background.gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // hide background
            BackgroundRed.gameObject.SetActive(false);
            Background.gameObject.SetActive(true);
        }

        #endregion 
        
        // -------------------------------------------------------------------------------------------------------------
    }
}