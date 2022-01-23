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