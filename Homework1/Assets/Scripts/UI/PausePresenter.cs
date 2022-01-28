using System;

namespace UI
{
    public class PausePresenter
    {
        private PauseWindow _pauseWindow;

        public event Action _resume;
        public event Action _menu;
        public event Action _restart;

        public PausePresenter(PauseWindow pauseWindow, Action resume, Action menu, Action restart)
        {
            _pauseWindow = pauseWindow;
            _resume = resume;
            _menu = menu;
            _restart = restart;
        }

        public void OnOpen()
        {
            _pauseWindow.CancelEvent += OnCancel;
            _pauseWindow.MenuEvent += OnMenu;
            _pauseWindow.RestartEvent += OnRestart;
        }

        private void OnCancel()
        {
            _resume?.Invoke();
        }

        private void OnMenu()
        {
            _menu?.Invoke();
        }

        private void OnRestart()
        {
            _restart?.Invoke();
        }
        
        public void OnClose()
        {
            _pauseWindow.CancelEvent -= OnCancel;
            _pauseWindow.MenuEvent -= OnMenu;
            _pauseWindow.RestartEvent -= OnRestart;
        }
    }
}