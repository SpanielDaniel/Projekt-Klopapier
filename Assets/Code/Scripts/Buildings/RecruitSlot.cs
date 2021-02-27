using Assets.Code.Scripts.Unit_Scripts;
using Code.Scripts;
using Player;
using TMPro;
using UI_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Buildings
{
    public class RecruitSlot  : MonoBehaviour
    {
        [SerializeField] private GameObject PrefUnit;
        [SerializeField] private Image CurrentImage;
        [SerializeField] private Ground SpawnPos;
        [SerializeField] private TextMeshProUGUI ToiletteAmount;

        public void SetSpawnPos(Ground _spawnPos)
        {
            SpawnPos = _spawnPos;
        }
        protected void Start()
        {
            CurrentImage.sprite = PrefUnit.GetComponent<Unit>().GetUnitData.Icon;
            ToiletteAmount.text = PrefUnit.GetComponent<Recruit>().GetToilettePaperCosts.ToString();
            
        }

        public void ButtonAction()
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