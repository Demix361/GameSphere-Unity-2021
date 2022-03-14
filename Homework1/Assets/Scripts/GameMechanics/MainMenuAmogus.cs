using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class MainMenuAmogus : MonoBehaviour
    {
        private bool entered = false;
        
        private void Start()
        {
            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }

            StartCoroutine(LifetimeCoroutine());
        }

        public void SetSprite(Sprite skin)
        {
            GetComponent<SpriteRenderer>().sprite = skin;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("DeleteZone"))
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("DeleteZone"))
            {
                entered = true;
            }
        }

        private IEnumerator LifetimeCoroutine()
        {
            yield return new WaitForSeconds(8);

            if (!entered)
            {
                Destroy(gameObject);
            }
        }
    }
}