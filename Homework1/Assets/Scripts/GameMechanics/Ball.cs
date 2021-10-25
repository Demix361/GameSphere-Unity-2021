using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float popTime;
        [SerializeField] public float maxScale;
        [SerializeField] private PolygonCollider2D defaultCollider;
        [SerializeField] private PolygonCollider2D imposterCollider;

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Sprite[] imposterSprites;
        
        private GameController gameController;
        public bool imposter;
        private string _gameType;

        public void SetAmogus(float time, bool isImposter, string gameType)
        {
            _gameType = gameType;
            popTime = time;
            imposter = isImposter;
            gameController = FindObjectOfType<GameController>();

            if (imposter)
            {
                GetComponent<SpriteRenderer>().sprite = imposterSprites[Random.Range(0, imposterSprites.Length)];
                defaultCollider.enabled = false;
                imposterCollider.enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            }

            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }
            
            StartCoroutine(LifeCycle());
        }

        private IEnumerator LifeCycle()
        {
            var counter = 0f;
            var scale = transform.localScale;
            var deltaScale = new Vector3(maxScale * scale.x, maxScale * scale.y, 1) - scale;
        
            while (counter < popTime)
            {
                counter += Time.deltaTime;
                
                transform.localScale += deltaScale * Time.deltaTime / popTime;

                yield return null;
            }
            
            if (!imposter && _gameType == "Classic")
            {
                gameController.MissBall();
            }

            Destroy(gameObject);
        }
    }
}
