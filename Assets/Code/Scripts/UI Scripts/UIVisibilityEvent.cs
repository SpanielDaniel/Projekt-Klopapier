// File     : HudBase.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier



using System;
using UnityEngine;

namespace UI_Scripts
{
    public class UIVisibilityEvent : MonoBehaviour
    {
        #region Init

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

        #endregion

        /// <summary>
        /// Deactivates specific UI elements.
        /// </summary>
        /// <param name="_isActive"></param>
        public virtual void SetUIActive(bool _isActive) { }
        
        /// <summary>
        /// Closes the Hud;
        /// </summary>
        protected void CloseHud()
        {
            IsHudOpenH = false;   
        }

        /// <summary>
        /// Opened the Hud
        /// </summary>
        protected void OpenHud()
        {
            IsHudOpenH = true;
        }
        
    }
}