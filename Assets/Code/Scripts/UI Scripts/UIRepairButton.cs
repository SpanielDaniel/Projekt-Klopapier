
using UI_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.UI_Scripts
{
    public class UIRepairButton : MonoBehaviour
        ,IPointerEnterHandler
        ,IPointerExitHandler
    {
        [SerializeField] private UIBuildingManager UIBuildingManager;

        
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIBuildingManager.ShowRepairResource();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIBuildingManager.HideResReduce();
        }
    }
}