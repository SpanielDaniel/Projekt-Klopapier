using System;
using Buildings;
using UI_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Buildings.UIElements
{
    public class UIHospital : MonoBehaviour
    {
        [SerializeField] private UISlot[] DoctorSlots;
        [SerializeField] private UISlot[] PatientsSlots;

        [SerializeField] private Slider[] Slider;

        private void Awake()
        {
            Unit.OnHeal += UpdateHealthBar;
        }

        private void UpdateHealthBar(int _idUnit, float _healthPoints)
        {
            for (int i = 0; i < PatientsSlots.Length; i++)
            {
                int id = PatientsSlots[i].GetID;
                if (id == _idUnit)
                {
                    Slider[i].maxValue = Unit.Units[id].GetMaxHealth;
                    Slider[i].value = _healthPoints;
                }
            }
        }

        public void UpdateUI(Hospital _hospital)
        {
            for (int i = 0; i < _hospital.GetDoctorsUnitIDs.Length; i++)
            {
                if(i >= _hospital.GetMaxDoctors) DoctorSlots[i].SetSlotActive(false);
                else DoctorSlots[i].SetSlotActive(true);
                
                DoctorSlots[i].SetDefaultSprite();
                

                int unitID = _hospital.GetDoctorsUnitIDs[i];
                if (unitID >= 0 ) DoctorSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
            }
            
            for (int i = 0; i < _hospital.GetPatientsUnitIDs.Length; i++)
            {
                if(i >= _hospital.GetMaxPatient) PatientsSlots[i].SetSlotActive(false);
                else PatientsSlots[i].SetSlotActive(true);
                
                PatientsSlots[i].SetDefaultSprite();

                int unitID = _hospital.GetPatientsUnitIDs[i];
                if (unitID >= 0)
                {
                    Slider[i].gameObject.SetActive(true);
                    PatientsSlots[i].Init(Unit.Units[unitID].GetUnitData.Icon,unitID);
                    UpdateHealthBar(unitID, Unit.Units[unitID].GetCurrentHealth);

                }
                else
                {
                    Slider[i].gameObject.SetActive(false);
                }
            }

            
        }
    }
}