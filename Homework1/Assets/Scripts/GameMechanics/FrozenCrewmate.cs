using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace GameMechanics
{
    public class FrozenCrewmate : MonoBehaviour, IAmogus
    {
        [SerializeField] private GameObject _particleSystemPrefab;
        [SerializeField] private GameObject _popSound;
        [SerializeField] private SpriteRenderer _bodySprite;
        
        private GameController _gameController;
        private bool _destroyed;

        public IAmogus.AmogusType Type { get; } = IAmogus.AmogusType.Frozen;
        public AmogusInfo Info { get; private set; }
        
        public void SetAmogus(int minSortingOrder, GameController gameController)
        {
            _gameController = gameController;
            GetComponent<SpriteRenderer>().sortingOrder = minSortingOrder + 1;
            _bodySprite.sortingOrder = minSortingOrder;
            
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
            Clicked();
        }
        
        public bool Clicked()
        {
            Instantiate(_particleSystemPrefab, transform.position, Quaternion.identity);
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