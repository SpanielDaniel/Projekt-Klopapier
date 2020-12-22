// File     : HudBase.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using UnityEngine;

namespace UI_Scripts
{
    public class HudBase : MonoBehaviour
    {
        public static event Action<HudBase,bool> OnHudVisableChanged;

        private bool IsHudOpen = false;
        
        protected bool IsHudOpenH
        {
            set
            {
                IsHudOpen = value;
                OnHudVisableChanged?.Invoke(this,IsHudOpen);
            }
            get => IsHudOpen;
        }
        
        public virtual void SetUIActive (bool _isActive){}
    }
}