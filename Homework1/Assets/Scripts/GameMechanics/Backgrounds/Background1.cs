using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Background1 : MonoBehaviour
    {
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private int starAmount;
        private float height;
        private float width;
        
        private void Start()
        {
            height = Camera.main.orthographicSize;
            width = height * Camera.main.aspect;

            for (int i = 0; i < starAmount; i++)
            {
                var pos = new Vector2(Random.Range(-width, width), Random.Range(-height, height));
                Instantiate(starPrefab, pos, Quaternion.identity, transform);
            }

            StartCoroutine(SpawnStars());
        }

        private IEnumerator SpawnStars()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

                var pos = new Vector2(width * 1.1f, Random.Range(-height, height));
                Instantiate(starPrefab, pos, Quaternion.identity, transform);
            }
        }
    }
}
