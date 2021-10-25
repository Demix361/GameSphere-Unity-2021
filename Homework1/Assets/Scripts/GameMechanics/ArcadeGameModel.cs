using System;

namespace GameMechanics
{
    public class ArcadeGameModel
    {
        private PlayerModel _playerModel;
        private const float Timer = 30;
        
        public ArcadeGameModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public float CurTimer { get; set; } = Timer;
        public float SpawnInterval { get; set; } = 1;
        public int Points { get; set; } = 0;

        public event Action EndGameEvent;
        public event Action<int> ChangePointsEvent;
        public event Action<float> ChangeTimeEvent;

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

            CurTimer = Timer;
            Points = 0;

            EndGameEvent?.Invoke();
        }
    }
}