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
        [SerializeField] private EndWindow _endWindow;
        [SerializeField] private LanguageWindow _languageWindow;
        [SerializeField] private PauseWindow _pauseWindow;
        
        [SerializeField] private ModelManager _modelManager;
        
        private StartPresenter _startPresenter;
        private SettingsPresenter _settingsPresenter;
        private ClassicGamePresenter _classicGamePresenter;
        private ArcadeGamePresenter _arcadeGamePresenter;
        private EndClassicPresenter _endClassicPresenter;
        private EndArcadePresenter _endArcadePresenter;
        private LanguagePresenter _languagePresenter;
        private PausePresenter _arcadePausePresenter;
        private PausePresenter _classicPausePresenter;
        
        private void Start()
        {
            _settingsPresenter = new SettingsPresenter(_modelManager.PlayerModel, _settingsWindow, () =>
            {
                _settingsPresenter.OnClose();
                ShowStartWindow();
            }, () =>
            {
                _settingsWindow.gameObject.SetActive(false);
                _settingsPresenter.OnClose();
                
                _languageWindow.gameObject.SetActive(true);
                _languagePresenter.OnOpen();
            });

            _languagePresenter = new LanguagePresenter(_modelManager.PlayerModel, _languageWindow, () =>
            {
                _languageWindow.gameObject.SetActive(false);
                _languagePresenter.OnClose();
                
                _settingsWindow.gameObject.SetActive(true);
                _settingsPresenter.OnOpen();
            });
            
            _startPresenter = new StartPresenter(_modelManager.PlayerModel, _modelManager.MainMenuModel, _startWindow, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _classicGameWindow.gameObject.SetActive(true);
                _modelManager.ClassicGameModel.OnStartGame();
                _classicGamePresenter.OnOpen();
            }, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _arcadeGameWindow.gameObject.SetActive(true);
                _modelManager.ArcadeGameModel.OnStartGame();
                _arcadeGamePresenter.OnOpen();
            }, () =>
            {
                _startWindow.gameObject.SetActive(false);
                _startPresenter.OnClose();
                
                _settingsWindow.gameObject.SetActive(true);
                _settingsPresenter.OnOpen();
            }, () =>
            {
                _startPresenter.OnClose();
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                     Application.Quit();
                #endif
            });
            
            _classicPausePresenter = new PausePresenter(_pauseWindow, () =>
            {
                _modelManager.ClassicGameModel.OnUnpauseGame();
                _classicPausePresenter.OnClose();
                _pauseWindow.gameObject.SetActive(false);
            }, () =>
            {
                _classicPausePresenter.OnClose();
                _pauseWindow.gameObject.SetActive(false);
                _classicGamePresenter.OnClose();
                _classicGameWindow.gameObject.SetActive(false);
                
                _modelManager.ClassicGameModel.OnStopGame();
                
                ShowStartWindow();
            }, () =>
            {
                _classicPausePresenter.OnClose();
                _pauseWindow.gameObject.SetActive(false);
                _modelManager.ClassicGameModel.OnStopGame();
                _modelManager.ClassicGameModel.OnStartGame();
            });

            _arcadePausePresenter = new PausePresenter(_pauseWindow, () =>
            {
                _modelManager.ArcadeGameModel.OnUnpauseGame();
                _arcadePausePresenter.OnClose();
                _pauseWindow.gameObject.SetActive(false);
            }, () =>
            {
                _arcadePausePresenter.OnClose();
                _pauseWindow.gameObject.SetActive(false);
                _arcadeGamePresenter.OnClose();
                _arcadeGameWindow.gameObject.SetActive(false);
                
                _modelManager.ArcadeGameModel.OnStopGame();
                
                ShowStartWindow();
            }, () =>
            {
                _arcadePausePresenter.OnClose();
                _pauseWindow.gameObject.SetActive(false);
                _modelManager.ArcadeGameModel.OnStopGame();
                _modelManager.ArcadeGameModel.OnStartGame();
            });

            _classicGamePresenter = new ClassicGamePresenter(_modelManager.ClassicGameModel, _classicGameWindow, () =>
            {
                _classicGamePresenter.OnClose();
                _classicGameWindow.gameObject.SetActive(false);
                
                _endWindow.gameObject.SetActive(true);
                _endClassicPresenter.OnOpen();
            }, () =>
            {
                _modelManager.ClassicGameModel.OnPauseGame();
                _classicPausePresenter.OnOpen();
                _pauseWindow.gameObject.SetActive(true);
            });

            _arcadeGamePresenter = new ArcadeGamePresenter(_modelManager.ArcadeGameModel, _arcadeGameWindow, () =>
            {
                _arcadeGamePresenter.OnClose();
                _arcadeGameWindow.gameObject.SetActive(false);
                
                _endWindow.gameObject.SetActive(true);
                _endArcadePresenter.OnOpen();
            }, () =>
            {
                _modelManager.ArcadeGameModel.OnPauseGame();
                _arcadePausePresenter.OnOpen();
                _pauseWindow.gameObject.SetActive(true);
            });

            _endClassicPresenter = new EndClassicPresenter(_modelManager.PlayerModel, _modelManager.ClassicGameModel, _endWindow, 
                () => 
                {
                    _endClassicPresenter.OnClose();
                    ShowStartWindow();
                }, () =>
                {
                    _endWindow.gameObject.SetActive(false);
                    _modelManager.ClassicGameModel.OnStartGame();
                    _classicGameWindow.gameObject.SetActive(true);
                    _classicGamePresenter.OnOpen();
                    _endClassicPresenter.OnClose();
                });
            
            _endArcadePresenter = new EndArcadePresenter(_modelManager.PlayerModel, _modelManager.ArcadeGameModel, _endWindow, 
                () => 
                {
                    _endArcadePresenter.OnClose();
                    ShowStartWindow();
                }, () =>
                {
                    _endWindow.gameObject.SetActive(false);
                    _modelManager.ArcadeGameModel.OnStartGame();
                    _arcadeGameWindow.gameObject.SetActive(true);
                    _arcadeGamePresenter.OnOpen();
                    _endArcadePresenter.OnClose();
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
            _endWindow.gameObject.SetActive(false);
            _languageWindow.gameObject.SetActive(false);
            _pauseWindow.gameObject.SetActive(false);
        }

        private void ShowSettingsWindow()
        {
            _settingsWindow.gameObject.SetActive(true);
            _settingsPresenter.OnOpen();
            _startWindow.gameObject.SetActive(false);
            _classicGameWindow.gameObject.SetActive(false);
            _arcadeGameWindow.gameObject.SetActive(false);
            _endWindow.gameObject.SetActive(false);
            _languageWindow.gameObject.SetActive(false);
        }
    }
}