using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using  DG.Tweening;

namespace GameMechanics
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D defaultCollider;
        [SerializeField] private PolygonCollider2D imposterCollider;
        [SerializeField] public SpriteRenderer bonus;
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private AmogusInfo[] _amogusInfos;
        [SerializeField] public SpriteRenderer impostorOutline;
        [SerializeField] private GameObject _popSound;
        [SerializeField] private AudioSource _imposterSound;
        
        public enum AmogusType
        {
            Default,
            Bonus,
            Impostor
        }
        
        [NonSerialized] public AmogusType Type;
        [NonSerialized] public AmogusInfo Info;
        private GameController _gameController;
        private float _scaleSpeed;
        private Sequence _impostorFlashTween;
        private bool _destroyed;

        public void SetAmogus(float scaleSpeed, AmogusType type, GameController gameController)
        {
            _scaleSpeed = scaleSpeed;
            Type = type;
            _gameController = gameController;
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];

            if (Type == AmogusType.Impostor)
            {
                GetComponent<SpriteRenderer>().sprite = Info.impostorSprite;
                transform.localScale = new Vector3(transform.localScale.x * 3.7f, transform.localScale.y * 3.7f, transform.localScale.z);
                defaultCollider.enabled = false;
                imposterCollider.enabled = true;
                impostorOutline.gameObject.SetActive(true);
                
                _imposterSound.Play();
                _imposterSound.DOFade(0, 3f);//.SetEase(Ease.InQuint);
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

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_destroyed && other.CompareTag("DeleteZone"))
            {
                if (Type == AmogusType.Default)
                {
                    _gameController.MissBall();
                }

                SafeDestroy();
            }
        }

        private IEnumerator LifeCycle()
        {
            var scale = transform.localScale;
            var deltaScale = new Vector3(scale.x * _scaleSpeed, scale.y * _scaleSpeed, 0);

            if (Type == AmogusType.Impostor)
            {
                _impostorFlashTween = DOTween.Sequence().Append(impostorOutline.DOColor(Color.red, 10f).SetEase(Ease.Flash, 20, 0));
            }
            
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
            _impostorFlashTween.Kill();
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}
