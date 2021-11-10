using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace GameMechanics
{
    public class Impostor : MonoBehaviour, IAmogus
    {
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private AmogusInfo[] _amogusInfos;
        [SerializeField] private GameObject _popSound;
        [SerializeField] private SpriteRenderer _outline;
        [SerializeField] private GameObject _imposterSound;
        
        private GameController _gameController;
        private float _scaleSpeed;
        private bool _destroyed;
        private Sequence _outlineFlashTween;
        private RandomClipPlayer _appearClipPlayer;

        public IAmogus.AmogusType Type { get; } = IAmogus.AmogusType.Impostor;
        public AmogusInfo Info { get; private set; }

        public void SetAmogus(float scaleSpeed, int minSortingOrder, GameController gameController)
        {
            _scaleSpeed = scaleSpeed;
            _gameController = gameController;
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];

            _appearClipPlayer = Instantiate(_imposterSound).GetComponent<RandomClipPlayer>();

            GetComponent<SpriteRenderer>().sprite = Info.impostorSprite;
            _outline.sortingOrder = minSortingOrder;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder + 1;
            
            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }
            
            StartCoroutine(LifeCycle());
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_destroyed && other.CompareTag("DeleteZone"))
            {
                SafeDestroy();
            }
        }
        
        private IEnumerator LifeCycle()
        {
            var scale = transform.localScale;
            var deltaScale = new Vector3(scale.x * _scaleSpeed, scale.y * _scaleSpeed, 0);
            
            _outlineFlashTween = DOTween.Sequence().Append(_outline.DOColor(Color.red, 10f).SetEase(Ease.Flash, 20, 0));
            
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
            ps.SetSprite(0, Info.miniBoneSprite);
            
            _appearClipPlayer.Stop();
            Instantiate(_popSound);
            
            SafeDestroy();
        }

        public void SafeDestroy()
        {
            _outlineFlashTween.Kill();
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}