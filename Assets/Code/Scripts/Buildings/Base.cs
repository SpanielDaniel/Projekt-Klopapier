using System;
using Assets.Code.Scripts.Unit_Scripts;
using Code.Scripts.Events;
using Player;
using UnityEngine;

namespace Buildings
{
    public class Base : Building
    {
        public static event Action OnDestroy;
        public static event Action OnBaseCreated;
        protected override void OnBuildEffect()
        {
            UnitCanEnter = false;
            OnBaseCreated?.Invoke();
            base.OnBuildEffect();
            
        }
        
        public override void DestroyEffect()
        {
            base.DestroyEffect();
            GameManager.GetInstance.Lose = true;
            OnDestroy?.Invoke();
        }

        public override void Upgrade()
        {
            base.Upgrade();
        }
    }
}