using System;
using GameMechanics;

namespace UI
{
    public class LanguagePresenter
    {
        private LanguageWindow _languageWindow;
        private PlayerModel _playerModel;
        private Action _onExit;

        public LanguagePresenter(PlayerModel playerModel, LanguageWindow languageWindow, Action onExit)
        {
            _languageWindow = languageWindow;
            _onExit = onExit;
            _playerModel = playerModel;
        }

        public void OnOpen()
        {
            _languageWindow.SetActiveLanguage(_playerModel.ActiveLanguage);
            _languageWindow.CancelEvent += OnCancel;
            _languageWindow.ChangeLanguageEvent += OnChangeLanguage;
        }

        private void OnCancel()
        {
            _onExit?.Invoke();
        }

        private void OnChangeLanguage(int languageIndex)
        {
            _playerModel.ActiveLanguage = languageIndex;
        }

        public void OnClose()
        {
            _languageWindow.CancelEvent -= OnCancel;
            _languageWindow.ChangeLanguageEvent -= OnChangeLanguage;
        }
    }
}