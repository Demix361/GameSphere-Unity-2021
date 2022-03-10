using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Star3 : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _starSpriteRenderer;
        private float _deltaSpeed;
        private float _period;
        private float _minBright;
        private float _maxBright;
        private bool _rising = true;
        private float _color;

        private void Start()
        {
            _color = Random.Range(0, 1f);
            transform.localScale *= Random.Range(0.15f, 0.8f);

            _period = Random.Range(0.5f, 4f);
            _minBright = Random.Range(0f, 0.4f);
            _maxBright = 1f;
            _deltaSpeed = (_maxBright - _minBright) / _period;
            
            _starSpriteRenderer.color = new Color(1f, _color, 0f, _minBright);
        }
        
        
        private void Update()
        {
            if (_rising)
            {
                _starSpriteRenderer.color = new Color(1f, _color, 0f, 
                    _starSpriteRenderer.color.a + _deltaSpeed * Time.deltaTime);
            }
            else
            {
                _starSpriteRenderer.color = new Color(1f, _color, 0f,
                    _starSpriteRenderer.color.a - _deltaSpeed * Time.deltaTime);
            }

            if (_starSpriteRenderer.color.a >= _maxBright || _starSpriteRenderer.color.a <= _minBright)
            {
                _rising = !_rising;
            }
        }
    }
}