using System;
using UnityEngine;

namespace GameMechanics
{
    public class PlayerModel
    {
        private int highScoreClassic;
        private int highScoreArcade;
        private float musicVolume;
        private float effectsVolume;
        private int activeLanguage;
        private int money;
        private int background;
        private BackgroundInfo[] backgroundInfos;
        private SkinInfo[] skinInfos;

        public event Action<float> ChangeMusicVolume;
        public event Action<float> ChangeEffectsVolume;
        public event Action<int> ChangeLanguage;
        public event Action<int> ChangeBackground;
        

        public enum BackgroundStatus
        {
            Locked,
            Unlocked
        }

        public BackgroundInfo[] BackgroundInfos
        {
            get
            {
                return backgroundInfos;
            }
            set
            {
                backgroundInfos = value;
            }
        }

        public SkinInfo[] SkinInfos
        {
            get
            {
                return skinInfos;
            }
            set
            {
                skinInfos = value;
            }
        }

        public int Background
        {
            get
            {
                background = PlayerPrefs.GetInt("CurrentBackground", 0);
                return background;
            }
            set
            {
                background = value;
                ChangeBackground?.Invoke(background);
                PlayerPrefs.SetInt("CurrentBackground", background);
            }
        }

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

        public int Money
        {
            get
            {
                money = PlayerPrefs.GetInt("money", 0);
                return money;
            }
            set
            {
                money = value;
                PlayerPrefs.SetInt("money", money);
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
                activeLanguage = PlayerPrefs.GetInt("activeLanguage", -1);
                return activeLanguage;
            }
            set
            {
                activeLanguage = value;
                PlayerPrefs.SetInt("activeLanguage", activeLanguage);
                ChangeLanguage?.Invoke(activeLanguage);
            }
        }

        public void AddMoney(int add)
        {
            Money += add;
        }
        
        public BackgroundStatus GetBgStatus(int id)
        {
            var status = PlayerPrefs.GetString("Background" + Convert.ToString(id), "Locked");
            
            if (status == "Unlocked" || id == 0)
            {
                return BackgroundStatus.Unlocked;
            }

            return BackgroundStatus.Locked;
        }

        public void SetBgStatus(int id, BackgroundStatus status)
        {
            if (status == BackgroundStatus.Unlocked)
            {
                PlayerPrefs.SetString("Background" + Convert.ToString(id), "Unlocked");
            }
            else if (status == BackgroundStatus.Locked)
            {
                PlayerPrefs.SetString("Background" + Convert.ToString(id), "Locked");
            }
        }

        public void BuyBackground(int id)
        {
            Money -= BackgroundInfos[id].price;
            SetBgStatus(id, BackgroundStatus.Unlocked);
            Background = id;
        }

        public void LockAllBackgrounds()
        {
            for (int i = 0; i < BackgroundInfos.Length; i++)
            {
                SetBgStatus(i, BackgroundStatus.Locked);
                Background = 0;
            }
        }
        
        public bool GetSkinStatus(int id)
        {
            var status = PlayerPrefs.GetInt("Skin" + Convert.ToString(id), 0);

            if (status == 0)
            {
                return false;
            }

            return true;
        }

        public void SetSkinStatus(int id, bool status)
        {
            if (status)
            {
                PlayerPrefs.SetInt("Skin" + Convert.ToString(id), 1);
            }
            else
            {
                PlayerPrefs.SetInt("Skin" + Convert.ToString(id), 0);
            }
        }

        public void BuySkin(int id)
        {
            Money -= SkinInfos[id].price;
            SetSkinStatus(id, true);
        }

        public void LockAllSkins()
        {
            for (int i = 0; i < SkinInfos.Length; i++)
            {
                SetSkinStatus(i, false);
            }
        }
    }
}