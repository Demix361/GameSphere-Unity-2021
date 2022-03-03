using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Star2 : MonoBehaviour
    {
        private Vector3 _deltaSpeed = Vector3.zero;
        private float _borderX;
        private float _borderY;
        private Vector3 _deltaSize;
        
        private void Start()
        {
            transform.localScale *= Random.Range(0.025f, 0.08f);
            _deltaSpeed = transform.position.normalized * Random.Range(1f, 2f);
            _deltaSize = transform.localScale * Random.Range(1f, 2f);

            _borderY = Camera.main.orthographicSize * 1.1f;
            _borderX = _borderY * Camera.main.aspect;
        }

        private void Update()
        {
            _deltaSpeed += _deltaSpeed * Time.deltaTime * 0.1f;
            transform.position += _deltaSpeed * Time.deltaTime;
            transform.localScale += _deltaSize * Time.deltaTime;

            if (Math.Abs(transform.position.x) > _borderX || Math.Abs(transform.position.y) > _borderY)
            {
                Destroy(gameObject);
            }
        }
    }
}