// File     : HUD.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;
using UnityEngine.EventSystems;


public class UIPointerInHudManager : MonoBehaviour
, IPointerEnterHandler
, IPointerExitHandler
{
    #region Init

    private static bool IsInHud;
    public static bool GetIsInHut => IsInHud;

    #endregion

    #region Functions

    /// <summary>
    /// Sets the 
    /// </summary>
    /// <param name="_value">Hands over the that the pointer isHud.</param>
    public static void SetPointerInHud(bool _value)
    {
        IsInHud = _value;
    }
    // Unity event -----------------------------------------------------------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsInHud = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsInHud = false;
    }
}

#endregion
