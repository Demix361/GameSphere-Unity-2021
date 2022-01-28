using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private InputField playerName;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider effectsSlider;
        
        public event Action<string> ApplyEvent;
        public event Action CancelEvent;
        public event Action ResetProgressEvent;
        public event Action<float> ChangeMusicVolume;
        public event Action<float> ChangeEffectsVolume;
        public event Action LanguageEvent;

        public void OnLanguage()
        {
            LanguageEvent?.Invoke();
        }
        
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

        public void SetMusicSlider(float value)
        {
            musicSlider.value = value;
        }

        public void SetEffectsSlider(float value)
        {
            effectsSlider.value = value;
        }

        public void OnMusicSliderChanged(float value)
        {
            ChangeMusicVolume?.Invoke(value);
        }
        
        public void OnEffectsSliderChanged(float value)
        {
            ChangeEffectsVolume?.Invoke(value);
        }

        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    OnCancel();
                }
            }
        }
    }
}
