using System;
using GameMechanics;
using UnityEngine;

namespace UI
{
    public class StartPresenter
    {
        private PlayerModel _playerModel;
        private MainMenuModel _mainMenuModel;
        private StartWindow _startWindow;
        private Action _onStartClassic;
        private Action _onStartArcade;
        private Action _onSettings;
        private Action _onShop;
        private Action _onLanguage;
        private Action _onExit;

        public StartPresenter(PlayerModel playerModel, MainMenuModel mainMenuModel, StartWindow startWindow, 
            Action onStartClassic, Action onStartArcade, Action onSettings, Action onShop, Action onLanguage, Action onExit)
        {
            _onExit = onExit;
            _onSettings = onSettings;
            _onStartArcade = onStartArcade;
            _onStartClassic = onStartClassic;
            _playerModel = playerModel;
            _startWindow = startWindow;
            _mainMenuModel = mainMenuModel;
            _onShop = onShop;
            _onLanguage = onLanguage;
        }

        public void OnOpen()
        {
            var scoreC = Convert.ToString(_playerModel.HighScoreClassic);
            var scoreA = Convert.ToString(_playerModel.HighScoreArcade);
            var money = Convert.ToString(_playerModel.Money);
            _startWindow.SetHighScores(scoreC, scoreA);
            _startWindow.SetMoney(money);
            _mainMenuModel.StartSpawn();
            
            _startWindow.StartClassicEvent += OnStartWindowOnStartClassicEvent;
            _startWindow.StartArcadeEvent += OnStartWindowOnStartArcadeEvent;
            _startWindow.SettingsEvent += OnStartWindowOnSettingsEvent;
            _startWindow.ShopEvent += OnShopEvent;
            _startWindow.LanguageEvent += OnLanguageEvent;
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
            _mainMenuModel.StopSpawn();
        }

        private void OnStartWindowOnStartClassicEvent()
        {
            _onStartClassic?.Invoke();
            _mainMenuModel.StopSpawn();
        }

        private void OnShopEvent()
        {
            _onShop?.Invoke();
        }

        private void OnLanguageEvent()
        {
            _onLanguage?.Invoke();
        }

        public void OnClose()
        {
            _startWindow.StartClassicEvent -= OnStartWindowOnStartClassicEvent;
            _startWindow.StartArcadeEvent -= OnStartWindowOnStartArcadeEvent;
            _startWindow.SettingsEvent -= OnStartWindowOnSettingsEvent;
            _startWindow.QuitEvent -= OnStartWindowOnQuitEvent;
            _startWindow.ShopEvent -= OnShopEvent;
            _startWindow.LanguageEvent -= OnLanguageEvent;
        }
    }
}