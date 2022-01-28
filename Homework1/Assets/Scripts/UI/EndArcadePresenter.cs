using System;
using GameMechanics;

namespace UI
{
    public class EndArcadePresenter
    {
        private PlayerModel _playerModel;
        private ArcadeGameModel _gameModel;
        private EndWindow _endWindow;

        private Action _onMainMenu;
        private Action _onRestart;

        public EndArcadePresenter(PlayerModel playerModel, ArcadeGameModel gameModel, EndWindow endWindow, Action onMainMenu, Action onRestart)
        {
            _playerModel = playerModel;
            _gameModel = gameModel;
            _endWindow = endWindow;
            _onMainMenu = onMainMenu;
            _onRestart = onRestart;
        }

        public void OnOpen()
        {
            OnSetPoints();
            OnSetMoney();
            _endWindow.MainMenuEvent += OnMainMenu;
            _endWindow.RestartEvent += OnRestart;
        }

        private void OnSetPoints()
        {
            var points = _gameModel.Points;
            if (points == _playerModel.HighScoreArcade)
            {
                _endWindow.SetPoints("New Highscore: " + Convert.ToString(points));
                _endWindow.SetHighscore("");
            }
            else
            {
                _endWindow.SetPoints("Points: " + Convert.ToString(points));
                _endWindow.SetHighscore("Highscore: " + Convert.ToString(_playerModel.HighScoreArcade));
            }
        }

        private void OnSetMoney()
        {
            _endWindow.SetMoney(_gameModel.MoneyForGame, _playerModel.Money);
        }

        private void OnMainMenu()
        {
            _onMainMenu?.Invoke();
        }

        private void OnRestart()
        {
            _onRestart?.Invoke();
        }

        public void OnClose()
        {
            _gameModel.OnCloseEndAnimation();
            _endWindow.MainMenuEvent -= OnMainMenu;
            _endWindow.RestartEvent -= OnRestart;
        }
    }
}