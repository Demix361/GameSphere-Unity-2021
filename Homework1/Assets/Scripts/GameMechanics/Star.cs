using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Star : MonoBehaviour
    {
        private Vector3 _deltaSpeed = Vector3.zero;
        private float _border;
        
        private void Start()
        {
            transform.localScale *= Random.Range(0.1f, 0.9f);
            _deltaSpeed.x = 1 - transform.localScale.x;
            _border = - Camera.main.orthographicSize * Camera.main.aspect * 1.1f;
        }

        private void Update()
        {
            transform.position -= _deltaSpeed * Time.deltaTime;

            if (transform.position.x < _border)
            {
                Destroy(gameObject);
            }
        }
    }
}
