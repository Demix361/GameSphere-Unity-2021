using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Background5 : MonoBehaviour
    {
        [SerializeField] private GameObject starPrefab;
        [SerializeField] private Transform planetTransform;
        private int starAmount = 200;
        private float height;
        private float width;
        
        private void Start()
        {
            height = Camera.main.orthographicSize;
            width = height * Camera.main.aspect;

            for (int i = 0; i < starAmount; i++)
            {
                var pos = new Vector2(Random.Range(-width, width), Random.Range(-width, width));
                Instantiate(starPrefab, pos, Quaternion.identity, transform);
            }
            
            StartCoroutine(SpawnStars());
        }

        private void Update()
        {
            planetTransform.Rotate(0f, 0f, 1f * Time.deltaTime);
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