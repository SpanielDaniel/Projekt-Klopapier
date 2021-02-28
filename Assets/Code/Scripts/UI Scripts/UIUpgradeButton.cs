// File     : UIUpgradeButton.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using Interfaces;
using UI_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.UI_Scripts
{
    public class UIUpgradeButton : MonoBehaviour
        ,IPointerEnterHandler
        ,IPointerExitHandler
    {
        [SerializeField] private UIBuildingManager UIBuildingManager;

        
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIBuildingManager.ShowUpgradeReduce();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIBuildingManager.HideResReduce();
        }
    }
}