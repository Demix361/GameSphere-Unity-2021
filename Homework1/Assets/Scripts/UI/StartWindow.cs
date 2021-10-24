using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartWindow : MonoBehaviour
    {
        [SerializeField] private Text highScoreClassicText;
        [SerializeField] private Text highScoreArcadeText;

        public event Action StartClassicEvent;
        public event Action StartArcadeEvent;
        public event Action SettingsEvent;
        public event Action QuitEvent;

        public void OnStartClassic()
        {
            StartClassicEvent?.Invoke();
        }
        
        public void OnStartArcade()
        {
            StartArcadeEvent?.Invoke();
        }

        public void OnSettings()
        {
            SettingsEvent?.Invoke();
        }

        public void OnQuit()
        {
            QuitEvent?.Invoke();
        }

        public void SetHighScores(string newHighScoreC, string newHighScoreA)
        {
            highScoreClassicText.text = newHighScoreC;
            highScoreArcadeText.text = newHighScoreA;
        }
    }
}
