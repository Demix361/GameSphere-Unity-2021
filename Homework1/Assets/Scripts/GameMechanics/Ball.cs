using System.Collections;
using Unity.VisualScripting;
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

        [SerializeField] private SpriteRenderer bonus;
        
        private GameController gameController;
        public string _type;
        private string _gameType;

        public void SetAmogus(float time, string type, string gameType)
        {
            _gameType = gameType;
            popTime = time;
            _type = type;
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
            
            if (_type == "Default" && _gameType == "Classic")
            {
                gameController.MissBall();
            }

            Destroy(gameObject);
        }
    }
}
