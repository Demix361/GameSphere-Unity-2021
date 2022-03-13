using System;
using System.Collections;
using System.Collections.Generic;
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
        private AmogusInfo[] amogusInfos;

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
        
        public AmogusInfo[] AmogusInfos
        {
            get
            {
                return amogusInfos;
            }
            set
            {
                amogusInfos = value;
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

        public void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
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
        
        // SKINS
        
        public class SkinPriceComparer : IComparer<SkinInfo>
        {
            public int Compare(SkinInfo x, SkinInfo y)
            {
                return (new CaseInsensitiveComparer()).Compare(x.price, y.price);
            }
        }
        
        public class ReverseSkinPriceComparer : IComparer<SkinInfo>
        {
            public int Compare(SkinInfo x, SkinInfo y)
            {
                return (new CaseInsensitiveComparer()).Compare(y.price, x.price);
            }
        }
        
        public class ReverseSkinRarityComparer : IComparer<SkinInfo>
        {
            public int Compare(SkinInfo x, SkinInfo y)
            {
                return (new CaseInsensitiveComparer()).Compare(y.rarity, x.rarity);
            }
        }

        public AmogusInfo GetAmogusInfoByColor(string colorId)
        {
            foreach (var info in amogusInfos)
            {
                if (info.colorName == colorId)
                {
                    return info;
                }
            }

            return null;
        }
        
        public void SetSelectedSkin(string colorId, int skinId)
        {
            PlayerPrefs.SetInt("SelectedSkin" + colorId, skinId);
        }
        
        public int GetSkinIdByColor(string colorId)
        {
            var skinId = PlayerPrefs.GetInt("SelectedSkin" + colorId, -1);

            if (skinId == -1)
            {
                skinId = GetAmogusInfoByColor(colorId).defaultSkin.id;
                SetSelectedSkin(colorId, skinId);
            }

            return skinId;
        }

        public SkinInfo GetSkinInfoByColor(string colorId)
        {
            var skinId = GetSkinIdByColor(colorId);
            foreach (var skin in skinInfos)
            {
                if (skin.id == skinId)
                {
                    return skin;
                }
            }

            return null;
        }
        
        public Sprite GetSkinSpriteByColor(string colorId)
        {
            return GetSkinInfoByColor(colorId).skin;
        }
        
        // Возвращает цвет (если выбран) для данного скина
        public string GetColorBySkin(int skinId)
        {
            foreach (var info in amogusInfos)
            {
                if (PlayerPrefs.GetInt("SelectedSkin" + info.colorName, -1) == skinId)
                {
                    return info.colorName;
                }
            }
            
            return null;
        }

        public Color GetBorderColor(SkinInfo.RarityClass rarity)
        {
            var color = Color.white;
            
            if (rarity == SkinInfo.RarityClass.Rare)
            {
                color = new Color(0f, 0.5f, 1f, 1f);
            }
            else if (rarity == SkinInfo.RarityClass.Epic)
            {
                color = new Color(0.6f, 0f, 1f, 1f);
            }
            else if (rarity == SkinInfo.RarityClass.Legendary)
            {
                color = new Color(1f, 0.4f, 0f, 1f);
            }

            return color;
        }
    }
}