using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace GameMechanics
{
    public class RageCrewmate : MonoBehaviour, IAmogus
    {
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private GameObject _popSound;
        [SerializeField] private SpriteRenderer _bodySprite;
        
        private GameController _gameController;
        private bool _destroyed;

        public IAmogus.AmogusType Type { get; } = IAmogus.AmogusType.Rage;
        public AmogusInfo Info { get; private set; }
        
        public void SetAmogus(float scaleSpeed, int minSortingOrder, GameController gameController)
        {
            _gameController = gameController;
            //Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];
            
            //GetComponent<SpriteRenderer>().sprite = Info.crewmateSprite;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder + 1;
            _bodySprite.sortingOrder = minSortingOrder;
            
            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
                //var bls = bonus.transform.localScale;
                //bonus.transform.localScale = new Vector3(-bls.x, bls.y, bls.z);
            }
            
            //StartCoroutine(LifeCycle());
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_destroyed && other.CompareTag("DeleteZone"))
            {
                _gameController.MissBall();
                SafeDestroy();
            }
        }

        public void Clicked(Vector3 pos)
        {
            
        }
        
        public void Clicked()
        {
            print("CLICKED");
            var particleSystem = Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            //var ps = particleSystem.textureSheetAnimation;
            //ps.SetSprite(0, Info.miniSprite);

            Instantiate(_popSound);
            
            SafeDestroy();
        }

        public void SafeDestroy()
        {
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}