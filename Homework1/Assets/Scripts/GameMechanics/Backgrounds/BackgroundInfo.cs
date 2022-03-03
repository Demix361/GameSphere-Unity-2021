using UnityEngine;

namespace GameMechanics
{
    [CreateAssetMenu]
    public class BackgroundInfo : ScriptableObject
    {
        [SerializeField] public int price;
        [SerializeField] public int id;
        [SerializeField] public Sprite preview;
        [SerializeField] public GameObject prefab;
    }
}