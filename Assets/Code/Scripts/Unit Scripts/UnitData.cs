// File     : UnitData.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;

namespace Assets.Code.Scripts.Unit_Scripts
{
    [CreateAssetMenu(fileName = "New Unit",menuName = "Unit")]
    public class UnitData : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public float MaxHealthPoints;
        public float Defence;
        public float AttackPoints;
        public float AttackSpeed;
        public float MoveSpeed;
        public float Range;
    }
}
