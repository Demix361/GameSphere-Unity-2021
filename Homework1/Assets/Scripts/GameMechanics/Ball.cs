using System.Collections;
using UnityEngine;

namespace GameMechanics
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private bool changeScale;
        [SerializeField] private bool changeColor;
        [SerializeField] private float popTime;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] public float maxScale;
    
        private GameController gameController;

        private void Start()
        {
            gameController = FindObjectOfType<GameController>();
            StartCoroutine(LifeCycle());
        }

        public void SetSettings(bool changeScale, bool changeColor)
        {
            this.changeColor = changeColor;
            this.changeScale = changeScale;
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
        
            gameController.MissBall();
            Destroy(gameObject);
        }
    }
}
