using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Star4 : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _starSpriteRenderer;
        private float _speed;
        private float _angle;
        private float _radius;

        private void Start()
        {
            transform.localScale *= Random.Range(0.15f, 0.8f);
            _speed = Random.Range(0.1f, 1f);

            var x = transform.position.x;
            var y = transform.position.y;
            
            _angle = Mathf.Atan(Mathf.Abs(x / y));
            _radius = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));

            if (x >= 0)
            {
                if (y <= 0)
                {
                    _angle = 2 * Mathf.PI - _angle;
                }
            }
            else
            {
                if (y >= 0)
                {
                    _angle = Mathf.PI - _angle;
                }
                else
                {
                    _angle = Mathf.PI + _angle;
                }
            }
        }

        private void Update()
        {
            _angle += Time.deltaTime * _speed;

            var x = Mathf.Sin(_angle) * _radius;
            var y = Mathf.Cos(_angle) * _radius;

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}