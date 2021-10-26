using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D defaultCollider;
        [SerializeField] private PolygonCollider2D imposterCollider;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Sprite[] imposterSprites;
        [SerializeField] public SpriteRenderer bonus;
        
        private GameController gameController;
        public string _type;
        private string _gameType;
        private float _popTime;
        private float _maxScale;
        
        public void SetAmogus(float time, float maxScale, string type, string gameType)
        {
            _gameType = gameType;
            _popTime = time;
            _type = type;
            _maxScale = maxScale;
            gameController = FindObjectOfType<GameController>();

            if (_type == "Imposter")
            {
                GetComponent<SpriteRenderer>().sprite = imposterSprites[Random.Range(0, imposterSprites.Length)];
                defaultCollider.enabled = false;
                imposterCollider.enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
                if (_type == "Bonus")
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
            
            if (_type == "Default" && _gameType == "Classic")
            {
                gameController.MissBall();
            }

            Destroy(gameObject);
        }
    }
}
