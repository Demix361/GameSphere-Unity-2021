using System;
using System.Collections;
using Unity.VisualScripting;
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
        
        public AmogusType Type;
        public AmogusInfo Info;
        private GameController _gameController;
        private float _scaleSpeed = 0.005f;

        public void SetAmogus(float scaleSpeed, AmogusType type, GameController gameController)
        {
            _scaleSpeed = scaleSpeed;
            Type = type;
            _gameController = gameController;
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];
            
            if (Type == AmogusType.Imposter)
            {
                GetComponent<SpriteRenderer>().sprite = Info.imposterSprite;
                transform.localScale = new Vector3(transform.localScale.x * 3.7f, transform.localScale.y * 3.7f, transform.localScale.z);
                defaultCollider.enabled = false;
                imposterCollider.enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = Info.crewmateSprite;
                if (Type == AmogusType.Bonus)
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("DeleteZone"))
            {
                if (Type == AmogusType.Default)
                {
                    _gameController.MissBall();
                }
                
                Destroy(gameObject);
            }
        }

        private IEnumerator LifeCycle()
        {
            var scale = transform.localScale;
            var deltaScale = new Vector3(scale.x * _scaleSpeed, scale.y * _scaleSpeed, scale.z);
            
            while (true)
            {
                transform.localScale += deltaScale;
                yield return null;
            }
        }

        public void Clicked()
        {
            Destroy(gameObject);

            var particleSystem = Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            var ps = particleSystem.textureSheetAnimation;
            ps.SetSprite(0, Info.miniSprite);
        }
    }
}
