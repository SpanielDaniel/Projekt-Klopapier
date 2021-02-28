using UnityEngine;

namespace Buildings
{
    public class Hospital : Building
    {

        [SerializeField] private float RecoverHealthPerSecond;
        private int MaxDoctors = 3;
        private int MaxPatient = 4;

        private int[] DoctorsUnitIDs = new int[6];
        private int[] PatientsUnitIDs = new int[8];

        private int DoctorsAmount = 0;
        private int PatientsAmount = 0;

        public int GetMaxDoctors => MaxDoctors;
        public int GetMaxPatient => MaxPatient;

        public int[] GetDoctorsUnitIDs => DoctorsUnitIDs;
        public int[] GetPatientsUnitIDs => PatientsUnitIDs;

        private float Counter = 0;
        protected override void UpdateAction()
        {
            
            if (DoctorsAmount > 0 && PatientsAmount > 0)
            {
                
               Unit unit =  Unit.Units[GetFirstPatient()];
               unit.Heal(RecoverHealthPerSecond * DoctorsAmount * Time.deltaTime);
               if (unit.GetCurrentHealth >= unit.GetMaxHealth)
               {
                   RemoveUnit(unit,EntranceGround);
                   StartOnValueChanged();
               }
            }
        }

        protected override void OnBuildEffect()
        {
            for (int i = 0; i < DoctorsUnitIDs.Length; i++)
            {
                DoctorsUnitIDs[i] = -1;
            }

            for (int i = 0; i < PatientsUnitIDs.Length; i++)
            {
                PatientsUnitIDs[i] = -1;
            }
            base.OnBuildEffect();
        }

        public override void Upgrade()
        {
            base.Upgrade();
            if (Level == 1)
            {
                MaxDoctors += 2;
                MaxPatient += 2;
            }
            if (Level == 2)
            {
                MaxDoctors += 1;
                MaxPatient += 2;
            }
            StartOnValueChanged();
        }

        protected override void AddUnitEffect()
        {
            if (!CheckDoctorSide())
                if (!CheckPatientsSide())
                {
                    
                }
            
            StartOnValueChanged();
            

        }

        private bool CheckDoctorSide()
        {
            int counter = 0;

            foreach (var iD in DoctorsUnitIDs)
            {
                if (iD >= 0) counter++;
            }

            if (counter < MaxDoctors)
            {
                int a = GetUnitIDs.Count - 1;

                for (int i = 0; i < DoctorsUnitIDs.Length; i++)
                {
                    if (DoctorsUnitIDs[i] < 0)
                    {
                        DoctorsUnitIDs[i] = GetUnitIDs[a];
                        DoctorsAmount++;
                        return true;
                    }
                }
            }

            return false;
        }
        
        private bool CheckPatientsSide()
        {
            int counter = 0;

            foreach (var iD in PatientsUnitIDs)
            {
                if (iD >= 0) counter++;
            }

            if (counter < MaxPatient)
            {
                int a = GetUnitIDs.Count - 1;

                for (int i = 0; i < PatientsUnitIDs.Length; i++)
                {
                    if (PatientsUnitIDs[i] < 0)
                    {
                        PatientsUnitIDs[i] = GetUnitIDs[a];
                        PatientsAmount++;
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void RemoveUnitEffect(int _unitID)
        {
            for (int i = 0; i < DoctorsUnitIDs.Length; i++)
            {
                if (DoctorsUnitIDs[i] == _unitID)
                {
                    DoctorsUnitIDs[i] = -1;
                    DoctorsAmount--;
                }
            }
            
            for (int i = 0; i < PatientsUnitIDs.Length; i++)
            {
                if (PatientsUnitIDs[i] == _unitID)
                {
                    PatientsUnitIDs[i] = -1;
                    PatientsAmount--;
                }
            }
            StartOnValueChanged();
        }

        private int GetFirstPatient()
        {
            for (int i = 0; i < PatientsUnitIDs.Length; i++)
            {
                if (PatientsUnitIDs[i] >= 0)
                {
                    return PatientsUnitIDs[i];
                }
            }

            return -1;
        }
        
        public void ChangeUnitSlot(int _slot1,int _slot2)
        {
            Debug.Log(_slot1 + " " + _slot2);
            int unitID = -1;
            if (_slot1 < 6)
            {
                unitID = DoctorsUnitIDs[_slot1];
                DoctorsUnitIDs[_slot1] = -1;
                DoctorsAmount--;
            }
            else if (_slot1 >= 6 && _slot1 < 14)
            {
                unitID = PatientsUnitIDs[_slot1 - 6];
                PatientsUnitIDs[_slot1 - 6] = -1;
                PatientsAmount--;
            }
            
            
            
            if (_slot2 < 6)
            {
                DoctorsUnitIDs[_slot2] = unitID;
                DoctorsAmount++;
            }
            else if (_slot2 >= 6 && _slot2 < 14)
            {
                PatientsUnitIDs[_slot2 - 6] = unitID;
                PatientsAmount++;
            }
            
            
            StartOnValueChanged();
        }
    }
}