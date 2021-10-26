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
        
        [SerializeField] private ModelManager _modelManager;
        
        private StartPresenter _startPresenter;
        private SettingsPresenter _settingsPresenter;
        private ClassicGamePresenter _classicGamePresenter;
        private ArcadeGamePresenter _arcadeGamePresenter;

        private void Start()
        {
            _settingsPresenter = new SettingsPresenter(_modelManager.PlayerModel, _settingsWindow, () =>
            {
                _settingsPresenter.OnClose();
                ShowStartWindow();
            });
            
            _startPresenter = new StartPresenter(_modelManager.PlayerModel, _startWindow, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _classicGameWindow.gameObject.SetActive(true);
                _classicGamePresenter.OnOpen();
                _modelManager.ClassicGameModel.OnStartGame();
            }, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _arcadeGameWindow.gameObject.SetActive(true);
                _arcadeGamePresenter.OnOpen();
                _modelManager.ArcadeGameModel.OnStartGame();
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

            _classicGamePresenter = new ClassicGamePresenter(_modelManager.ClassicGameModel, _classicGameWindow, () =>
            {
                _classicGamePresenter.OnClose();
                ShowStartWindow();
            });

            _arcadeGamePresenter = new ArcadeGamePresenter(_modelManager.ArcadeGameModel, _arcadeGameWindow, () =>
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