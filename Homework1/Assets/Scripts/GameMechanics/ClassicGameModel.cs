using System;
using UnityEngine;

namespace GameMechanics
{
    public class ClassicGameModel
    {
        private static int _lives = 3;
        private float _spawnInterval = 1f;
        private int _restoreLifePoints = 100;
        private int _curLives = _lives;
        private PlayerModel _playerModel;
        public float ImposterChance { get; } = 0.1f;
        public float DefaultChance { get; } = 0.9f;
        public int MoneyForGame { get; set; } = 0;
        
        public ClassicGameModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public int Lives => _lives;

        public float SpawnInterval
        {
            get
            {
                return _spawnInterval;
            }
            set
            {
                _spawnInterval = value;
            }
        }
        public int RestoreLifePoints => _restoreLifePoints;

        public int Points { get; set; } = 0;

        public int CurLives
        {
            get
            {
                return _curLives;
            }
            set
            {
                _curLives = value;
            }
        }

        public event Action EndGameEvent;
        public event Action<int> ChangePointsEvent;
        public event Action<int> ChangeLivesEvent;
        public event Action StartGame;
        public event Action CloseEndAnimation;
        
        public event Action PauseGame;
        public event Action UnpauseGame;
        
        public event Action StopGame;
        
        public void OnStopGame()
        {
            StopGame?.Invoke();
        }

        public void OnPauseGame()
        {
            PauseGame?.Invoke();
        }

        public void OnUnpauseGame()
        {
            UnpauseGame?.Invoke();
        }

        public void OnCloseEndAnimation()
        {
            CloseEndAnimation?.Invoke();
        }
        
        public void OnStartGame()
        {
            //CurLives = Lives;
            //Points = 0;
            OnChangeLives(Lives);
            OnChangePoints(0);
            StartGame?.Invoke();
        }
        
        public void OnChangePoints(int newValue)
        {
            ChangePointsEvent?.Invoke(newValue);
            
            Points = newValue;
            if (Points % RestoreLifePoints == 0 && CurLives < Lives)
            {
                CurLives += 1;
                ChangeLivesEvent?.Invoke(CurLives);
            }
        }

        public bool OnChangeLives(int newValue)
        {
            CurLives = newValue;
            
            if (CurLives >= 0)
            {
                ChangeLivesEvent?.Invoke(CurLives);
            }

            if (CurLives <= 0)
            {
                OnEndGame();
                return true;
            }

            return false;
        }

        public void OnEndGame()
        {
            if (Points > _playerModel.HighScoreClassic)
            {
                _playerModel.HighScoreClassic = Points;
            }
            
            MoneyForGame = Points / 5;
            _playerModel.AddMoney(MoneyForGame);
            
            EndGameEvent?.Invoke();
        }
        
        public float ProgressSpawnInterval(float value)
        {
            // парабола
            // (1): Начальная точка относительно (4) (1 + 4);
            // (2): Скорость уменьшения функции;
            // (3): Смещение графика по X;
            // (4): Предел к которому стремится функция;
            return (_spawnInterval - 0.3f) / (0.015f * value + 1) + 0.3f;
        }
    }
}