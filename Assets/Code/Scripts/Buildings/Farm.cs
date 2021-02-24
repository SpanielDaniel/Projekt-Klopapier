using Player;
using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {

        [SerializeField] private float FoodPerDayPerUnit = 0;
        private float FoodPerDay = 0;

        public float GetFoodPerDay => FoodPerDay;
        protected override void OnBuildEffect()
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

        protected override void AddUnitEffect()
        {
            UpdateFoodPerDay();
        }

        protected override void RemoveUnitEffect(int _unitID)
        {
            UpdateFoodPerDay();
        }

        private void UpdateFoodPerDay()
        {
            FoodPerDay = FoodPerDayPerUnit * GetUnitIDs.Count;
            StartOnValueChanged();
        }
    }
}