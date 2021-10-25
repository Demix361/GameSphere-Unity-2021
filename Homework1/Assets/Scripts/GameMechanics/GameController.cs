using System.Collections;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private WindowManager _windowManager;
        [SerializeField] private GameObject ballPrefab;
        
        private Camera cam;
        private float height;
        private float width;
        private float originalSpawnInterval;

        private Coroutine _spawnBallsCoroutine;
        private Coroutine _inputCoroutine;
        
        public void StartClassic()
        {
            cam = Camera.main;
            var ballSR = ballPrefab.GetComponent<SpriteRenderer>();
            
            height = (cam.orthographicSize - ballSR.sprite.rect.size.y / ballSR.sprite.pixelsPerUnit / 2 * 
                ballSR.transform.localScale.y * ballPrefab.GetComponent<Ball>().maxScale);
            width = (cam.orthographicSize * Screen.width / Screen.height - ballSR.sprite.rect.size.x / ballSR.sprite.pixelsPerUnit / 2 * 
                ballSR.transform.localScale.x * ballPrefab.GetComponent<Ball>().maxScale);
            
            _spawnBallsCoroutine = StartCoroutine(ClassicSpawnBalls());
            _inputCoroutine = StartCoroutine(ClassicInputCoroutine());
        }

        private IEnumerator ClassicInputCoroutine()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var pos = cam.ScreenToWorldPoint(Input.mousePosition);
                    var a = Physics2D.OverlapPoint(pos);

                    if (a != null && a.GetComponent<Ball>())
                    {
                        if (a.GetComponent<Ball>()._type == "Default")
                        {
                            _windowManager.ClassicGameModel.OnChangePoints(_windowManager.ClassicGameModel.Points + 1);
                            Destroy(a.gameObject);
                        }
                        else if (a.GetComponent<Ball>()._type == "Imposter")
                        {
                            _windowManager.ClassicGameModel.OnEndGame();
                            ProcessGameEnd();
                        }
                    }
                }
                yield return null;
            }
        }

        private IEnumerator ClassicSpawnBalls()
        {
            var z = 0f;
            var sortingOrder = 0;
            var timePassed = 0f;
            var spawnInterval = _windowManager.ClassicGameModel.SpawnInterval;
            var curSpawnInterval = spawnInterval;
            
            while (true)
            {
                var pos = new Vector3(Random.Range(-width, height), Random.Range(-height, height), z);
                var ball = Instantiate(ballPrefab, pos, Quaternion.identity);

                var imposterChance = Random.Range(0f, 1f);
                if (imposterChance > 0.9f)
                {
                    ball.GetComponent<Ball>().SetAmogus(spawnInterval * 2, "Imposter", "Classic");
                }
                else
                {
                    ball.GetComponent<Ball>().SetAmogus(spawnInterval * 2, "Default", "Classic");
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
        
        public void StartArcade()
        {
            cam = Camera.main;
            var ballSR = ballPrefab.GetComponent<SpriteRenderer>();
            
            height = (cam.orthographicSize - ballSR.sprite.rect.size.y / ballSR.sprite.pixelsPerUnit / 2 * 
                ballSR.transform.localScale.y * ballPrefab.GetComponent<Ball>().maxScale);
            width = (cam.orthographicSize * Screen.width / Screen.height - ballSR.sprite.rect.size.x / ballSR.sprite.pixelsPerUnit / 2 * 
                ballSR.transform.localScale.x * ballPrefab.GetComponent<Ball>().maxScale);
            
            _spawnBallsCoroutine = StartCoroutine(ArcadeSpawnBalls());
            _inputCoroutine = StartCoroutine(ArcadeInputCoroutine());
        }
        
         private IEnumerator ArcadeInputCoroutine()
        {
            var counter = _windowManager.ArcadeGameModel.CurTimer;
            
            while (true)
            {
                counter -= Time.deltaTime;
                _windowManager.ArcadeGameModel.OnChangeTime(counter);

                if (counter <= 0)
                {
                    ProcessGameEnd();
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    var pos = cam.ScreenToWorldPoint(Input.mousePosition);
                    var a = Physics2D.OverlapPoint(pos);

                    if (a != null && a.GetComponent<Ball>())
                    {
                        if (a.GetComponent<Ball>()._type == "Default")
                        {
                            _windowManager.ArcadeGameModel.OnChangePoints(_windowManager.ArcadeGameModel.Points + 1);
                            Destroy(a.gameObject);
                        }
                        else if (a.GetComponent<Ball>()._type == "Bonus")
                        {
                            counter += 3;
                            _windowManager.ArcadeGameModel.OnChangeTime(counter);
                            Destroy(a.gameObject);
                        }
                        else if (a.GetComponent<Ball>()._type == "Imposter")
                        {
                            _windowManager.ArcadeGameModel.OnChangePoints(_windowManager.ArcadeGameModel.Points - 10);
                            Destroy(a.gameObject);
                        }
                    }
                }
                yield return null;
            }
        }

        private IEnumerator ArcadeSpawnBalls()
        {
            var z = 0f;
            var sortingOrder = 0;
            var timePassed = 0f;
            var spawnInterval = _windowManager.ArcadeGameModel.SpawnInterval;
            var curSpawnInterval = spawnInterval;

            while (true)
            {
                var pos = new Vector3(Random.Range(-width, height), Random.Range(-height, height), z);
                var ball = Instantiate(ballPrefab, pos, Quaternion.identity);

                var typeChance = Random.Range(0f, 1f);
                if (typeChance > 0.8f && typeChance <= 0.9f)
                {
                    ball.GetComponent<Ball>().SetAmogus(spawnInterval * 2, "Bonus", "Arcade");
                }
                else if (typeChance > 0.9f)
                {
                    ball.GetComponent<Ball>().SetAmogus(spawnInterval * 2, "Imposter", "Arcade");
                }
                else
                {
                    ball.GetComponent<Ball>().SetAmogus(spawnInterval * 2, "Default", "Arcade");
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
            if (_windowManager.ClassicGameModel.OnChangeLives(_windowManager.ClassicGameModel.CurLives - 1))
            {
                ProcessGameEnd();
            }
        }

        private void ProcessGameEnd()
        {
            StopCoroutine(_spawnBallsCoroutine);
            StopCoroutine(_inputCoroutine);
            
            var amoguses = FindObjectsOfType<Ball>();
            foreach (var a in amoguses)
            {
                Destroy(a.gameObject);
            }
        }
    }
}
