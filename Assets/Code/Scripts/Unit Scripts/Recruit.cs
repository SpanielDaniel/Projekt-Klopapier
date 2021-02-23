// File     : Recrute.cs
// Author   : Daniel Bäcker
// Project  : Projekt-Klopapier

using UnityEngine;

namespace Assets.Code.Scripts.Unit_Scripts
{
    public class Recruit : MonoBehaviour
    {
        [SerializeField] private float RecruitTimeInSeconds;
        [SerializeField] private int ToilettePaperCosts;

        public float GetRecruitTimeInSeconds => RecruitTimeInSeconds;
        public int GetToilettePaperCosts => ToilettePaperCosts;
    }
}