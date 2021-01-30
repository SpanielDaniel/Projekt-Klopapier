using System.Collections.Generic;
using UI_Scripts;
using UnityEngine;




public class UIVisibilityManager : MonoBehaviour
{
    [SerializeField] private List<UIVisibilityEvent> UIManagersWithVisibility = new List<UIVisibilityEvent>();
    
    private void Awake()
    {
        UIVisibilityEvent.OnHudVisibleChanged += SetHudActive;
    }
    
    /// <summary>
    /// Deactivates all UI elements in the List and and set the activity of handed over UI to the handed over value.
    /// </summary>
    /// <param name="_uiActive">UI element that changed the visibility.</param>
    /// <param name="_isActive">New visibility value of the UI element</param>
    private void SetHudActive(UIVisibilityEvent _uiActive, bool _isActive)
    {
        foreach (var hud in UIManagersWithVisibility)
        {
            if(hud == _uiActive) hud.SetUIActive(_isActive);
            else hud.SetUIActive(false);
        }
    }

    public void SetAllHusNonVisable()
    {
        foreach (var hud in UIManagersWithVisibility)
        {
            hud.SetUIActive(false);
        }
    }
}

    

    
