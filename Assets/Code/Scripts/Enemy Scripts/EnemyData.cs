using UnityEngine;

namespace Assets.Code.Scripts.Enemy_Scripts
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class EnemyData : ScriptableObject
    {
        public int MaxHealthPoints;
        public int Defence;
        public int AttackPoints;
        public float AttackSpeed;
        public float MoveSpeed;
        public float Range;
        public int PaperDrop;
    }
}
