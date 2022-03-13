using UnityEngine;

namespace GameMechanics
{
    [CreateAssetMenu]
    public class SkinInfo : ScriptableObject
    {
        [SerializeField] public int price;
        [SerializeField] public int id;
        [SerializeField] public RarityClass rarity;
        [SerializeField] public Sprite skin;
        [SerializeField] public Sprite[] particles;

        public enum RarityClass
        {
            Common,
            Rare,
            Epic,
            Legendary
        }
    }
}