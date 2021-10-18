using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class StarsBackground : MonoBehaviour
    {
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private int starAmount;
        
        private void Start()
        {
            var height = Camera.main.orthographicSize;
            var width = height * Screen.width / Screen.height;

            for (int i = 0; i < starAmount; i++)
            {
                var pos = new Vector2(Random.Range(-width, width), Random.Range(-height, height));
                var star = Instantiate(starPrefab, pos, Quaternion.identity);
                star.transform.localScale *= Random.Range(0.2f, 1f);
            }
        }
    }
}
