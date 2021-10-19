using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartPanel : MonoBehaviour
    {
        [SerializeField] private StatsPanel statsPanel;
        [SerializeField] private GameMechanics.GameController gameController;
        [SerializeField] private Text highscoreText;

        private void OnEnable()
        {
            var highscore = PlayerPrefs.GetInt("highscore", -1);
            if (highscore == -1)
            {
                highscore = 0;
                PlayerPrefs.SetInt("highscore", highscore);
            }

            highscoreText.text = "Highscore: " + Convert.ToString(highscore);
        }

        public void OnClickStartGame()
        {
            gameController.gameObject.SetActive(true);
            statsPanel.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
