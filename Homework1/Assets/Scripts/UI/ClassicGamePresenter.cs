using System;

namespace UI
{
    public class ClassicGamePresenter
    {
        private GameMechanics.ClassicGameModel _gameModel;
        private ClassicGameWindow _gameWindow;
        private event Action _onExit;
        private event Action _onPause;

        public ClassicGamePresenter(GameMechanics.ClassicGameModel gameModel, ClassicGameWindow gameWindow, Action onExit, Action onPause)
        {
            _gameModel = gameModel;
            _gameWindow = gameWindow;
            _onExit = onExit;
            _onPause = onPause;
        }

        public void OnOpen()
        {
            OnChangePointsEvent(_gameModel.Points);
            OnChangeLivesEvent(_gameModel.CurLives);
            _gameModel.ChangePointsEvent += OnChangePointsEvent;
            _gameModel.ChangeLivesEvent += OnChangeLivesEvent;
            _gameModel.EndGameEvent += OnEndGameEvent;
            _gameWindow.PauseEvent += OnPause;
        }
        
        private void OnPause()
        {
            _onPause?.Invoke();
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
            _gameWindow.PauseEvent -= OnPause;
        }
    }
}