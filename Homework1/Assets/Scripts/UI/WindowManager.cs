using GameMechanics;
using UnityEngine;

namespace UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private StartWindow _startWindow;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private ClassicGameWindow _classicGameWindow;
        [SerializeField] private ArcadeGameWindow _arcadeGameWindow;
        
        // использовать actions?
        [SerializeField] private GameController _gameController;

        private StartPresenter _startPresenter;
        private SettingsPresenter _settingsPresenter;
        private ClassicGamePresenter _classicGamePresenter;
        private static PlayerModel _playerModel = new PlayerModel();
        private ClassicGameModel _classicGameModel = new ClassicGameModel(_playerModel);
        private ArcadeGamePresenter _arcadeGamePresenter;
        private ArcadeGameModel _arcadeGameModel = new ArcadeGameModel(_playerModel);

        public ClassicGameModel ClassicGameModel => _classicGameModel;
        public ArcadeGameModel ArcadeGameModel => _arcadeGameModel;

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
                
                _arcadeGameWindow.gameObject.SetActive(true);
                _arcadeGamePresenter.OnOpen();
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

            _arcadeGamePresenter = new ArcadeGamePresenter(_arcadeGameModel, _arcadeGameWindow, () =>
            {
                _arcadeGamePresenter.OnClose();
                ShowStartWindow();
            });
            
            ShowStartWindow();
        }
        
        private void ShowStartWindow()
        {
            _startWindow.gameObject.SetActive(true);
            _startPresenter.OnOpen();
            _classicGameWindow.gameObject.SetActive(false);
            _arcadeGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
        }
    }
}