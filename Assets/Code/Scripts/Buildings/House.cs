using System.Linq;
using Player;
using TMPro;
using UnityEngine;

namespace Buildings
{
    public class House : Building
    {

        [SerializeField] private int PopulationCapacity;
        
        [SerializeField] private GameObject[] FrontGuns;
        [SerializeField] private GameObject[] BackGuns;
        [SerializeField] private GameObject[] RightGuns;
        [SerializeField] private GameObject[] LeftGuns;
        
        private int MaxFrontUnitAmount = 4;
        private int MaxBackUnitAmount = 4;
        private int MaxLeftUnitAmount = 1;
        private int MaxRightUnitAmount = 1;
        
        private int[] FrontSideUnitIDs = new int[8];
        private int[] BackSideUnitIDs = new int[8];
        private int[] LeftSideUnitIDs = new int[3];
        private int[] RightSideUnitIDs = new int[3];


        public int GetMaxFrontAmount => MaxFrontUnitAmount;
        public int GetMaxBackUnitAmount => MaxBackUnitAmount;
        public int GetMaxLeftUnitAmount => MaxLeftUnitAmount;
        public int GetMaxRightUnitAmount => MaxRightUnitAmount;
        
        public int[] GetFrontSideUnitIDs => FrontSideUnitIDs;
        public int[] GetBackSideUnitIDs => BackSideUnitIDs;
        public int[] GetLeftSideUnitIDs => LeftSideUnitIDs;
        public int[] GetRightSideUnitIDs => RightSideUnitIDs;

        protected override void OnBuildEffect()
        {
            for (int i = 0; i < FrontSideUnitIDs.Length; i++)
            {
                FrontSideUnitIDs[i] = -1;
            }
            
            for (int i = 0; i < BackSideUnitIDs.Length; i++)
            {
                BackSideUnitIDs[i] = -1;
            }
            
            for (int i = 0; i < LeftSideUnitIDs.Length; i++)
            {
                LeftSideUnitIDs[i] = -1;
            }
            
            for (int i = 0; i < RightSideUnitIDs.Length; i++)
            {
                RightSideUnitIDs[i] = -1;
            }

            PlayerData.GetInstance.PopulationCapacityH += PopulationCapacity;
            base.OnBuildEffect();
        }
        

        public override void Upgrade()
        {
            base.Upgrade();
            MaxFrontUnitAmount += 2;
            MaxBackUnitAmount += 2;
            MaxLeftUnitAmount++;
            MaxRightUnitAmount++;
            
            StartOnValueChanged();
        }

        protected override void AddUnitEffect()
        {
            if(!FrontSideCheck())
                if(!BackSideCheck())
                    if(!RightSideCheck())
                        if (!LeftSideCheck())
                        {
                            
                        }
            StartOnValueChanged();
        }

        private bool FrontSideCheck()
        {
            int counter = 0;

            foreach (var iD in FrontSideUnitIDs)
            {
                if (iD >= 0) counter++;
            }

            if (counter < MaxFrontUnitAmount)
            {
                int a = GetUnitIDs.Count - 1;

                for (int i = 0; i < FrontSideUnitIDs.Length; i++)
                {
                    if (FrontSideUnitIDs[i] < 0)
                    {
                        FrontSideUnitIDs[i] = GetUnitIDs[a];
                        FrontGuns[i].SetActive(true);
                        return true;
                    }
                }
            }

            return false;
        }
        
        private bool BackSideCheck()
        {
            int counter = 0;

            foreach (var iD in BackSideUnitIDs)
            {
                if (iD >= 0) counter++;
            }

            if (counter < MaxBackUnitAmount)
            {
                int a = GetUnitIDs.Count - 1;

                for (int i = 0; i < BackSideUnitIDs.Length; i++)
                {
                    if (BackSideUnitIDs[i] < 0 )
                    {
                        BackSideUnitIDs[i] = GetUnitIDs[a];
                        BackGuns[i].SetActive(true);
                        return true;
                    }
                }
            }

            return false;
        }
        private bool LeftSideCheck()
        {
            int counter = 0;

            foreach (var iD in LeftSideUnitIDs)
            {
                if (iD >= 0) counter++;
            }

            if (counter < MaxLeftUnitAmount)
            {
                int a = GetUnitIDs.Count - 1;

                for (int i = 0; i < LeftSideUnitIDs.Length; i++)
                {
                    if (LeftSideUnitIDs[i] < 0)
                    {
                        LeftSideUnitIDs[i] = GetUnitIDs[a];
                        LeftGuns[i].SetActive(true);
                        return true;
                    }
                }
            }

            return false;
        }
        private bool RightSideCheck()
        {
            int counter = 0;

            foreach (var iD in RightSideUnitIDs)
            {
                if (iD >= 0) counter++;
            }

            if (counter < MaxRightUnitAmount)
            {
                int a = GetUnitIDs.Count - 1;

                for (int i = 0; i < RightSideUnitIDs.Length; i++)
                {
                    if (RightSideUnitIDs[i] < 0)
                    {
                        RightSideUnitIDs[i] = GetUnitIDs[a];
                        RightGuns[i].SetActive(true);
                        return true;
                    }

                }
            }

            return false;
        }

        protected override void RemoveUnitEffect(int _unitID)
        {

            for (int i = 0; i < FrontSideUnitIDs.Length; i++)
            {
                if (FrontSideUnitIDs[i] == _unitID)
                {
                    FrontSideUnitIDs[i] = -1;
                    FrontGuns[i].SetActive(false);
                }
            }
            
            for (int i = 0; i < BackSideUnitIDs.Length; i++)
            {
                if (BackSideUnitIDs[i] == _unitID)
                {
                    BackSideUnitIDs[i] = -1;
                    BackGuns[i].SetActive(false);
                }
            }
            
            for (int i = 0; i < LeftSideUnitIDs.Length; i++)
            {
                if (LeftSideUnitIDs[i] == _unitID)
                {
                    LeftSideUnitIDs[i] = -1;
                    LeftGuns[i].SetActive(false);
                }
            }
            
            for (int i = 0; i < RightSideUnitIDs.Length; i++)
            {
                if (RightSideUnitIDs[i] == _unitID)
                {
                    RightSideUnitIDs[i] = -1;
                    RightGuns[i].SetActive(false);
                }
            }
            
            StartOnValueChanged();
        }

        public void ChangeUnitSlot(int _slot1, int _slot2)
        {
            int unitID = -1;
            if (_slot1 < 8)
            {
                unitID = FrontSideUnitIDs[_slot1];
                FrontGuns[_slot1].SetActive(false);
                FrontSideUnitIDs[_slot1] = -1;
            }
            else if (_slot1 >= 8 && _slot1 < 16)
            {
                unitID = BackSideUnitIDs[_slot1 - 8];
                BackGuns[_slot1 - 8].SetActive(false);
                BackSideUnitIDs[_slot1 - 8] = -1;
            }
            else if (_slot1 >= 16 && _slot1 < 19)
            {
                unitID = LeftSideUnitIDs[_slot1 - 16];
                LeftGuns[_slot1 - 16].SetActive(false);
                LeftSideUnitIDs[_slot1 - 16] = -1;
            }
            else if (_slot1 >= 19 && _slot1 < 22)
            {
                unitID = RightSideUnitIDs[_slot1 - 19];
                RightGuns[_slot1 - 19].SetActive(false);
                RightSideUnitIDs[_slot1 - 19] = -1;
            }
            
            
            if (_slot2 < 8)
            {
                FrontSideUnitIDs[_slot2] = unitID;
                FrontGuns[_slot2].SetActive(true);
            }
            else if (_slot2 >= 8 && _slot2 < 16)
            {
                BackSideUnitIDs[_slot2 - 8] = unitID;
                BackGuns[_slot2 - 8].SetActive(true);
            }
            else if (_slot2 >= 16 && _slot2 < 19)
            {
                LeftSideUnitIDs[_slot2 - 16] = unitID;
                LeftGuns[_slot2 - 16].SetActive(true);

            }
            else if (_slot2 >= 19 && _slot2 < 22)
            {
                RightSideUnitIDs[_slot2 - 19] = unitID;
                RightGuns[_slot2 - 19].SetActive(true);
            }
            
            StartOnValueChanged();
            
            
        }
    }
}