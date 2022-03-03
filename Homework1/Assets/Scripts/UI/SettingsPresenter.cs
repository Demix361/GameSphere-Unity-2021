using System;
using GameMechanics;

namespace UI
{
    public class SettingsPresenter
    {
        private PlayerModel _playerModel;
        private SettingsWindow _settingsWindow;
        private Action _onExit;
        private Action _onLanguage;

        public SettingsPresenter(PlayerModel playerModel, SettingsWindow settingsWindow, Action onExit, Action onLanguage)
        {
            _playerModel = playerModel;
            _settingsWindow = settingsWindow;
            _onExit = onExit;
            _onLanguage = onLanguage;
        }

        public void OnOpen()
        {
            _settingsWindow.SetPlayerName(_playerModel.PlayerName);
            _settingsWindow.SetMusicSlider(_playerModel.MusicVolume);
            _settingsWindow.SetEffectsSlider(_playerModel.EffectsVolume);

            _settingsWindow.LanguageEvent += OnOpenLanguage;
            _settingsWindow.ApplyEvent += OnApply;
            _settingsWindow.CancelEvent += OnCancel;
            _settingsWindow.ResetProgressEvent += OnResetProgress;
            _settingsWindow.ChangeMusicVolume += OnChangeMusicVolume;
            _settingsWindow.ChangeEffectsVolume += OnChangeEffectsVolume;

            _settingsWindow.PlusMoneyEvent += OnPlusMoney;
            _settingsWindow.ZeroMoneyEvent += OnZeroMoney;
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
            _playerModel.Money = 0;
            _playerModel.LockAllBackgrounds();
            _onExit?.Invoke();
        }

        private void OnChangeMusicVolume(float value)
        {
            _playerModel.MusicVolume = value;
        }

        private void OnChangeEffectsVolume(float value)
        {
            _playerModel.EffectsVolume = value;
        }

        private void OnOpenLanguage()
        {
            _onLanguage?.Invoke();
        }

        private void OnPlusMoney()
        {
            _playerModel.AddMoney(100);
        }

        private void OnZeroMoney()
        {
            _playerModel.Money = 0;
        }

        public void OnClose()
        {
            _settingsWindow.ApplyEvent -= OnApply;
            _settingsWindow.CancelEvent -= OnCancel;
            _settingsWindow.ResetProgressEvent -= OnResetProgress;
            _settingsWindow.LanguageEvent -= OnOpenLanguage;
            _settingsWindow.ChangeMusicVolume -= OnChangeMusicVolume;
            _settingsWindow.ChangeEffectsVolume -= OnChangeEffectsVolume;
            
            _settingsWindow.PlusMoneyEvent -= OnPlusMoney;
            _settingsWindow.ZeroMoneyEvent -= OnZeroMoney;
        }
    }
}