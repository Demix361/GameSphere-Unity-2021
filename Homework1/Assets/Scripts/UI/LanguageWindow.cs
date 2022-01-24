using UnityEngine;
using System;
using UnityEngine.UI;

namespace UI
{
    public class LanguageWindow : MonoBehaviour
    {
        public event Action CancelEvent;
        public event Action<int> ChangeLanguageEvent;

        [SerializeField] private Button[] _buttons;
        
        private Color _activeColor = Color.green;
        private Color _defaultColor = Color.white;

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
            for (int i = 0; i < _buttons.Length; i++)
            {
                if (i == languageIndex)
                {
                    _buttons[i].image.color = _activeColor;
                }
                else
                {
                    _buttons[i].image.color = _defaultColor;
                }
            }
        }
    }
}

