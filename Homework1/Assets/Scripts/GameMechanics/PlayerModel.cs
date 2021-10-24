using UnityEngine;

namespace GameMechanics
{
    public class PlayerModel
    {
        private string playerName;
        private int highScoreClassic;
        private int highScoreArcade;

        public string PlayerName
        {
            get
            {
                playerName = PlayerPrefs.GetString("playerName", "Player1");
                return playerName;
            }
            set
            {
                playerName = value;
                PlayerPrefs.SetString("playerName", playerName);
            }
        }

        public int HighScoreClassic
        {
            get
            {
                highScoreClassic = PlayerPrefs.GetInt("highScoreClassic", 0);
                return highScoreClassic;
            }
            set
            {
                highScoreClassic = value;
                PlayerPrefs.SetInt("highScoreClassic", highScoreClassic);
            }
        }
        
        public int HighScoreArcade
        {
            get
            {
                highScoreArcade = PlayerPrefs.GetInt("highScoreArcade", 0);
                return highScoreArcade;
            }
            set
            {
                highScoreArcade = value;
                PlayerPrefs.SetInt("highScoreArcade", highScoreArcade);
            }
        }
    }
}