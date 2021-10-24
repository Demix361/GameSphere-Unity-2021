using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private InputField playerName;

        public event Action<string> ApplyEvent;
        public event Action CancelEvent;
        public event Action ResetProgressEvent;

        public void OnApply()
        {
            ApplyEvent?.Invoke(playerName.text);
        }

        public void OnCancel()
        {
            CancelEvent?.Invoke();
        }

        public void OnResetProgress()
        {
            ResetProgressEvent?.Invoke();
        }

        public void SetPlayerName(string newPlayerName)
        {
            playerName.text = newPlayerName;
        }
    }
}
