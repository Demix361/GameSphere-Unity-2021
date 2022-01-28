using System;
using GameMechanics;

namespace UI
{
    public class EndClassicPresenter
    {
        private PlayerModel _playerModel;
        private ClassicGameModel _gameModel;
        private EndWindow _endWindow;

        private Action _onMainMenu;
        private Action _onRestart;

        public EndClassicPresenter(PlayerModel playerModel, ClassicGameModel gameModel, EndWindow endWindow, Action onMainMenu, Action onRestart)
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
            if (points == _playerModel.HighScoreClassic)
            {
                _endWindow.SetPoints("New Highscore: " + Convert.ToString(points));
                _endWindow.SetHighscore("");
            }
            else
            {
                _endWindow.SetPoints("Points: " + Convert.ToString(points));
                _endWindow.SetHighscore("Highscore: " + Convert.ToString(_playerModel.HighScoreClassic));
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