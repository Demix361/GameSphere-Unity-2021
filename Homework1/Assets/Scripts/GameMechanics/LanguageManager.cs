using UnityEngine;
using System.Collections;
using UnityEngine.Localization.Settings;

namespace GameMechanics
{
    public class LanguageManager : MonoBehaviour
    {
        [SerializeField] private ModelManager _modelManager;
        
        private void Start()
        {
            if (_modelManager.PlayerModel.ActiveLanguage == -1)
            {
                var index = 2;
                
                if (Application.systemLanguage != SystemLanguage.Unknown)
                {
                    var sysLanguage = Application.systemLanguage;
                    
                    if (sysLanguage == SystemLanguage.Arabic)
                    {
                        index = 0;
                    }
                    else if (sysLanguage == SystemLanguage.Chinese)
                    {
                        index = 1;
                    }
                    else if (sysLanguage == SystemLanguage.English)
                    {
                        index = 2;
                    }
                    else if (sysLanguage == SystemLanguage.French)
                    {
                        index = 3;
                    }
                    else if (sysLanguage == SystemLanguage.German)
                    {
                        index = 4;
                    }
                    else if (sysLanguage == SystemLanguage.Italian)
                    {
                        index = 5;
                    }
                    else if (sysLanguage == SystemLanguage.Japanese)
                    {
                        index = 6;
                    }
                    else if (sysLanguage == SystemLanguage.Korean)
                    {
                        index = 7;
                    }
                    else if (sysLanguage == SystemLanguage.Polish)
                    {
                        index = 8;
                    }
                    else if (sysLanguage == SystemLanguage.Portuguese)
                    {
                        index = 9;
                    }
                    else if (sysLanguage == SystemLanguage.Russian)
                    {
                        index = 10;
                    }
                    else if (sysLanguage == SystemLanguage.Spanish)
                    {
                        index = 11;
                    }
                    else if (sysLanguage == SystemLanguage.Thai)
                    {
                        index = 12;
                    }
                    else if (sysLanguage == SystemLanguage.Turkish)
                    {
                        index = 13;
                    }
                    else if (sysLanguage == SystemLanguage.Ukrainian)
                    {
                        index = 14;
                    }
                }

                _modelManager.PlayerModel.ActiveLanguage = index;
            }
            
            ChangeLanguage(_modelManager.PlayerModel.ActiveLanguage);
            _modelManager.PlayerModel.ChangeLanguage += ChangeLanguage;
        }

        private void ChangeLanguage(int languageIndex)
        {
            StartCoroutine(ChangeLanguageCoroutine(languageIndex));
        }
        
        public IEnumerator ChangeLanguageCoroutine(int languageIndex) 
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
        }
    }
}