using System;
using GameMechanics;

namespace UI
{
    public class SettingsPresenter
    {
        private PlayerModel _playerModel;
        private SettingsWindow _settingsWindow;
        private Action _onExit;

        public SettingsPresenter(PlayerModel playerModel, SettingsWindow settingsWindow, Action onExit)
        {
            _playerModel = playerModel;
            _settingsWindow = settingsWindow;
            _onExit = onExit;
        }

        public void OnOpen()
        {
            _settingsWindow.SetPlayerName(_playerModel.PlayerName);
            _settingsWindow.ApplyEvent += OnApply;
            _settingsWindow.CancelEvent += OnCancel;
            _settingsWindow.ResetProgressEvent += OnResetProgress;
        }

        private void OnCancel()
        {
            _onExit?.Invoke();
        }

        private void OnApply(string newPlayerName)
        {
            _playerModel.PlayerName = newPlayerName;
            _onExit?.Invoke();
        }

        private void OnResetProgress()
        {
            _playerModel.HighScoreClassic = 0;
            _playerModel.HighScoreArcade = 0;
            _onExit?.Invoke();
        }

        public void OnClose()
        {
            _settingsWindow.ApplyEvent -= OnApply;
            _settingsWindow.CancelEvent -= OnCancel;
            _settingsWindow.ResetProgressEvent -= OnResetProgress;
        }
    }
}