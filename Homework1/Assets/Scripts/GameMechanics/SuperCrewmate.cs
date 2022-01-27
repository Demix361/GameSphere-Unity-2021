using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class SuperCrewmate : MonoBehaviour, IAmogus
    {
        [FormerlySerializedAs("_particleSystemPrefab")] [SerializeField] private GameObject _popParticleSystemPrefab;
        [SerializeField] private GameObject _destroyParticleSystemPrefab;
        [SerializeField] private AmogusInfo[] _amogusInfos;
        [SerializeField] private GameObject _popSound;
        
        private GameController _gameController;
        private Vector3 _deltaScale;
        private bool _destroyed;
        private int _touchCount = 0;

        public IAmogus.AmogusType Type { get; } = IAmogus.AmogusType.SuperCrewmate;
        public AmogusInfo Info { get; private set; }

        public void SetAmogus(float scaleSpeed, int minSortingOrder, GameController gameController)
        {
            _gameController = gameController;
            Info = _amogusInfos[Random.Range(0, _amogusInfos.Length)];
            
            GetComponent<SpriteRenderer>().sprite = Info.crewmateSprite;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder;
            
            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }

            _deltaScale = transform.localScale * 0.01f;
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
            yield return new WaitForSeconds(4.5f);
            Instantiate(_destroyParticleSystemPrefab, transform.position, Quaternion.identity);
            SafeDestroy();
        }

        public void Clicked()
        {
            
        }
        
        public void Clicked(Vector3 pos)
        {
            _touchCount += 1;
            var psPos = new Vector3(pos.x, pos.y, transform.position.z);
            Instantiate(_popParticleSystemPrefab, psPos, Quaternion.identity);

            if (_touchCount == 1)
            {
                var rb = GetComponent<Rigidbody2D>();
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);
                
                StartCoroutine(LifeCycle());
            }

            transform.position += (transform.position - psPos) * 0.03f;
            transform.localScale += _deltaScale;

            Instantiate(_popSound);
        }

        public void SafeDestroy()
        {
            _destroyed = true;
            Destroy(gameObject);
        }
    }
}
