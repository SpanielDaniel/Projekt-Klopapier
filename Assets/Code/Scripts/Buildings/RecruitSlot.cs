using Assets.Code.Scripts.Unit_Scripts;
using Code.Scripts;
using Player;
using TMPro;
using UI_Scripts;
using UnityEngine;

namespace Buildings
{
    public class RecruitSlot  : UISlot
    {
        [SerializeField] private GameObject PrefUnit;
        [SerializeField] private Ground SpawnPos;
        [SerializeField] private TextMeshProUGUI ToiletteAmount;

        public void SetSpawnPos(Ground _spawnPos)
        {
            SpawnPos = _spawnPos;
        }
        protected override void StartEffect()
        {
            SetImage(PrefUnit.GetComponent<Unit>().GetUnitData.Icon);
            ToiletteAmount.text = PrefUnit.GetComponent<Recruit>().GetToilettePaperCosts.ToString();
            base.StartEffect();
        }

        public override void ButtonAction()
        {
            Recruit recruit = PrefUnit.GetComponent<Recruit>();
            if (PlayerData.GetInstance.IsPlayerHavingEnoughResources(recruit.GetToilettePaperCosts, 0, 0, 0))
            {
                PlayerData.GetInstance.ToiletPaperAmountH -= recruit.GetToilettePaperCosts; 
                SpawnUnitOnPos(PrefUnit,SpawnPos);
                AudioManager.GetInstance.Play("Recruit");
            }
            else
            {
                AudioManager.GetInstance.Play("CantBuild");
            }
        }

        private static void SpawnUnitOnPos(GameObject _unitPref, Ground _pos)
        {
            GameObject unitObj = Instantiate(_unitPref);
            Unit unit = unitObj.GetComponent<Unit>();
            unit.SetPos(_pos.GetWidth, _pos.GetHeight);
            unit.UpdatePos();
        }
    }
}