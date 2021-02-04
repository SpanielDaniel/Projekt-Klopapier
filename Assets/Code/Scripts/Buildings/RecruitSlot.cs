using Assets.Code.Scripts.Unit_Scripts;
using UI_Scripts;
using UnityEngine;

namespace Buildings
{
    public class RecruitSlot  : UISlot
    {
        [SerializeField] private GameObject PrefUnit;
        [SerializeField] private Vector2 SpawnPos;

        public void SetSpawnPos(Vector2 _spawnPos)
        {
            SpawnPos = _spawnPos;
        }
        protected override void StartEffect()
        {
            SetImage(PrefUnit.GetComponent<Unit>().GetUnitData.Icon);
            base.StartEffect();
        }

        public override void ButtonAction()
        {
            SpawnUnitOnPos(PrefUnit,SpawnPos);
            base.ButtonAction();
        }

        private static void SpawnUnitOnPos(GameObject _unitPref, Vector2 _pos)
        {
            GameObject unitObj = Instantiate(_unitPref);
            Unit unit = unitObj.GetComponent<Unit>();
            unit.SetPos((int)_pos.x, (int)_pos.y);
            unit.UpdatePos();
        }
    }
}