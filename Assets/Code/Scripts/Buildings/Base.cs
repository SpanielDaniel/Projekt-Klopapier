using System;
using Assets.Code.Scripts.Unit_Scripts;
using Player;
using UnityEngine;

namespace Buildings
{
    public class Base : Building
    {

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
        }

        public override void Upgrade()
        {
            base.Upgrade();
        }
    }
}