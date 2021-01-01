// File     : HudBase.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier



using System;
using UnityEngine;

namespace UI_Scripts
{
    public class UIVisibilityEvent : MonoBehaviour
    {
        public static event Action<UIVisibilityEvent,bool> OnHudVisibleChanged;

        private bool IsHudOpen = false;
        protected bool IsHudOpenH
        {
            set
            {
                IsHudOpen = value;
                OnHudVisibleChanged?.Invoke(this,IsHudOpen);
            }
            get => IsHudOpen;
        }
        
        /// <summary>
        /// Deactivates specific UI elements.
        /// </summary>
        /// <param name="_isActive"></param>
        public virtual void SetUIActive (bool _isActive){}
    }
}