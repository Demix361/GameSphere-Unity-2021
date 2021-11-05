using System;

namespace UI
{
    public class ClassicGamePresenter
    {
        private GameMechanics.ClassicGameModel _gameModel;
        private ClassicGameWindow _gameWindow;
        private Action _onExit;

        public ClassicGamePresenter(GameMechanics.ClassicGameModel gameModel, ClassicGameWindow gameWindow, Action onExit)
        {
            _gameModel = gameModel;
            _gameWindow = gameWindow;
            _onExit = onExit;
        }

        public void OnOpen()
        {
            OnChangePointsEvent(_gameModel.Points);
            OnChangeLivesEvent(_gameModel.CurLives);
            _gameModel.ChangePointsEvent += OnChangePointsEvent;
            _gameModel.ChangeLivesEvent += OnChangeLivesEvent;
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

        private void OnChangeLivesEvent(int lives)
        {
            _gameWindow.SetCrosses(_gameModel.Lives - lives);
        }

        public void OnClose()
        {
            _gameModel.ChangePointsEvent -= OnChangePointsEvent;
            _gameModel.ChangeLivesEvent -= OnChangeLivesEvent;
            _gameModel.EndGameEvent -= OnEndGameEvent;
        }
    }
}