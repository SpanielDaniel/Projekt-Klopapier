using System.Linq;
using Player;
using TMPro;
using UnityEngine;

namespace Buildings
{
    public class House : Building
    {

        [SerializeField] private int PopulationCapacity;
        
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

            Debug.Log("Add");
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
                }
            }
            
            for (int i = 0; i < BackSideUnitIDs.Length; i++)
            {
                if (BackSideUnitIDs[i] == _unitID)
                {
                    BackSideUnitIDs[i] = -1;
                }
            }
            
            for (int i = 0; i < LeftSideUnitIDs.Length; i++)
            {
                if (LeftSideUnitIDs[i] == _unitID)
                {
                    LeftSideUnitIDs[i] = -1;
                }
            }
            
            for (int i = 0; i < RightSideUnitIDs.Length; i++)
            {
                if (RightSideUnitIDs[i] == _unitID)
                {
                    RightSideUnitIDs[i] = -1;
                }
            }
            
            StartOnValueChanged();
        }
    }
}