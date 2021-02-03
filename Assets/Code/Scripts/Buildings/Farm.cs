using Player;
using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        
        public override void OnBuildEffect()
        {
            Debug.Log("Effect");
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