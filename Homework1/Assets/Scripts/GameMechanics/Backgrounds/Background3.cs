using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Background3 : MonoBehaviour
    {
        [SerializeField] private GameObject starPrefab;
        private int starAmount = 200;
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
        }
    }
}