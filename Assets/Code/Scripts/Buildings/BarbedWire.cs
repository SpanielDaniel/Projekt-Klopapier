using UnityEngine;

namespace Buildings
{
    public class BarbedWire : Building
    {
        [SerializeField] private GameObject Wire;

        protected override void OnBuildEffect()
        {
            Wire.SetActive(true);
            base.OnBuildEffect();
        }
    }
}