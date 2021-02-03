using Player;
using UnityEngine;

namespace Buildings
{
    public class Storage : Building
    {
        [SerializeField] private int[] StorageCapacityAdded;
        public override void OnBuildEffect()
        {
            Debug.Log("Effect");
            PlayerData.GetInstance.StorageCapacityH += StorageCapacityAdded[0];
            UnitCanEnter = false;
            base.OnBuildEffect();
        }

        public override void DestroyEffect()
        {
            PlayerData.GetInstance.StorageCapacityH -= StorageCapacityAdded[CurrentLevel];
            base.DestroyEffect();
        }

        public override void Upgrade()
        {
            base.Upgrade();
            PlayerData.GetInstance.StorageCapacityH += StorageCapacityAdded[CurrentLevel] - StorageCapacityAdded[CurrentLevel - 1];
        }
    }
}