using System;
using UnityEngine;

namespace GameMechanics
{
    public class PlayerModel
    {
        private string playerName;
        private int highScoreClassic;
        private int highScoreArcade;
        private float musicVolume;
        private float effectsVolume;
        private int activeLanguage;

        public event Action<float> ChangeMusicVolume;
        public event Action<float> ChangeEffectsVolume;
        public event Action<int> ChangeLanguage;

        public float MusicVolume
        {
            get
            {
                musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
                return musicVolume;
            }
            set
            {
                musicVolume = value;
                PlayerPrefs.SetFloat("MusicVolume", musicVolume);
                ChangeMusicVolume?.Invoke(musicVolume);
            }
        }
        
        public float EffectsVolume
        {
            get
            {
                effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 0f);
                return effectsVolume;
            }
            set
            {
                effectsVolume = value;
                PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
                ChangeEffectsVolume?.Invoke(effectsVolume);
            }
        }
        
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
        
        public int ActiveLanguage
        {
            get
            {
                activeLanguage = PlayerPrefs.GetInt("activeLanguage", 2);
                return activeLanguage;
            }
            set
            {
                activeLanguage = value;
                PlayerPrefs.SetInt("activeLanguage", activeLanguage);
                ChangeLanguage?.Invoke(activeLanguage);
            }
        }
    }
}