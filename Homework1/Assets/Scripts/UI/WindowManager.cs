using System;
using GameMechanics;
using UnityEngine;

namespace UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private ClassicGameWindow _classicGameWindow;
        
        // использовать actions?
        [SerializeField] private GameController _gameController;

        private StartPresenter _startPresenter;
        private SettingsPresenter _settingsPresenter;
        private ClassicGamePresenter _classicGamePresenter;
        private static PlayerModel _playerModel = new PlayerModel();
        private ClassicGameModel _classicGameModel = new ClassicGameModel(_playerModel);

        public ClassicGameModel ClassicGameModel => _classicGameModel;

        private void Start()
        {
            _settingsPresenter = new SettingsPresenter(_playerModel, _settingsWindow, () =>
            {
                _settingsPresenter.OnClose();
                ShowStartWindow();
            });
            
            _startPresenter = new StartPresenter(_playerModel, _startWindow, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _classicGameWindow.gameObject.SetActive(true);
                _classicGamePresenter.OnOpen();
                _gameController.StartClassic(); //
            }, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _gameController.StartArcade(); //
            }, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _settingsWindow.gameObject.SetActive(true);
                _settingsPresenter.OnOpen();
            }, () =>
            {
                _startPresenter.OnClose();
                Application.Quit();
            });

            _classicGamePresenter = new ClassicGamePresenter(_classicGameModel, _classicGameWindow, () =>
            {
                _classicGamePresenter.OnClose();
                ShowStartWindow();
            });
            
            ShowStartWindow();
        }
        
        private void ShowStartWindow()
        {
            _startWindow.gameObject.SetActive(true);
            _startPresenter.OnOpen();
            _classicGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
        }
    }
}