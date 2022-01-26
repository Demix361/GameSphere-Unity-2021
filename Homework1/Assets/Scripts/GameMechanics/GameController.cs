using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ModelManager _modelManager;
        [SerializeField] private GameObject _endGameAlienPrefab;
        [SerializeField] private GameObject _endGameWalkPrefab;
        [SerializeField] private GameObject _crewmatePrefab;
        [SerializeField] private GameObject _impostorPrefab;
        [SerializeField] private GameObject _ragePrefab;
        [SerializeField] private GameObject _superCrewmatePrefab;
        [SerializeField] private AudioSource _missSound;

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
            _cam = Camera.main;
            _height = _cam.orthographicSize;
            _width = _cam.orthographicSize * _cam.aspect;
            
            _modelManager.ClassicGameModel.StartGame += StartClassic;
            _modelManager.ArcadeGameModel.StartGame += StartArcade;
            _modelManager.ClassicGameModel.CloseEndAnimation += CloseEndAnimation;
            _modelManager.ArcadeGameModel.CloseEndAnimation += CloseEndAnimation;
        }

        private void StartClassic()
        {
            _curGameType = GameType.Classic;

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
                    
                    if (a != null && a.CompareTag("Amogus"))
                    {
                        var amogus = a.GetComponent<IAmogus>();
                        
                        if (amogus.Type == IAmogus.AmogusType.Crewmate)
                        {
                            _modelManager.ClassicGameModel.OnChangePoints(_modelManager.ClassicGameModel.Points + 1);
                            amogus.Clicked();
                        }
                        else if (amogus.Type == IAmogus.AmogusType.Impostor)
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
                GameObject amogus;
                
                var lastSortingOrder = sortingOrder;

                var typeChance = Random.Range(0f, defaultC + imposterC);
                if (typeChance <= imposterC)
                {
                    amogus = Instantiate(_impostorPrefab, pos, Quaternion.identity);
                    sortingOrder += 2;
                }
                else
                {
                    amogus = Instantiate(_crewmatePrefab, pos, Quaternion.identity);
                    sortingOrder += 1;
                }
                amogus.GetComponent<IAmogus>().SetAmogus(_modelManager.ClassicGameModel.ScaleSpeed, lastSortingOrder, this);
                amogus.GetComponent<Rigidbody2D>().AddForce(CalculateForce(pos));
                amogus.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50f, 50f));

                z -= 0.00001f;

                spawnInterval = Random.Range(spawnInterval * 0.5f, spawnInterval * 1.5f);
                yield return new WaitForSeconds(spawnInterval);
                timePassed += spawnInterval;

                spawnInterval = _modelManager.ClassicGameModel.ProgressSpawnInterval(timePassed);
            }
        }
        
        private void StartArcade()
        {
            _curGameType = GameType.Arcade;

            _spawnBallsCoroutine = StartCoroutine(ArcadeSpawnBalls());
            _inputCoroutine = StartCoroutine(ArcadeInputCoroutine());
        }

        private IEnumerator ArcadeInputCoroutine()
        {
            var counter = _modelManager.ArcadeGameModel.CurTimer;
            var endProcessStarted = false;
            
            while (true)
            {
                /*
                if (counter <= 0)
                {
                    ProcessGameEnd();
                }
                */
                if (endProcessStarted == false && counter <= 2f)
                {
                    ProcessSpecialGameEnd();
                    endProcessStarted = true;
                }
                
                counter -= Time.deltaTime;
                
                if (endProcessStarted && counter <= 0)
                {
                    counter = 0.01f;
                }
                
                _modelManager.ArcadeGameModel.OnChangeTime(counter);
                
                if (Input.GetMouseButtonDown(0))
                {
                    var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
                    var a = Physics2D.OverlapPoint(pos);

                    if (a != null && a.CompareTag("Amogus"))
                    {
                        var amogus = a.GetComponent<IAmogus>();
                        
                        if (amogus.Type == IAmogus.AmogusType.Crewmate)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                            amogus.Clicked();
                        }
                        /* else if (amogus.Type == IAmogus.AmogusType.Bonus)
                        {
                            counter += 3;
                            _modelManager.ArcadeGameModel.OnChangeTime(counter);
                            amogus.Clicked();
                        }*/
                        else if (amogus.Type == IAmogus.AmogusType.Impostor)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points - 10);
                            amogus.Clicked();
                        }
                        else if (amogus.Type == IAmogus.AmogusType.SuperCrewmate)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                            amogus.Clicked(pos);
                        }
                        else if (amogus.Type == IAmogus.AmogusType.Rage)
                        {
                            amogus.Clicked();
                            StartRageSpawn();
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
            var impostorC = _modelManager.ArcadeGameModel.ImposterChance;
            var bonusC = _modelManager.ArcadeGameModel.BonusChance;
            var lastRageSpawn = _modelManager.ArcadeGameModel.CurTimer;
            var lastName2Spawn = _modelManager.ArcadeGameModel.CurTimer;
            var lastName3Spawn = _modelManager.ArcadeGameModel.CurTimer;
            
            while (true)
            {
                var pos = new Vector3(Random.Range(-_width, _width) * 0.85f, -_height, z);
                GameObject amogus;
                var lastSortingOrder = sortingOrder;
                var newAmogusType = GetArcadeAmogus(lastRageSpawn, lastName2Spawn, lastName3Spawn);
                
                if (newAmogusType == IAmogus.AmogusType.Impostor)
                {
                    amogus = Instantiate(_impostorPrefab, pos, Quaternion.identity);
                    sortingOrder += 2;
                }
                else if (newAmogusType == IAmogus.AmogusType.Rage)
                {
                    amogus = Instantiate(_ragePrefab, pos, Quaternion.identity);
                    sortingOrder += 2;
                    lastRageSpawn = _modelManager.ArcadeGameModel.CurTimer;
                }
                else
                {
                    amogus = Instantiate(_crewmatePrefab, pos, Quaternion.identity);
                    sortingOrder += 1;
                }
                
                amogus.GetComponent<IAmogus>().SetAmogus(_modelManager.ArcadeGameModel.ScaleSpeed, lastSortingOrder, this);
                amogus.GetComponent<Rigidbody2D>().AddForce(CalculateForce(pos));
                amogus.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50f, 50f));
                
                z -= 0.00001f;
                
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
                _missSound.Play();
                if (_modelManager.ClassicGameModel.OnChangeLives(_modelManager.ClassicGameModel.CurLives - 1))
                {
                    ProcessGameEnd();
                }
            }
        }

        private void StartRageSpawn()
        {
            
        }
        
        private void ProcessGameEnd()
        {
            StopCoroutine(_spawnBallsCoroutine);
            StopCoroutine(_inputCoroutine);

            if (_endAnimation == null)
            {
                _endAnimation = Instantiate(_endGameWalkPrefab);
            }

            var amoguses = GameObject.FindGameObjectsWithTag("Amogus");
            foreach (var a in amoguses)
            {
                a.GetComponent<IAmogus>().SafeDestroy();
            }
        }

        private void ProcessSpecialGameEnd()
        {
            StartCoroutine(ProcessSpecialGameEndCoroutine(6.5f));
        }

        private IEnumerator ProcessSpecialGameEndCoroutine(float delayTime)
        {
            StopCoroutine(_spawnBallsCoroutine);

            while (true)
            {
                if (GameObject.FindGameObjectsWithTag("Amogus").Length == 0)
                {
                    break;
                }

                yield return new WaitForSeconds(0.2f);
            }
            
            yield return new WaitForSeconds(1f);

            SpawnSuperCrewmate();
            
            while (true)
            {
                if (GameObject.FindGameObjectsWithTag("Amogus").Length == 0)
                {
                    break;
                }

                yield return new WaitForSeconds(0.2f);
            }
            
            yield return new WaitForSeconds(1f);
            
            StopCoroutine(_inputCoroutine);
            if (_endAnimation == null)
            {
                _endAnimation = Instantiate(_endGameWalkPrefab);
            }

            var amoguses = GameObject.FindGameObjectsWithTag("Amogus");
            foreach (var a in amoguses)
            {
                a.GetComponent<IAmogus>().SafeDestroy();
            }
            
            _modelManager.ArcadeGameModel.OnChangeTime(0f);
        }

        private void SpawnSuperCrewmate()
        {
            var pos = new Vector3(Random.Range(-_width, _width) * 0.15f, -_height, -1);
            var amogus = Instantiate(_superCrewmatePrefab, pos, Quaternion.identity);
            
            amogus.GetComponent<IAmogus>().SetAmogus(_modelManager.ArcadeGameModel.ScaleSpeed, 0, this);
            amogus.GetComponent<Rigidbody2D>().AddForce(CalculateForce(pos));
            amogus.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50f, 50f));
        }
        
        
        private void CloseEndAnimation()
        {
            Destroy(_endAnimation);
        }

        private Vector2 CalculateForce(Vector3 startPos)
        {
            var res = Vector2.zero;
            
            var endPos = new Vector3(Random.Range(-_width, _width) * 0.85f, -_height, 0);

            res.x = (endPos.x - startPos.x) * 20f;
            res.y = Random.Range(450f, 630f);
            
            return res;
        }

        private IAmogus.AmogusType GetArcadeAmogus(float lastRageSpawn, float lastName2Spawn, float lastName3Spawn)
        {
            var typeChance = Random.Range(0f, 1f);
            var imposterChance = _modelManager.ArcadeGameModel.ImposterChance;
            var bonusChance = _modelManager.ArcadeGameModel.BonusChance;
            
            if (typeChance < imposterChance)
            {
                return IAmogus.AmogusType.Impostor;
            }
            else if (typeChance < imposterChance + bonusChance && _modelManager.ArcadeGameModel.CurTimer > 10f)
            {
                var bonusTypeValue = Random.Range(0, 3);

                if (bonusTypeValue == 0 && lastRageSpawn - _modelManager.ArcadeGameModel.CurTimer > 10f)
                {
                    return IAmogus.AmogusType.Rage;
                }
                else if (bonusTypeValue == 1 && lastName2Spawn - _modelManager.ArcadeGameModel.CurTimer > 10f)
                {
                    return IAmogus.AmogusType.Rage;
                }
                else if (bonusTypeValue == 2 && lastName3Spawn - _modelManager.ArcadeGameModel.CurTimer > 10f)
                {
                    return IAmogus.AmogusType.Rage;
                }
            }
            
            return IAmogus.AmogusType.Crewmate;
        }
    }
}
