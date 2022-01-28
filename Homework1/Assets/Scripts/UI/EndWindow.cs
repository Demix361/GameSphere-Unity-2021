using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndWindow : MonoBehaviour
    {
        [SerializeField] private Text _pointsText;
        [SerializeField] private Text _highscoreText;
        [SerializeField] private Text _moneyText;
        [SerializeField] private Text _addedMoneyText;
        
        public event Action MainMenuEvent;
        public event Action RestartEvent;

        private Coroutine moneyCoroutine;
        
        public void OnMainMenu()
        {
            if (moneyCoroutine != null)
            {
                StopCoroutine(moneyCoroutine);
            }
            MainMenuEvent?.Invoke();
        }

        public void OnRestart()
        {
            if (moneyCoroutine != null)
            {
                StopCoroutine(moneyCoroutine);
            }
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

        public void SetMoney(int added, int final)
        {
            StartCoroutine(SetMoneyCoroutine(added, final));
        }

        private IEnumerator SetMoneyCoroutine(int added, int final)
        {
            var a = final - added;
            DOTween.To(()=> a, x=> a = x, final, 3);
            _addedMoneyText.text = "+" + Convert.ToString(added);

            while (a != final)
            {
                yield return new WaitForSeconds(0.1f);
                _moneyText.text = Convert.ToString(a);
            }
        }
    }
}