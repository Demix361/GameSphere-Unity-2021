using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndWindow : MonoBehaviour
    {
        [SerializeField] private Text _pointsText;
        [SerializeField] private Text _highscoreText;
        
        public event Action MainMenuEvent;
        public event Action RestartEvent;
        
        public void OnMainMenu()
        {
            MainMenuEvent?.Invoke();
        }

        public void OnRestart()
        {
            RestartEvent?.Invoke();
        }

        public void SetPoints(string text)
        {
            _pointsText.text = text;
        }

        public void SetHighscore(string text)
        {
            _highscoreText.text = text;
        }
    }
}