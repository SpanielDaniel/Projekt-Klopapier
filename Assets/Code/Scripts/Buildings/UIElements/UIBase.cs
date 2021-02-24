using Code.Scripts;
using UnityEngine;

namespace Buildings
{
    public class UIBase : MonoBehaviour
    {
        
        
        [SerializeField] private RecruitSlot[] Slots;

        public void SetSlotEntrance(Ground _entrancePos)
        {
            foreach (RecruitSlot recruitSlot in Slots)
            {
                recruitSlot.SetSpawnPos(_entrancePos);
            }
        }
        
    }
}