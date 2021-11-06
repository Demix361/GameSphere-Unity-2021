using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class MainMenuAmogus : MonoBehaviour
    {
        [SerializeField] private AmogusInfo[] crewmates;
        [SerializeField] private float updateInterval;

        private float halfHeight;
        private float halfWidth;
        private float count = 0;

        private void Start()
        {
            halfHeight = Camera.main.orthographicSize * 1.3f;
            halfWidth = halfHeight * Camera.main.aspect;
            GetComponent<SpriteRenderer>().sprite = crewmates[Random.Range(0, crewmates.Length)].crewmateSprite;

            if (Random.Range(0, 2) == 0)
            {
                var ls = transform.localScale;
                transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
            }
        }

        private void Update()
        {
            count += Time.deltaTime;

            if (count > updateInterval)
            {
                if (Math.Abs(transform.position.x) > halfWidth || Math.Abs(transform.position.y) > halfWidth)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}