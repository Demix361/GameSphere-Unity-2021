using System;
using GameMechanics;
using UnityEngine;

namespace UI
{
    public class ArcadeGamePresenter
    {
        private ArcadeGameModel _gameModel;
        private ArcadeGameWindow _gameWindow;
        private Action _onExit;
        private event Action _onPause;

        public ArcadeGamePresenter(ArcadeGameModel gameModel, ArcadeGameWindow gameWindow, Action onExit, Action onPause)
        {
            _gameModel = gameModel;
            _gameWindow = gameWindow;
            _onExit = onExit;
            _onPause = onPause;
        }

        public void OnOpen()
        {
            OnChangePointsEvent(_gameModel.Points);
            OnChangeTimeEvent(_gameModel.CurTimer);
            _gameModel.ChangePointsEvent += OnChangePointsEvent;
            _gameModel.ChangeTimeEvent += OnChangeTimeEvent;
            _gameModel.EndGameEvent += OnEndGameEvent;
            _gameModel.ShowNotification += OnShowNotification;
            _gameWindow.PauseEvent += OnPause;
        }

        private void OnPause()
        {
            _onPause?.Invoke();
        }

        private void OnShowNotification(Notification notification, int notValue, int points, Vector2 pos, float delay)
        {
            if (notification == Notification.Minus)
            {
                _gameWindow.ShowDefaultNotification("-" + Convert.ToString(points), Color.magenta, pos);
            }
            else if (notification == Notification.Plus)
            {
                _gameWindow.ShowDefaultNotification("+" + Convert.ToString(points), Color.green, pos);
            }
            else if (notification == Notification.MaxCombo)
            {
                _gameWindow.ShowMaxComboNotification(Color.red, pos, Convert.ToString(points));
            }
            else if (notification == Notification.ComboOf)
            {
                _gameWindow.ShowComboOfNotification(Color.yellow, pos, Convert.ToString(notValue), Convert.ToString(points));
            }
            else if (notification == Notification.ComboInRow)
            {
                _gameWindow.ShowComboInRowNotification(new Color(1f, 0.5f, 0f), pos, Convert.ToString(notValue), Convert.ToString(points), delay);
            }
        }

        private void OnEndGameEvent()
        {
            _onExit?.Invoke();
        }

        private void OnChangePointsEvent(int points)
        {
            _gameWindow.SetPoints(Convert.ToString(points));
        }

        private void OnChangeTimeEvent(float time)
        {
            var stringTime = Convert.ToString(Convert.ToInt32(time));

            if (stringTime.Length == 1)
            {
                _gameWindow.SetTime("0:0" + stringTime);
            }
            else
            {
                _gameWindow.SetTime("0:" + stringTime);
            }
        }

        public void OnClose()
        {
            _gameModel.ChangePointsEvent -= OnChangePointsEvent;
            _gameModel.ChangeTimeEvent -= OnChangeTimeEvent;
            _gameModel.EndGameEvent -= OnEndGameEvent;
            _gameModel.ShowNotification -= OnShowNotification;
            _gameWindow.PauseEvent -= OnPause;
        }
    }
}