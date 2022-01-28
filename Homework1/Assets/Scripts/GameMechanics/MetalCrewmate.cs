using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class MetalCrewmate : MonoBehaviour, IAmogus
    {
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private GameObject _metalParticleSystem;
        [SerializeField] private AmogusInfo[] _amogusInfos;
        [SerializeField] private GameObject _popSound;
        [SerializeField] private SpriteRenderer _armorSpriteRenderer;
        [SerializeField] private Sprite _halfArmor;
        
        private GameController _gameController;
        private bool _destroyed;
        private int _clickCount = 0;

        public IAmogus.AmogusType Type { get; } = IAmogus.AmogusType.Metal;
        public AmogusInfo Info { get; private set; }

        public void SetAmogus(int minSortingOrder, GameController gameController)
        {
            _gameController = gameController;
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];
            
            GetComponent<SpriteRenderer>().sprite = Info.crewmateSprite;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder;
            _armorSpriteRenderer.sortingOrder = minSortingOrder + 1;
            
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
            if (_clickCount == 0)
            {
                Instantiate(_metalParticleSystem, transform.position, Quaternion.identity);
                _armorSpriteRenderer.sprite = _halfArmor;
                Instantiate(_popSound);
            }
            else if (_clickCount == 1)
            {
                Instantiate(_metalParticleSystem, transform.position, Quaternion.identity);
                _armorSpriteRenderer.enabled = false;
                Instantiate(_popSound);
            }
            else if (_clickCount == 2)
            {
                var particleSystem = Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                var ps = particleSystem.textureSheetAnimation;
                ps.SetSprite(0, Info.miniSprite);
                
                Instantiate(_popSound);
                            
                SafeDestroy();
                return true;
            }
            
            _clickCount += 1;
            return false;
        }

        public void SafeDestroy()
        {
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}