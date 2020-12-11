using UnityEngine;


    [CreateAssetMenu(fileName = "New Building", menuName = "Building")]
    public class Building : ScriptableObject
    {
        [SerializeField]private string Name;
        [SerializeField]private string Description;
        [SerializeField]private Sprite BuldingTexture;
        [SerializeField]private float MaxLife;
        
        [SerializeField]private int WoodCosts;
        [SerializeField]private int StoneCosts;
        [SerializeField]private int SteelCosts;
    }
