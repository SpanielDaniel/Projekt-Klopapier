// File     : PlayerController.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UI_Scripts;
using UnityEngine;
using Build;
using Interfaces;
using NUnit.Framework.Internal;
using TestScripts;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        
        [SerializeField] private BuildManager BuildManager;

        private Vector3 MousePos;
        private bool IsPointerEntered;
        Transform LastHit;
        Transform CurrentHit;
        
        
        private void Update()
        {
            MousePos = Input.mousePosition;
            
            if (UIPointerHandler.GetIsInHut) return;

            if (BuildManager.GetIsBuilding)
            {
                BuildManager.BuildBuilding();
                return;
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
                    IMouseLeftClick mLeftclick = CurrentHit.GetComponent<IMouseLeftClick>();
                    if(mLeftclick != null) mLeftclick.OnMouseLeftClickAction(); 
                }
                
                LastHit = CurrentHit;
            }
        }
    }
}