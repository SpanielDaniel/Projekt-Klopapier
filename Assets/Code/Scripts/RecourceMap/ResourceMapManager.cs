// File     : ResourceMapmanager.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using System;
using System.Collections.Generic;
using Assets.Code.Scripts.UI_Scripts;
using Player;
using UnityEngine;

namespace Code.Scripts
{
    public class ResourceMapManager : MonoBehaviour
    {
        public static event Action OnButtonClose;
        [SerializeField] private GameObject PathLinePrefab;
        [SerializeField] private GameObject HudResMap;
        [SerializeField] private GameObject Hud2;
        [SerializeField] private GameObject PrefUnit;
        [SerializeField] private GameObject SVConten;
        
        private List<UnitGather> UnitsInContent = new List<UnitGather>();

        
        private void Awake()
        {
            GameManager.OnResCamActive += OnGather;
            GameManager.OnMapCamActive += OnMap;
            UIUnitManager.OnUnitgather += AddUnit;
            MapResource.OnClickRes += UnitGoGather;
            UIUnitGather.IsBack += StopGather;
            Unit.CancledGather += RemoveUnit;
            Unit.OnMapEntrance += StartGather;
        }

        private void StartGather(Unit _unit,Ground _ground)
        {
            foreach (UnitGather unitGather in UnitsInContent)
            {
                if (unitGather.Unit == _unit)
                {
                    unitGather.SetGround(_ground);
                    unitGather.UnitUi.GetComponent<UIUnitGather>().SetText("Einheit sucht Rohstoffe");
                    unitGather.StartGather();
                }
            }
        }

        private void StopGather(Unit _unit)
        {
            foreach (UnitGather unitGather in UnitsInContent)
            {
                if (unitGather.Unit == _unit)
                {
                    PlayerData.GetInstance.IncreaseWood(unitGather.GetWoodAmount);
                    PlayerData.GetInstance.IncreaseStone(unitGather.GetStoneAmount);
                    PlayerData.GetInstance.IncreaseSteel(unitGather.GetSteelAmount);
                    PlayerData.GetInstance.IncreaseToiletPaper(unitGather.GetToilettePaperAmount);
                    PlayerData.GetInstance.IncreaseFood(unitGather.GetFoodAmount);
                    
                    unitGather.UnitUi.GetComponent<UIUnitGather>().SetText("Einheit stoppt sucht Rohstoffe");
                    _unit.gameObject.SetActive(true);
                    
                    _unit.SetPos(unitGather._comeFromGround.GetWidth,unitGather._comeFromGround.GetHeight);
                    _unit.UpdatePos();
                    
                    Destroy(unitGather.UnitUi);
                    break;
                }
            }
        }

        private void UnitGoGather(MapResource _res)
        {
            foreach (UnitGather unitGather in UnitsInContent)
            {
                unitGather.Resource = _res;
                
                if (unitGather.IsGather == false)
                {
                    
                    unitGather.Unit.GoToMapEnd();
                    unitGather.IsGather = true;
                    unitGather.UnitUi.GetComponent<UIUnitGather>().SetText("Einheit auf dem weg!");
                }
            }
        }

        private void AddUnit(Unit obj)
        {
            GameObject unit = Instantiate(PrefUnit);
            unit.transform.SetParent(SVConten.transform);
            UnitsInContent.Add(new UnitGather(unit,obj,false));
            unit.transform.SetSiblingIndex(0);
        }

        private void RemoveUnit(Unit _unit)
        {
           
            foreach (UnitGather unitGater in UnitsInContent)
            {
                if (unitGater.Unit == _unit)
                {
                    UnitsInContent.Remove(unitGater);
                    Destroy(unitGater.UnitUi);
                    break;
                }
            }

        }

        private void OnMap()
        {
            Hud2.transform.position += Vector3.right * 350;
        }

        private void OnGather()
        {
            Hud2.transform.position += Vector3.left * 350;
        }

        public void OnButtonClick_Close()
        {
            List<UnitGather> uG = new List<UnitGather>();
            foreach (UnitGather unitGather in UnitsInContent)
            {
                if (unitGather.IsGather == false)
                {
                    uG.Add(unitGather);
                }
            }

            foreach (UnitGather ug in uG)
            {
                UnitsInContent.Remove(ug);
                Destroy(ug.UnitUi);
            }
            OnButtonClose?.Invoke();
            
        }
    }
}