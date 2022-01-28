using UnityEngine;
using System;

namespace UI
{
    public class PauseWindow : MonoBehaviour
    {
        public event Action CancelEvent;
        public event Action MenuEvent;
        public event Action RestartEvent;
        
        public void OnCancel()
        {
            CancelEvent?.Invoke();
        }

        public void OnRestart()
        {
            RestartEvent?.Invoke();
        }

        public void OnMenu()
        {
            MenuEvent?.Invoke();
        }
    }
}