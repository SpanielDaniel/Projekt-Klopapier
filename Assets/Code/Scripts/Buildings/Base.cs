using Assets.Code.Scripts.Unit_Scripts;
using Player;
using UnityEngine;

namespace Buildings
{
    public class Base : Building
    {
        protected override void OnBuildEffect()
        {
           
            UnitCanEnter = false;
            
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