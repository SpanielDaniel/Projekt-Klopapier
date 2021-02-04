using Assets.Code.Scripts.Unit_Scripts;
using Player;
using UnityEngine;

namespace Buildings
{
    public class Base : Building
    {
        public override void OnBuildEffect()
        {
            CreateSlots();
            UnitCanEnter = false;
            Entrance = new Vector2(GetXPos,GetYPOs - 1);
            base.OnBuildEffect();
            
        }

        private void CreateSlots()
        {
            // foreach (UnitData data in Data)
            // {
            //     GameObject slot = Instantiate(Pref_UnitSlot);
            //     slot.transform.SetParent(UISlot.transform);
            // }
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