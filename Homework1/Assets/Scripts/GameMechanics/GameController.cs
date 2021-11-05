using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ModelManager _modelManager;
        [SerializeField] private GameObject _amogusPrefab;
        [SerializeField] private GameObject _endGameAlienPrefab;
        [SerializeField] private GameObject _endGameWalkPrefab;

        private Camera _cam;
        private float _height;
        private float _width;
        private Coroutine _spawnBallsCoroutine;
        private Coroutine _inputCoroutine;
        private GameObject _endAnimation;
        private enum GameType
        {
            Classic,
            Arcade
        }
        private GameType _curGameType;

        
        private void Start()
        {
            _modelManager.ClassicGameModel.StartGame += StartClassic;
            _modelManager.ArcadeGameModel.StartGame += StartArcade;
            _modelManager.ClassicGameModel.CloseEndAnimation += CloseEndAnimation;
        }

        public void StartClassic()
        {
            _curGameType = GameType.Classic;
            _cam = Camera.main;
            _height = _cam.orthographicSize;
            _width = _cam.orthographicSize * _cam.aspect;
            
            _spawnBallsCoroutine = StartCoroutine(ClassicSpawnBalls());
            _inputCoroutine = StartCoroutine(ClassicInputCoroutine());
        }

        private IEnumerator ClassicInputCoroutine()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
                    var a = Physics2D.OverlapPoint(pos);
                    
                    if (a != null && a.GetComponent<Ball>())
                    {
                        var amogus = a.GetComponent<Ball>();
                        
                        if (amogus.Type == Ball.AmogusType.Default)
                        {
                            _modelManager.ClassicGameModel.OnChangePoints(_modelManager.ClassicGameModel.Points + 1);
                            amogus.Clicked();
                        }
                        else if (amogus.Type == Ball.AmogusType.Imposter)
                        {
                            _modelManager.ClassicGameModel.OnEndGame();
                            
                            _endAnimation = Instantiate(_endGameAlienPrefab, Vector3.zero, Quaternion.identity);
                            _endAnimation.GetComponent<EndKillAlien>().ImpostorAnimator.runtimeAnimatorController = amogus.Info.alienKillAnimator;

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
            var spawnInterval = _modelManager.ClassicGameModel.SpawnInterval;
            var defaultC = _modelManager.ClassicGameModel.DefaultChance;
            var imposterC = _modelManager.ClassicGameModel.ImposterChance;

            while (true)
            {
                var pos = new Vector3(Random.Range(-_width, _width), -_height, z);
                var ball = Instantiate(_amogusPrefab, pos, Quaternion.identity);
                ball.GetComponent<Rigidbody2D>().AddForce(CalculateForce(pos));
                ball.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50f, 50f));

                var typeChance = Random.Range(0f, defaultC + imposterC);
                if (typeChance <= imposterC)
                {
                    ball.GetComponent<Ball>().SetAmogus(_modelManager.ClassicGameModel.ScaleSpeed, Ball.AmogusType.Imposter, this);
                }
                else
                {
                    ball.GetComponent<Ball>().SetAmogus(_modelManager.ClassicGameModel.ScaleSpeed, Ball.AmogusType.Default, this);
                }

                ball.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

                z -= 0.00001f;
                sortingOrder += 1;

                spawnInterval = Random.Range(spawnInterval * 0.5f, spawnInterval * 1.5f);
                yield return new WaitForSeconds(spawnInterval);
                timePassed += spawnInterval;

                spawnInterval = _modelManager.ClassicGameModel.ProgressSpawnInterval(timePassed);
            }
        }
        
        public void StartArcade()
        {
            _curGameType = GameType.Arcade;
            _cam = Camera.main;
            _height = _cam.orthographicSize;
            _width = _cam.orthographicSize * _cam.aspect;
            
            _spawnBallsCoroutine = StartCoroutine(ArcadeSpawnBalls());
            _inputCoroutine = StartCoroutine(ArcadeInputCoroutine());
        }
        
        private IEnumerator ArcadeInputCoroutine()
        {
            var counter = _modelManager.ArcadeGameModel.CurTimer;
            
            while (true)
            {
                counter -= Time.deltaTime;
                _modelManager.ArcadeGameModel.OnChangeTime(counter);

                if (counter <= 0)
                {
                    ProcessGameEnd();
                }
                
                if (Input.GetMouseButtonDown(0))
                {
                    var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
                    var a = Physics2D.OverlapPoint(pos);

                    if (a != null && a.GetComponent<Ball>())
                    {
                        var amogus = a.GetComponent<Ball>();
                        
                        if (amogus.Type == Ball.AmogusType.Default)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                            amogus.Clicked();
                        }
                        else if (amogus.Type == Ball.AmogusType.Bonus)
                        {
                            counter += 3;
                            _modelManager.ArcadeGameModel.OnChangeTime(counter);
                            amogus.Clicked();
                        }
                        else if (amogus.Type == Ball.AmogusType.Imposter)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points - 10);
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
            var spawnInterval = _modelManager.ArcadeGameModel.SpawnInterval;
            var defaultC = _modelManager.ArcadeGameModel.DefaultChance;
            var imposterC = _modelManager.ArcadeGameModel.ImposterChance;
            var bonusC = _modelManager.ArcadeGameModel.BonusChance;
            
            while (true)
            {
                var pos = new Vector3(Random.Range(-_width, _width) * 0.85f, -_height, z);
                var ball = Instantiate(_amogusPrefab, pos, Quaternion.identity);
                ball.GetComponent<Rigidbody2D>().AddForce(CalculateForce(pos));
                ball.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50f, 50f));

                var typeChance = Random.Range(0f, defaultC + imposterC + bonusC);
                if (typeChance <= defaultC)
                {
                    ball.GetComponent<Ball>().SetAmogus(_modelManager.ArcadeGameModel.ScaleSpeed, Ball.AmogusType.Default, this);
                }
                else if (typeChance <= defaultC + imposterC)
                {
                    ball.GetComponent<Ball>().SetAmogus(_modelManager.ArcadeGameModel.ScaleSpeed, Ball.AmogusType.Imposter, this);
                }
                else
                {
                    ball.GetComponent<Ball>().SetAmogus(_modelManager.ArcadeGameModel.ScaleSpeed, Ball.AmogusType.Bonus, this);
                    ball.GetComponent<Ball>().bonus.sortingOrder = sortingOrder + 1;
                }

                ball.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

                z -= 0.00001f;
                sortingOrder += 2;

                spawnInterval = Random.Range(spawnInterval * 0.5f, spawnInterval * 1.5f);
                yield return new WaitForSeconds(spawnInterval);
                timePassed += spawnInterval;

                spawnInterval = _modelManager.ArcadeGameModel.ProgressSpawnInterval(timePassed);
            }
        }
        
        public void MissBall()
        {
            if (_curGameType == GameType.Classic)
            {
                if (_modelManager.ClassicGameModel.OnChangeLives(_modelManager.ClassicGameModel.CurLives - 1))
                {
                    ProcessGameEnd();
                }
            }
        }

        private void ProcessGameEnd()
        {
            StopCoroutine(_spawnBallsCoroutine);
            StopCoroutine(_inputCoroutine);

            if (_endAnimation == null)
            {
                _endAnimation = Instantiate(_endGameWalkPrefab);
            }
            
            var amoguses = FindObjectsOfType<Ball>();
            foreach (var a in amoguses)
            {
                Destroy(a.gameObject);
            }
        }
        
        private void CloseEndAnimation()
        {
            Destroy(_endAnimation);
        }

        private Vector2 CalculateForce(Vector3 startPos)
        {
            var res = Vector2.zero;
            
            var endPos = new Vector3(Random.Range(-_width, _width) * 0.85f, -_height, 0);

            res.x = (endPos.x - startPos.x) * 2f;
            res.y = Random.Range(450f, 630f);
            
            return res;
        }
    }
}
