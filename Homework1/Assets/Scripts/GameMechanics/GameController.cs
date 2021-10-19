using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        
        private void Start()
        {
            cam = Camera.main;
            var ballSR = ballPrefab.GetComponent<SpriteRenderer>();
            
            height = (cam.orthographicSize - ballSR.sprite.rect.size.y / ballSR.sprite.pixelsPerUnit / 2 * 
                ballSR.transform.localScale.y * ballPrefab.GetComponent<Ball>().maxScale);
            width = (cam.orthographicSize * Screen.width / Screen.height - ballSR.sprite.rect.size.x / ballSR.sprite.pixelsPerUnit / 2 * 
                ballSR.transform.localScale.x * ballPrefab.GetComponent<Ball>().maxScale);
        }
        
        private void OnEnable()
        {
            points = 0;
            missed = 0;
            StartCoroutine(SpawnBalls());
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = cam.ScreenToWorldPoint(Input.mousePosition);
                var a = Physics2D.OverlapPoint(pos);

                if (a != null && a.GetComponent<Ball>())
                {
                    if (!a.GetComponent<Ball>().imposter)
                    {
                        points += 1;
                        statsPanel.ChangePointsText(points);
                        Destroy(a.gameObject);

                        if (points % 100 == 0 && missed > 0)
                        {
                            missed -= 1;
                            statsPanel.DecreaseMissed();
                        }
                    }
                    else
                    {
                        EndGame();
                    }
                }
            }
        }
        
        public void InitializeGameController(int gameLength, bool changeColor, bool changeScale, bool changeRotation)
        {
            this.gameLength = gameLength;
            this.changeColor = changeColor;
            this.changeScale = changeScale;
            this.changeRotation = changeRotation;
        }
        
        private IEnumerator LifeCycle()
        {
            yield return new WaitForSeconds(gameLength);
            startPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        
        private IEnumerator SpawnBalls()
        {
            var z = 0f;
            var sortingOrder = 0;
            var timePassed = 0f;
            var curSpawnInterval = spawnInterval;
            
            while (true)
            {
                var pos = new Vector3(Random.Range(-width, height), Random.Range(-height, height), z);
                var ball = Instantiate(ballPrefab, pos, Quaternion.identity);

                var imposterChance = Random.Range(0f, 1f);
                if (imposterChance > 0.9f)
                {
                    ball.GetComponent<Ball>().SetPopTime(spawnInterval * 2, true);
                }
                else
                {
                    ball.GetComponent<Ball>().SetPopTime(spawnInterval * 2, false);
                }

                ball.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

                z -= 0.00001f;
                sortingOrder += 1;

                curSpawnInterval = Random.Range(spawnInterval * 0.5f, spawnInterval * 1.5f);
                yield return new WaitForSeconds(curSpawnInterval);
                timePassed += curSpawnInterval;

                spawnInterval = ProgressSpawnInterval(timePassed);
            }
        }

        private float ProgressSpawnInterval(float value)
        {
            // парабола (1): Начальная точка относительно (4) (1 + 4);
            // (2): Скорость уменьшения функции;
            // (3): Смещение графика по X;
            // (4): Предел к которому стремится функция;
            return 0.7f / (0.015f * value + 1) + 0.3f;
        }
        
        public void MissBall()
        {
            missed += 1;
            statsPanel.IncreaseMissed();

            if (missed >= 3)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            var highscore = PlayerPrefs.GetInt("highscore", 0);
            if (points > highscore)
            {
                PlayerPrefs.SetInt("highscore", points);
            }

            var amoguses = FindObjectsOfType<Ball>();
            foreach (var a in amoguses)
            {
                Destroy(a.gameObject);
            }
            
            statsPanel.gameObject.SetActive(false);
            startPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
