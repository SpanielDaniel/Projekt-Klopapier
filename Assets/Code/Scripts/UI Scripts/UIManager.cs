using System.Collections.Generic;
using UI_Scripts;
using UnityEngine;




public class UIManager : MonoBehaviour
{
    [SerializeField] private List<HudBase> DHuds = new List<HudBase>();
    
    private void Awake()
    {
        HudBase.OnHudVisableChanged += SetHudActive;
    }

    private void SetHudActive(HudBase _hudActive,bool _isActive)
    {
        foreach (var hud in DHuds)
        {
            if(hud == _hudActive) hud.SetUIActive(_isActive);
            else hud.SetUIActive(false);
        }
    }
}

    

    
