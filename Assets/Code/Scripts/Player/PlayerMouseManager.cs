// File     : PlayerController.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using Code.Scripts;
using UnityEngine;
using Interfaces;

namespace Player
{
    public class PlayerMouseManager : MonoBehaviour
    {
        
        [SerializeField] private BuildManager BuildManager;
        [SerializeField] private UnitSelector UnitSelector;
        [SerializeField] private UIVisibilityManager UIVisibilityManager;

        private Vector3 MousePos;
        private bool IsPointerEntered;
        Transform LastHit;
        Transform CurrentHit;
        
        private void Update()
        {
            MousePos = Input.mousePosition;
            
            if (UIPointerInHudManager.GetIsInHut) return;

            if(BuildManager != null)
            {
                if (BuildManager.GetIsBuilding)
                {
                    BuildManager.UpdateBuildBuilding();
                    return;
                }
                
            }

            if (UIVisibilityManager != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    UIVisibilityManager.SetAllHudsNonVisable();
                }
            }


            Ray ray = Camera.main.ScreenPointToRay(MousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CurrentHit = hit.transform.transform;
                
                if (CurrentHit != LastHit && LastHit != null)
                {
                    IMouseExit mExit = LastHit.GetComponent<IMouseExit>();
                    if(mExit != null) mExit.OnMouseExitAction();
                }
                
                if (CurrentHit != null)
                {
                    if (CurrentHit != LastHit)
                    {
                        IMouseEnter mEnter = CurrentHit.GetComponent<IMouseEnter>();
                        if (mEnter != null) mEnter.OnMouseEnterAction();
                    }
                    IMouseStay mStay = CurrentHit.GetComponent<IMouseStay>();
                    if (mStay != null) mStay.OnMouseStayAction();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    IMouseLeftDown mLeftclick = CurrentHit.GetComponent<IMouseLeftDown>();
                    
                    if(mLeftclick != null) mLeftclick.OnMouseLeftDownAction();
                }
                
                Vector2 vec = new Vector2(3,3);
                
                if (Input.GetMouseButtonUp(0) &&  UnitSelector.GetSelectionBox.sizeDelta.x < vec.x &&  UnitSelector.GetSelectionBox.sizeDelta.y < vec.y)
                {
                    IMouseLeftUp mLeftclick = CurrentHit.GetComponent<IMouseLeftUp>();
                    
                    if(mLeftclick != null) mLeftclick.OnMouseLeftUpAction();
                }

                
                LastHit = CurrentHit;
            }
        }
    }
}