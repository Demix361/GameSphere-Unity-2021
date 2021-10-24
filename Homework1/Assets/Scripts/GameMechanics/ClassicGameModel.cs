using System;
using UnityEngine;

namespace GameMechanics
{
    public class ClassicGameModel
    {
        private static int _lives = 3;
        private float _spawnInterval = 2;
        private int _restoreLivePoints = 100;
        private int _points = 0;
        private int _curLives = _lives;
        private PlayerModel _playerModel;

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
        public int RestoreLivePoints => _restoreLivePoints;

        public int Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }
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

        public void OnChangePoints(int newValue)
        {
            ChangePointsEvent?.Invoke(newValue);
            
            Points = newValue;
            if (Points % RestoreLivePoints == 0 && CurLives < Lives)
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

            CurLives = Lives;
            Points = 0;

            EndGameEvent?.Invoke();
        }
    }
}