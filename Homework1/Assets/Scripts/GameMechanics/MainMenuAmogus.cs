using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class MainMenuAmogus : MonoBehaviour
    {
        [SerializeField] private AmogusInfo[] crewmates;

        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = crewmates[Random.Range(0, crewmates.Length)].crewmateSprite;

            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("DeleteZone"))
            {
                Destroy(gameObject);
            }
        }
    }
}