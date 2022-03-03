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
        [SerializeField] private GameObject _frozenPrefab;
        [SerializeField] private GameObject _metalPrefab;
        [SerializeField] private GameObject _superCrewmatePrefab;
        [SerializeField] private AudioSource _missSound;

        private Camera _cam;
        private float _height;
        private float _width;
        private Coroutine _spawnBallsCoroutine;
        private Coroutine _inputCoroutine;
        private GameObject _endAnimation;
        private Coroutine _comboCoroutine;
        private bool _stopTimer = false;

        private float z;
        private int sortingOrder;
        private float curTimeScale;
        private Coroutine _rageCoroutine;
        private Coroutine _frozenCoroutine;
        private Coroutine _endCoroutine;
        
        private bool comboStarted = false;
        private float lastComboHit = 0f;
        private int comboLength = 0;
        private int comboLevel = 0;
        private int comboCount = 0;
        
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
            _width = _cam.orthographicSize * _cam.aspect * 0.9f;
            
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
                // mouse input
                if (Input.GetMouseButtonDown(0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
                {
                    var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
                    
                    ClassicTouchHandler(pos);
                }
                
                // touch input
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        var pos = _cam.ScreenToWorldPoint(touch.position);
                        
                        ClassicTouchHandler(pos);
                    }
                }

                yield return null;
            }
        }
        
        private void ClassicTouchHandler(Vector2 pos)
        {
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
                    _endAnimation.GetComponent<EndKillAlien>().SetBackgroundWidth(_cam.orthographicSize * _cam.aspect);

                    ProcessGameEnd();
                }
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
                amogus.GetComponent<IAmogus>().SetAmogus(lastSortingOrder, this);
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
            
            comboStarted = false;
            lastComboHit = 0f;
            comboLength = 0;
            comboLevel = 0;
            comboCount = 0;

            _spawnBallsCoroutine = StartCoroutine(ArcadeSpawnBalls());
            _inputCoroutine = StartCoroutine(ArcadeInputCoroutine());
            _comboCoroutine = StartCoroutine(ArcadeComboCoroutine());
        }

        private IEnumerator ArcadeInputCoroutine()
        {
            var counter = _modelManager.ArcadeGameModel.CurTimer;
            var endProcessStarted = false;
            _stopTimer = false;

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

                // mouse input
                if (Input.GetMouseButtonDown(0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer))
                {
                    var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
                    
                    ArcadeTouchHandler(pos);
                }
                
                // touch input
                for (int i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        var pos = _cam.ScreenToWorldPoint(touch.position);
                        
                        ArcadeTouchHandler(pos);
                    }
                }
                
                yield return null;
            }
        }

        private void ArcadeTouchHandler(Vector2 pos)
        {
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
                    lastComboHit = _modelManager.ArcadeGameModel.CurTimer;
                }

                if (amogus.Type == IAmogus.AmogusType.Crewmate)
                {
                    _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 1);
                    amogus.Clicked();
                }
                else if (amogus.Type == IAmogus.AmogusType.Impostor)
                {
                    _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points - 10);
                    _modelManager.ArcadeGameModel.OnShowNotification(Notification.Minus, points: 10, pos:pos);
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
                else if (amogus.Type == IAmogus.AmogusType.Metal)
                {
                    if (amogus.Clicked())
                    {
                        _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + 5);
                        _modelManager.ArcadeGameModel.OnShowNotification(Notification.Plus, points: 5, pos:pos);
                    }
                }
            }
        }

        private IEnumerator ArcadeComboCoroutine()
        {
            while (true)
            {
                if (comboStarted && lastComboHit - _modelManager.ArcadeGameModel.CurTimer > 0.5f)
                {
                    // комбо закончилось
                    if (comboLength >= 3)
                    {
                        _modelManager.ArcadeGameModel.OnShowNotification(Notification.ComboOf, comboLength, comboLength);
                        _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + comboLength);
                        comboCount += 1;
                    }

                    comboStarted = false;
                    comboLength = 0;
                }
                else if (comboStarted && comboLength >= 10)
                {
                    // принудительное завершение комбо на 10
                    _modelManager.ArcadeGameModel.OnShowNotification(Notification.MaxCombo, points: 10);
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
                    
                    _modelManager.ArcadeGameModel.OnShowNotification(Notification.ComboInRow, comboLevel * 3, points, pos:new Vector2(0, 2f), delay:1f);
                    _modelManager.ArcadeGameModel.OnChangePoints(_modelManager.ArcadeGameModel.Points + points);
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
                else if (newAmogusType == IAmogus.AmogusType.Metal)
                {
                    amogus = Instantiate(_metalPrefab, pos, Quaternion.identity);
                    sortingOrder += 2;
                }
                else
                {
                    amogus = Instantiate(_crewmatePrefab, pos, Quaternion.identity);
                    sortingOrder += 1;
                }
                
                amogus.GetComponent<IAmogus>().SetAmogus(lastSortingOrder, this);
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
                Vector3 pos;
                Vector2 force;
                
                pos = new Vector3(Random.Range(-_width, _width) * 0.85f, -_height, z);
                force = CalculateForce(pos);

                GameObject amogus;
                var lastSortingOrder = sortingOrder;
                
                amogus = Instantiate(_crewmatePrefab, pos, Quaternion.identity);
                sortingOrder += 1;

                amogus.GetComponent<IAmogus>()
                    .SetAmogus(lastSortingOrder, this);
                amogus.GetComponent<Rigidbody2D>().AddForce(force);
                amogus.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50f, 50f));

                z -= 0.00001f;

                var spawnInterval = Random.Range(0.1f, 0.4f);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void StartFrozen()
        {
            _frozenCoroutine = StartCoroutine(FrozenCoroutine());
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

            if (_comboCoroutine != null)
            {
                StopCoroutine(_comboCoroutine);
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
            
            StopCoroutine(_comboCoroutine);
            
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
            
            amogus.GetComponent<IAmogus>().SetAmogus(0, this);
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

        private IAmogus.AmogusType GetArcadeAmogus(float lastRageSpawn, float lastFrozenSpawn)
        {
            var typeChance = Random.Range(0f, 1f);
            var imposterChance = _modelManager.ArcadeGameModel.ImposterChance;
            var bonusChance = _modelManager.ArcadeGameModel.BonusChance;
            var metalChance = _modelManager.ArcadeGameModel.MetalChance;
            
            if (typeChance < imposterChance)
            {
                return IAmogus.AmogusType.Impostor;
            }
            
            if (typeChance < imposterChance + metalChance)
            {
                return IAmogus.AmogusType.Metal;
            }
            
            if (typeChance < imposterChance + metalChance + bonusChance && _modelManager.ArcadeGameModel.CurTimer > 10f)
            {
                var bonusTypeValue = Random.Range(0, 2);

                if (bonusTypeValue == 0 && lastRageSpawn - _modelManager.ArcadeGameModel.CurTimer > 10f)
                {
                    return IAmogus.AmogusType.Rage;
                }
                
                if (bonusTypeValue == 1 && lastFrozenSpawn - _modelManager.ArcadeGameModel.CurTimer > 10f)
                {
                    return IAmogus.AmogusType.Frozen;
                }
            }
            
            return IAmogus.AmogusType.Crewmate;
        }
    }
}
