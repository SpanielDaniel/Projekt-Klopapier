// File     : HUD.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;
using UnityEngine.EventSystems;


public class UIPointerHandler : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
     private static bool IsInHud;
     public static bool GetIsInHut => IsInHud;
     
     public void OnPointerEnter(PointerEventData eventData)
     {
         IsInHud = true;
     }

     public void OnPointerExit(PointerEventData eventData)
     {
         IsInHud = false;
     }
}
