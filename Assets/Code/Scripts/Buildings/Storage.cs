using Player;
using UnityEngine;

namespace Buildings
{
    public class Storage : Building
    {
        [SerializeField] private int StorageCapacityAdded;
        public override void OnBuildEffect()
        {
            Debug.Log("Effect");
            PlayerData.GetInstance.StorageCapacityH += StorageCapacityAdded;
            //base.OnBuildEffect();
        }

        public override void DestroyEffect()
        {
            PlayerData.GetInstance.StorageCapacityH -= StorageCapacityAdded;
            base.DestroyEffect();
        }
    }
}