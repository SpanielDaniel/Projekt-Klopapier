// File     : Scrap.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using Player;
using UnityEngine;

namespace Buildings
{
    public class Scrap : Building
    {

        
        [SerializeField] private int AmountOfScrap;
        public int GetAmountOfScrap => AmountOfScrap;
        
        protected override void OnBuildEffect()
        {
            UnitCanEnter = true;
            base.OnBuildEffect();
        }
        

        private float Counter;
        
        public void Update()
        {
            
            Counter += Time.deltaTime;
            if (Counter > 10)
            {
                Counter -= 10;
                AmountOfScrap -= 1 * UnitAmount;
                StartOnValueChanged();
            }
        }
        
    }
}