using System;
using UnityEngine;

namespace UI
{
    public class NotificationPresenter
    {
        private GameMechanics.ArcadeGameModel _gameModel;
        private NotificationWindow _notificationWindow;
        private Action _onExit;

        public void OnOpen()
        {
            
        }
        
        private void OnEndGameEvent()
        {
            _onExit?.Invoke();
        }

        private void OnShowNotification(string text, Color color, Vector2 pos)
        {
            _notificationWindow.ShowNotification(text, color, pos);
        }

        public void OnClose()
        {
            
        }
    }
}