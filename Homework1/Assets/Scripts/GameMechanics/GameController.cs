using System;
using System.Collections;
using UnityEngine;

namespace GameMechanics
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private float gameLength;
        [SerializeField] private bool changeColor;
        [SerializeField] private bool changeScale;
        [SerializeField] private bool changeRotation;
        [SerializeField] private float spawnInterval;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private UI.StatsPanel statsPanel;
        [SerializeField] private UI.StartPanel startPanel;
        
        private int points = 0;
        private int missed = 0;
        private Camera cam;
        private float height;
        private float width;
        private System.Random rand;

        public void InitializeGameController(int gameLength, bool changeColor, bool changeScale, bool changeRotation)
        {
            this.gameLength = gameLength;
            this.changeColor = changeColor;
            this.changeScale = changeScale;
            this.changeRotation = changeRotation;
        }
        
        private void OnEnable()
        {
            print("here");
            cam = Camera.main;
            
            var ballSR = ballPrefab.GetComponent<SpriteRenderer>();
            
            height = (Camera.main.orthographicSize - ballSR.sprite.rect.size.y / ballSR.sprite.pixelsPerUnit / 2 * ballSR.transform.localScale.y * ballPrefab.GetComponent<Ball>().maxScale) * 2;
            width = height * Screen.width / Screen.height;

            rand = new System.Random();

            points = 0;
            missed = 0;
            statsPanel.ChangePointsText(points);
            statsPanel.ChangeMissedText(missed);
            
            StartCoroutine(SpawnBalls());
            StartCoroutine(LifeCycle());
        }
        
        private IEnumerator LifeCycle()
        {
            yield return new WaitForSeconds(gameLength);
            startPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = cam.ScreenToWorldPoint(Input.mousePosition);
                var a = Physics2D.OverlapPoint(pos);

                if (a != null && a.GetComponent<Ball>())
                {
                    points += 1;
                    statsPanel.ChangePointsText(points);
                    Destroy(a.gameObject);
                }
            }
        }

        private IEnumerator SpawnBalls()
        {
            var z = 0f;
            var sortingOrder = 0;
            var count = 0f;
            
            while (count < gameLength)
            {
                var pos = new Vector3(((float)rand.NextDouble() - 0.5f) * width, ((float)rand.NextDouble() - 0.5f) * height, z);
                //var pos = new Vector3(((float)rand.Next(2) - 0.5f) * width, ((float)rand.Next(2) - 0.5f) * height, z);
                var ball = Instantiate(ballPrefab, pos, Quaternion.identity);
                
                ball.GetComponent<Ball>().SetSettings(changeScale, changeColor);
                
                ball.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

                z -= 0.00001f;
                sortingOrder += 1;
                count += Time.deltaTime;
                print(count);
                
                yield return new WaitForSeconds(spawnInterval);
            }
            
            
        }

        public void MissBall()
        {
            missed += 1;
            statsPanel.ChangeMissedText(missed);
        }
    }
}
