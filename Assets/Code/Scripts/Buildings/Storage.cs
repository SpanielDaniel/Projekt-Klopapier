﻿using Player;
using UnityEngine;

namespace Buildings
{
    public class Storage : Building
    {
        [SerializeField] private int[] StorageCapacityAdded;
        protected override void OnBuildEffect()
        {
            PlayerData.GetInstance.StorageCapacityH += StorageCapacityAdded[0];
            UnitCanEnter = false;
            base.OnBuildEffect();
        }

        public override void DestroyEffect()
        {
            PlayerData.GetInstance.StorageCapacityH -= StorageCapacityAdded[Level];
            base.DestroyEffect();
        }

        public override void Upgrade()
        {
            base.Upgrade();
            PlayerData.GetInstance.StorageCapacityH += StorageCapacityAdded[Level] - StorageCapacityAdded[Level - 1];
        }
    }
}