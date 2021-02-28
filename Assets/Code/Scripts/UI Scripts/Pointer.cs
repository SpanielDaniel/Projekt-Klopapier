using System;
using UI_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Scripts.UI_Scripts
{
    public class Pointer : MonoBehaviour
    , IDropHandler
    {
        [SerializeField] private GameObject Icon;


        private void Awake()
        {
            UISlot.OnDragStarted += SetIcon;
            Icon.SetActive(false);

        }

        private void Update()
        {
            Icon.transform.position = Input.mousePosition;
        }

        private void SetIcon(Sprite _sprite)
        {
            Icon.SetActive(true);
            Icon.GetComponent<Image>().sprite = _sprite;
        }


        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Drop");
        }
    }
}