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
            _endWindow.MainMenuEvent += OnMainMenu;
            _endWindow.RestartEvent += OnRestart;
            _endWindow.ShowHighscore(false);
            OnSetPoints();
            OnSetMoney();
        }

        private void OnSetPoints()
        {
            var points = _gameModel.Points;
            _endWindow.SetPoints(Convert.ToString(points));
            
            if (points == _playerModel.HighScoreClassic)
            {
                _endWindow.ShowHighscore(true);
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