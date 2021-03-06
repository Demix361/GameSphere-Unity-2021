using System;
using UnityEngine;

namespace GameMechanics
{
    public class ArcadeGameModel
    {
        private PlayerModel _playerModel;
        private const float Timer = 60;
        private float _spawnInterval = 1f;
        
        public ArcadeGameModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }
        public float CurTimer { get; set; } = Timer;
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
        public int Points { get; set; } = 0;
        public float ImposterChance { get; } = 0.15f;
        public float BonusChance { get; } = 0.05f;
        public float MetalChance { get; } = 0.2f;
        public int MoneyForGame { get; set; } = 0;
        
        public event Action EndGameEvent;
        public event Action<int> ChangePointsEvent;
        public event Action<float> ChangeTimeEvent;
        public event Action StartGame;
        public event Action PauseGame;
        public event Action UnpauseGame;
        public event Action CloseEndAnimation;
        public event Action<Notification, int, int, Vector2, float> ShowNotification;
        public event Action StopGame;
        
        
        public void OnCloseEndAnimation()
        {
            CloseEndAnimation?.Invoke();
        }
        
        public void OnStartGame()
        {
            CurTimer = Timer;
            Points = 0;
            StartGame?.Invoke();
        }

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
        
        public void OnChangePoints(int newValue)
        {
            Points = newValue < 0 ? 0 : newValue;

            ChangePointsEvent?.Invoke(Points);
        }

        public void OnChangeTime(float newValue)
        {
            CurTimer = newValue;
            
            if (CurTimer <= 0)
            {
                OnEndGame();
            }
            else
            {
                ChangeTimeEvent?.Invoke(newValue);
            }
        }

        public void OnEndGame()
        {
            if (Points > _playerModel.HighScoreArcade)
            {
                _playerModel.HighScoreArcade = Points;
            }

            MoneyForGame = Points / 10;
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
            return (_spawnInterval - 0.3f) / (0.045f * value + 1) + 0.3f;
        }

        public void OnShowNotification(Notification notification, int notValue = 0, int points = 0, Vector2 pos = new Vector2(), float delay = 0f)
        {
            ShowNotification?.Invoke(notification, notValue, points, pos, delay);
        }
    }
}