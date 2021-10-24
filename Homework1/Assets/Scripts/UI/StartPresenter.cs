using System;
using GameMechanics;

namespace UI
{
    public class StartPresenter
    {
        private PlayerModel _playerModel;
        private StartWindow _startWindow;
        private Action _onStartClassic;
        private Action _onStartArcade;
        private Action _onSettings;
        private Action _onExit;

        public StartPresenter(PlayerModel playerModel, StartWindow startWindow, 
            Action onStartClassic, Action onStartArcade, Action onSettings, Action onExit)
        {
            _onExit = onExit;
            _onSettings = onSettings;
            _onStartArcade = onStartArcade;
            _onStartClassic = onStartClassic;
            _playerModel = playerModel;
            _startWindow = startWindow;
        }

        public void OnOpen()
        {
            var scoreC = "Highscore: " + Convert.ToString(_playerModel.HighScoreClassic);
            var scoreA = "Highscore: " + Convert.ToString(_playerModel.HighScoreArcade);
            _startWindow.SetHighScores(scoreC, scoreA);
            _startWindow.StartClassicEvent += OnStartWindowOnStartClassicEvent;
            _startWindow.StartArcadeEvent += OnStartWindowOnStartArcadeEvent;
            _startWindow.SettingsEvent += OnStartWindowOnSettingsEvent;
            _startWindow.QuitEvent += OnStartWindowOnQuitEvent;
        }

        private void OnStartWindowOnQuitEvent()
        {
            _onExit?.Invoke();
        }

        private void OnStartWindowOnSettingsEvent()
        {
            _onSettings?.Invoke();
        }

        private void OnStartWindowOnStartArcadeEvent()
        {
            _onStartArcade?.Invoke();
        }

        private void OnStartWindowOnStartClassicEvent()
        {
            _onStartClassic?.Invoke();
        }

        public void OnClose()
        {
            _startWindow.StartClassicEvent -= OnStartWindowOnStartClassicEvent;
            _startWindow.StartArcadeEvent -= OnStartWindowOnStartArcadeEvent;
            _startWindow.SettingsEvent -= OnStartWindowOnSettingsEvent;
            _startWindow.QuitEvent -= OnStartWindowOnQuitEvent;
        }
    }
}