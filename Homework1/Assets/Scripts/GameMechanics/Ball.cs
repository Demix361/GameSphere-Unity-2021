using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D defaultCollider;
        [SerializeField] private PolygonCollider2D imposterCollider;
        [SerializeField] public SpriteRenderer bonus;
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private AmogusInfo[] _amogusInfos;

        public enum AmogusType
        {
            Default,
            Bonus,
            Imposter
        }
        
        private GameController _gameController;
        public AmogusType _type;
        private float _popTime;
        private float _maxScale;
        public AmogusInfo Info;

        public void SetAmogus(float time, float maxScale, AmogusType type)
        {
            _popTime = time;
            _type = type;
            _maxScale = maxScale;
            _gameController = FindObjectOfType<GameController>();
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];
            
            if (_type == AmogusType.Imposter)
            {
                GetComponent<SpriteRenderer>().sprite = Info.imposterSprite;
                transform.localScale = new Vector3(transform.localScale.x * 3.7f, transform.localScale.y * 3.7f, transform.localScale.z);
                defaultCollider.enabled = false;
                imposterCollider.enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = Info.crewmateSprite;
                if (_type == AmogusType.Bonus)
                {
                    bonus.gameObject.SetActive(true);
                }
            }

            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);

                var bls = bonus.transform.localScale;
                bonus.transform.localScale = new Vector3(-bls.x, bls.y, bls.z);
            }
            
            StartCoroutine(LifeCycle());
        }

        private IEnumerator LifeCycle()
        {
            var counter = 0f;
            var scale = transform.localScale;
            var deltaScale = new Vector3(_maxScale * scale.x, _maxScale * scale.y, scale.z) - scale;
        
            while (counter < _popTime)
            {
                counter += Time.deltaTime;
                
                transform.localScale += deltaScale * Time.deltaTime / _popTime;

                yield return null;
            }
            
            if (_type == AmogusType.Default)
            {
                _gameController.MissBall();
            }
            
            Destroy(gameObject);
        }

        public void Clicked()
        {
            Destroy(gameObject);

            var particleSystem = Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            var psMain = particleSystem.main;
            psMain.startColor = Info.color;
        }
    }
}
