using System;

namespace UI
{
    public class ArcadeGamePresenter
    {
        private GameMechanics.ArcadeGameModel _gameModel;
        private ArcadeGameWindow _gameWindow;
        private Action _onExit;

        public ArcadeGamePresenter(GameMechanics.ArcadeGameModel gameModel, ArcadeGameWindow gameWindow, Action onExit)
        {
            _gameModel = gameModel;
            _gameWindow = gameWindow;
            _onExit = onExit;
        }

        public void OnOpen()
        {
            OnChangePointsEvent(_gameModel.Points);
            OnChangeTimeEvent(_gameModel.CurTimer);
            _gameModel.ChangePointsEvent += OnChangePointsEvent;
            _gameModel.ChangeTimeEvent += OnChangeTimeEvent;
            _gameModel.EndGameEvent += OnEndGameEvent;
        }

        private void OnEndGameEvent()
        {
            _onExit?.Invoke();
        }

        private void OnChangePointsEvent(int points)
        {
            _gameWindow.SetPoints(Convert.ToString(points));
        }

        private void OnChangeTimeEvent(float time)
        {
            var stringTime = Convert.ToString(Convert.ToInt32(time));

            if (stringTime.Length == 1)
            {
                _gameWindow.SetTime("0:0" + stringTime);
            }
            else
            {
                _gameWindow.SetTime("0:" + stringTime);
            }
        }

        public void OnClose()
        {
            _gameModel.ChangePointsEvent -= OnChangePointsEvent;
            _gameModel.ChangeTimeEvent -= OnChangeTimeEvent;
            _gameModel.EndGameEvent -= OnEndGameEvent;
        }
    }
}