using UnityEngine;
using System;

namespace UI
{
    public class LanguageWindow : MonoBehaviour
    {
        public event Action CancelEvent;
        public event Action<int> ChangeLanguageEvent;

        public void OnCancel()
        {
            CancelEvent?.Invoke();
        }

        public void OnLanguageChange(int num)
        {
            ChangeLanguageEvent?.Invoke(num);
        }
        
        public void SetActiveLanguage(int languageIndex)
        {
            
        }
    }
}

