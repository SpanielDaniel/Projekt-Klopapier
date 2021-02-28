using Player;
using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {

        [SerializeField] private float FoodPerDayPerUnit = 0;
        private float FoodPerDay = 0;
        private float FoodPerMinute = 0;
        private float FoodCount = 0;

        public float GetFoodPerDay => FoodPerDay;

        
        
        protected override void UpdateAction()
        {
            FoodCount += FoodPerMinute * Timer.GetInstance.GetTimeSpeed * Time.deltaTime ;
        }

        protected override void OnBuildEffect()
        {
            Timer.OnDayChanged += AddFoodToPlayer;
            base.OnBuildEffect();
        }

        private void AddFoodToPlayer(int obj)
        {
            PlayerData.GetInstance.IncreaseFood((int)FoodCount);
            FoodCount = 0;
        }

        public override void DestroyEffect()
        {
            base.DestroyEffect();
        }

        public override void Upgrade()
        {
            FoodPerDay *= 2;
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
            FoodPerDay = FoodPerDayPerUnit * UnitAmount;
            FoodPerMinute = FoodPerDay / (24 * 60);
            StartOnValueChanged();
        }
    }
}