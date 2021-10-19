using System.Collections;
using UnityEngine;

namespace GameMechanics
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private bool changeScale = true;
        [SerializeField] private bool changeColor;
        [SerializeField] private float popTime;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] public float maxScale;
        [SerializeField] private PolygonCollider2D defaultCollider;
        [SerializeField] private PolygonCollider2D imposterCollider;

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Sprite[] imposterSprites;
        
        private GameController gameController;
        public bool imposter;

        public void SetPopTime(float time, bool isImposter)
        {
            popTime = time;
            imposter = isImposter;
            gameController = FindObjectOfType<GameController>();
            var rand = new System.Random();

            if (imposter)
            {
                GetComponent<SpriteRenderer>().sprite = imposterSprites[rand.Next(imposterSprites.Length)];
                defaultCollider.enabled = false;
                imposterCollider.enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[rand.Next(sprites.Length)];
            }

            if (rand.Next(2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }
            
            StartCoroutine(LifeCycle());
        }

        private IEnumerator LifeCycle()
        {
            var counter = 0f;
            var deltaColor = Color.red - spriteRenderer.color;
            var scale = transform.localScale;
            var deltaScale = new Vector3(maxScale * scale.x, maxScale * scale.y, 1) - scale;
        
            while (counter < popTime)
            {
                counter += Time.deltaTime;

                if (changeColor)
                {
                    spriteRenderer.color += deltaColor * Time.deltaTime / popTime;
                }

                if (changeScale)
                {
                    transform.localScale += deltaScale * Time.deltaTime / popTime;
                }

                yield return null;
            }

            if (!imposter)
            {
                gameController.MissBall();
            }

            Destroy(gameObject);
        }
    }
}
