// File     : HUD.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;
using UnityEngine.EventSystems;


public class UIPointerInHudManager : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
     private static bool IsInHud;
     public static bool GetIsInHut => IsInHud;

     public static void SetPointerInHud(bool _value)
     {
         IsInHud = _value;
     }
     
     public void OnPointerEnter(PointerEventData eventData)
     {
         IsInHud = true;
     }

     public void OnPointerExit(PointerEventData eventData)
     {
         IsInHud = false;
     }
}
