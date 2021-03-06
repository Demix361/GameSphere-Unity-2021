using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Crewmate : MonoBehaviour, IAmogus
    {
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private AmogusInfo[] _amogusInfos;
        [SerializeField] private GameObject _popSound;
        
        private GameController _gameController;
        private float _scaleSpeed;
        private bool _destroyed;
        private List<Sprite> _particleSprites = new List<Sprite>();

        public IAmogus.AmogusType Type { get; } = IAmogus.AmogusType.Crewmate;
        public AmogusInfo Info { get; private set; }

        public void SetAmogus(int minSortingOrder, GameController gameController)
        {
            _gameController = gameController;
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];
            
            GetComponent<SpriteRenderer>().sprite = Info.crewmateSprite;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder;
            
            _particleSprites.Add(Info.miniSprite);
            
            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }
        }
        
        public void SetAmogus(int minSortingOrder, GameController gameController, AmogusInfo amogusInfo, Sprite skin, Sprite[] particles)
        {
            _gameController = gameController;
            Info = amogusInfo;
            
            GetComponent<SpriteRenderer>().sprite = skin;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder;

            foreach (var p in particles)
            {
                _particleSprites.Add(p);
            }

            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }
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
        
        public bool Clicked()
        {
            var particleSystem = Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            var ps = particleSystem.textureSheetAnimation;
            
            foreach (var p in _particleSprites)
            {
                ps.AddSprite(p); 
            }

            Instantiate(_popSound);
            
            SafeDestroy();

            return true;
        }

        public void SafeDestroy()
        {
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}