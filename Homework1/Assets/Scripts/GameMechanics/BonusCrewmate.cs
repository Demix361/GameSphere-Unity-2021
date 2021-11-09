using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;


namespace GameMechanics
{
    public class BonusCrewmate : MonoBehaviour, IAmogus
    {
        [SerializeField] private SpriteRenderer bonus;
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private AmogusInfo[] _amogusInfos;
        [SerializeField] private GameObject _popSound;
        
        private GameController _gameController;
        private float _scaleSpeed;
        private bool _destroyed;
        private Sequence _bonusFlashTween;

        public IAmogus.AmogusType Type { get; } = IAmogus.AmogusType.Bonus;
        public AmogusInfo Info { get; private set; }

        public void SetAmogus(float scaleSpeed, int minSortingOrder, GameController gameController)
        {
            _scaleSpeed = scaleSpeed;
            _gameController = gameController;
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];
            
            GetComponent<SpriteRenderer>().sprite = Info.crewmateSprite;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder;
            bonus.sortingOrder = minSortingOrder + 1;
            
            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
                var bls = bonus.transform.localScale;
                bonus.transform.localScale = new Vector3(-bls.x, bls.y, bls.z);
            }
            
            StartCoroutine(LifeCycle());
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_destroyed && other.CompareTag("DeleteZone"))
            {
                _gameController.MissBall();
                SafeDestroy();
            }
        }
        
        private IEnumerator LifeCycle()
        {
            var scale = transform.localScale;
            var deltaScale = new Vector3(scale.x * _scaleSpeed, scale.y * _scaleSpeed, 0);

            var endColor = Color.white - Info.color;
            endColor.a = 1;
            _bonusFlashTween = DOTween.Sequence().Append(bonus.DOColor(endColor, 10f).SetEase(Ease.Flash, 20, 0));
            
            while (true)
            {
                transform.localScale += deltaScale * Time.deltaTime;
                yield return null;
            }
        }
        
        public void Clicked()
        {
            var particleSystem = Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            var ps = particleSystem.textureSheetAnimation;
            ps.SetSprite(0, Info.miniSprite);

            Instantiate(_popSound);
            
            SafeDestroy();
        }

        public void SafeDestroy()
        {
            _bonusFlashTween.Kill();
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}