using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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
        [SerializeField] private GameObject _frozenPrefab;
        [SerializeField] private GameObject _superCrewmatePrefab;
        [SerializeField] private AudioSource _missSound;

        private Camera _cam;
        private float _height;
        private float _width;
        private Coroutine _spawnBallsCoroutine;
        private Coroutine _inputCoroutine;
        private GameObject _endAnimation;
        private bool _stopTimer = false;

        private float z;
        private int sortingOrder;
        private float curTimeScale;
        private Coroutine _rageCoroutine;
        private Coroutine _frozenCoroutine;
        private Coroutine _endCoroutine;
        
        private enum GameType
        {
            Classic,
            Arcade
        }
        private GameType _curGameType;

        // Работает только один раз за все время жизни приложения
        private void Start()
        {
            _cam = Camera.main;
            _height = _cam.orthographicSize;
            _width = _cam.orthographicSize * _cam.aspect;
            
            _modelManager.ClassicGameModel.StartGame += StartClassic;
            _modelManager.ArcadeGameModel.StartGame += StartArcade;
            _modelManager.ClassicGameModel.CloseEndAnimation += CloseEndAnimation;
            _modelManager.ArcadeGameModel.CloseEndAnimation += CloseEndAnimation;

            _modelManager.ArcadeGameModel.PauseGame += PauseGame;
            _modelManager.ArcadeGameModel.UnpauseGame += UnpauseGame;
            _modelManager.ArcadeGameModel.StopGame += StopGame;
            
            _modelManager.ClassicGameModel.PauseGame += PauseGame;
            _modelManager.ClassicGameModel.UnpauseGame += UnpauseGame;
            _modelManager.ClassicGameModel.StopGame += StopGame;
        }

        private void PauseGame()
        {
            curTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        
        private void UnpauseGame()
        {
            Time.timeScale = curTimeScale;
        }

        private void StartClassic()
        {
            _curGameType = GameType.Classic;
            Time.timeScale = 1f;

            _spawnBallsCoroutine = StartCoroutine(ClassicSpawnBalls());
            _inputCoroutine = StartCoroutine(ClassicInputCoroutine());
        }

        private IEnumerator ClassicInputCoroutine()
        {
            while (true)
            {
                // for mouse input
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

                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        var pos = _cam.ScreenToWorldPoint(touch.position);
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
            Time.timeScale = 1f;

            _spawnBallsCoroutine = StartCoroutine(ArcadeSpawnBalls());
            _inputCoroutine = StartCoroutine(ArcadeInputCoroutine());
        }

        private IEnumerator ArcadeInputCoroutine()
        {
            var counter = _modelManager.ArcadeGameModel.CurTimer;
            var endProcessStarted = false;
            _stopTimer = false;
            var comboStarted = false;
            var lastComboHit = 0f;
            var comboLength = 0;
            var comboLevel = 0;
            var comboCount = 0;
            var lastComboScored = 0f;
            
            while (true)
            {
                if (endProcessStarted == false && counter <= 2f)
                {
                    ProcessSpecialGameEnd();
                    endProcessStarted = true;
                }

                if (!_stopTimer)
                {
                    counter -= Time.deltaTime;
                }

                if (endProcessStarted && counter <= 0)
                {
                    counter = 0.01f;
                }
                _modelManager.ArcadeGameModel.OnChangeTime(counter);

                if (comboStarted && lastComboHit - counter > 0.5f)
                {
                    // комбо закончилось
                    if (comboLength >= 3)
                    {
                        _modelManager.ArcadeGameModel.OnShowNotification($"Комбо из {comboLength}!\n+{comboLength}",
                            Color.yellow, new Vector2(0, 0));
                        _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + comboLength);
                        lastComboScored = counter;
                        comboCount += 1;
                    }

                    comboStarted = false;
                    comboLength = 0;
                }
                else if (comboStarted && comboLength >= 10)
                {
                    // принудительное завершение комбо на 10
                    _modelManager.ArcadeGameModel.OnShowNotification($"Максимальное комбо!\n+{comboLength}",
                        Color.red, new Vector2(0, 0));
                    _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + comboLength);

                    comboCount += 1;
                    
                    comboStarted = false;
                    comboLength = 0;
                }

                if (comboCount >= 3 && comboLevel < 5)
                {
                    // увеличение уровня комбо
                    comboCount = 0;
                    comboLevel += 1;

                    var points = comboLevel * 10;
                    
                    _modelManager.ArcadeGameModel.OnShowNotification($"{comboLevel * 3} комбо подряд!\n+{points}",
                        Color.green, new Vector2(0, 2));
                    _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + points);
                }
                
                // mouse input
                if (Input.GetMouseButtonDown(0))
                {
                    var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
                    
                    var a = Physics2D.OverlapPoint(pos);

                    if (a != null && a.CompareTag("Amogus"))
                    {
                        var amogus = a.GetComponent<IAmogus>();

                        if (amogus.Type == IAmogus.AmogusType.Impostor || amogus.Type == IAmogus.AmogusType.Super)
                        {
                            comboStarted = false;
                            comboLength = 0;

                            comboCount = 0;
                            comboLevel = 0;
                        }
                        else
                        {
                            if (!comboStarted)
                            {
                                comboStarted = true;
                            }
                            comboLength += 1;
                            lastComboHit = counter;
                        }

                        if (amogus.Type == IAmogus.AmogusType.Crewmate)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                            amogus.Clicked();
                        }
                        else if (amogus.Type == IAmogus.AmogusType.Impostor)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points - 10);
                            _modelManager.ArcadeGameModel.OnShowNotification("-10", Color.magenta, pos);
                            amogus.Clicked();
                            
                            StopAllBonuses();
                        }
                        else if (amogus.Type == IAmogus.AmogusType.Super)
                        {
                            _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                            amogus.Clicked(pos);
                        }
                        else if (amogus.Type == IAmogus.AmogusType.Rage)
                        {
                            amogus.Clicked();
                            StartRageSpawn();
                        }
                        else if (amogus.Type == IAmogus.AmogusType.Frozen)
                        {
                            amogus.Clicked();
                            StartFrozen();
                        }
                    }
                }

                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        var pos = _cam.ScreenToWorldPoint(touch.position);
                        var a = Physics2D.OverlapPoint(pos);

                        if (a != null && a.CompareTag("Amogus"))
                        {
                            var amogus = a.GetComponent<IAmogus>();
                        
                            if (amogus.Type == IAmogus.AmogusType.Crewmate)
                            {
                                _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                                amogus.Clicked();
                            }
                            else if (amogus.Type == IAmogus.AmogusType.Impostor)
                            {
                                _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points - 10);
                                amogus.Clicked();
                            }
                            else if (amogus.Type == IAmogus.AmogusType.Super)
                            {
                                _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                                amogus.Clicked(pos);
                            }
                            else if (amogus.Type == IAmogus.AmogusType.Rage)
                            {
                                amogus.Clicked();
                                StartRageSpawn();
                            }
                            else if (amogus.Type == IAmogus.AmogusType.Frozen)
                            {
                                amogus.Clicked();
                                StartFrozen();
                            }
                        }
                    }
                }
                
                yield return null;
            }
        }

        private IEnumerator ArcadeSpawnBalls()
        {
            z = 0f;
            sortingOrder = 0;
            var timePassed = 0f;
            var spawnInterval = _modelManager.ArcadeGameModel.SpawnInterval;
            var lastRageSpawn = _modelManager.ArcadeGameModel.CurTimer;
            var lastFrozenSpawn = _modelManager.ArcadeGameModel.CurTimer;

            while (true)
            {
                var pos = new Vector3(Random.Range(-_width, _width) * 0.85f, -_height, z);
                GameObject amogus;
                var lastSortingOrder = sortingOrder;
                var newAmogusType = GetArcadeAmogus(lastRageSpawn, lastFrozenSpawn);
                
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
                else if (newAmogusType == IAmogus.AmogusType.Frozen)
                {
                    amogus = Instantiate(_frozenPrefab, pos, Quaternion.identity);
                    sortingOrder += 2;
                    lastFrozenSpawn = _modelManager.ArcadeGameModel.CurTimer;
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
            _rageCoroutine = StartCoroutine(RageSpawnCoroutine());
        }

        private IEnumerator RageSpawnCoroutine()
        {
            var amogusNumber = Random.Range(20, 30);
            for (int i = 0; i < amogusNumber; i++)
            {
                var chance = 0;//Random.Range(0, 2);
                Vector3 pos;
                Vector2 force;
                if (chance == 0)
                {
                    pos = new Vector3(Random.Range(-_width, _width) * 0.85f, -_height, z);
                    force = CalculateForce(pos);
                }
                else
                {
                    pos = new Vector3(Random.Range(-_width, _width) * 0.85f, _height, z);
                    force = CalculateReverseForce(pos);
                }
                
                GameObject amogus;
                var lastSortingOrder = sortingOrder;
                
                amogus = Instantiate(_crewmatePrefab, pos, Quaternion.identity);
                sortingOrder += 1;

                amogus.GetComponent<IAmogus>()
                    .SetAmogus(_modelManager.ArcadeGameModel.ScaleSpeed, lastSortingOrder, this);
                amogus.GetComponent<Rigidbody2D>().AddForce(force);
                amogus.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50f, 50f));

                if (chance == 1)
                {
                    amogus.GetComponent<Rigidbody2D>().gravityScale = -1;
                }

                z -= 0.00001f;

                var spawnInterval = Random.Range(0.1f, 0.4f);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void StartFrozen()
        {
            StartCoroutine(FrozenCoroutine());
        }

        private IEnumerator FrozenCoroutine()
        {
            _stopTimer = true;
            Time.timeScale = 0.5f;
            
            yield return new WaitForSeconds(10f * Time.timeScale);

            _stopTimer = false;
            Time.timeScale = 1;
        }
        
        private void StopAllBonuses()
        {
            if (_rageCoroutine != null)
            {
                StopCoroutine(_rageCoroutine);
            }

            if (_frozenCoroutine != null)
            {
                StopCoroutine(_frozenCoroutine);
            }
            
            Time.timeScale = 1;
        }

        private void StopGame()
        {
            if (_spawnBallsCoroutine != null)
            {
                StopCoroutine(_spawnBallsCoroutine);
            }

            if (_inputCoroutine != null)
            {
                StopCoroutine(_inputCoroutine);
            }

            if (_endCoroutine != null)
            {
                StopCoroutine(_endCoroutine);
            }
            
            StopAllBonuses();

            var amoguses = GameObject.FindGameObjectsWithTag("Amogus");
            foreach (var a in amoguses)
            {
                a.GetComponent<IAmogus>().SafeDestroy();
            }
        }
        
        // Classic game ending
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
        
        // Arcade game ending
        private void ProcessSpecialGameEnd()
        {
            _endCoroutine = StartCoroutine(ProcessSpecialGameEndCoroutine());
        }

        private IEnumerator ProcessSpecialGameEndCoroutine()
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
        
        private Vector2 CalculateReverseForce(Vector3 startPos)
        {
            var res = Vector2.zero;
            
            var endPos = new Vector3(Random.Range(-_width, _width) * 0.85f, _height, 0);

            res.x = (endPos.x - startPos.x) * 20f;
            res.y = Random.Range(-450f, -630f);
            
            return res;
        }

        private IAmogus.AmogusType GetArcadeAmogus(float lastRageSpawn, float lastFrozenSpawn)
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
                var bonusTypeValue = Random.Range(0, 2);

                if (bonusTypeValue == 0 && lastRageSpawn - _modelManager.ArcadeGameModel.CurTimer > 10f)
                {
                    return IAmogus.AmogusType.Rage;
                }
                else if (bonusTypeValue == 1 && lastFrozenSpawn - _modelManager.ArcadeGameModel.CurTimer > 10f)
                {
                    return IAmogus.AmogusType.Frozen;
                }
            }
            
            return IAmogus.AmogusType.Crewmate;
        }
    }
}
