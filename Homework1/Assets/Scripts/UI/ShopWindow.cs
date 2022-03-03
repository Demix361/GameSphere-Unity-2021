using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameMechanics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopWindow : MonoBehaviour
    {
        public event Action CloseEvent;
        public event Action<int> BgButtonEvent;
        private List<BackgroundPanel> _backgroundPanels = new List<BackgroundPanel>();
        [SerializeField] private Color _priceColor;
        [SerializeField] private Text _moneyText;
        [SerializeField] private GameObject _notEnoughMoneyWindow;
        [SerializeField] private GameObject _confirmWindow;
        [SerializeField] private GameObject _bgPanelPrefab;
        [SerializeField] private RectTransform _bgPanelsParent;

        private Coroutine _moneyCoroutine;

        public void SetMainPanel(int amount)
        {
            _bgPanelsParent.sizeDelta = new Vector2(600 * amount + 60 * (amount - 1), _bgPanelsParent.sizeDelta.y);
            _bgPanelsParent.anchoredPosition =
                new Vector2(_bgPanelsParent.sizeDelta.x / 2, _bgPanelsParent.anchoredPosition.y);
        }
        
        public void OnBgPress(int id)
        {
            BgButtonEvent?.Invoke(id);
        }

        public void SpawnBgPanel(BackgroundInfo bgInfo)
        {
            var panel = Instantiate(_bgPanelPrefab, _bgPanelsParent);
            panel.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnBgPress(bgInfo.id); });
            var bgPanel = panel.GetComponent<BackgroundPanel>();
            _backgroundPanels.Add(bgPanel);
            
            //bgPanel.SetNameText(Convert.ToString(bgInfo.id));
            bgPanel.SetPreviewImage(bgInfo.preview);
        }

        public void ShowNotEnoughMoneyWindow(bool state)
        {
            _notEnoughMoneyWindow.SetActive(state);
        }

        public void ShowConfirmWindow(bool state)
        {
            _confirmWindow.SetActive(state);
        }
        
        public void SetButtonLocked(int id, int price)
        {
            _backgroundPanels[id].SetButtonText(Convert.ToString(price), _priceColor);
        }

        public void SetButtonUnlocked(int id)
        {
            _backgroundPanels[id].RemoveMoneyImage();
            _backgroundPanels[id].SetButtonText("V", Color.white);
        }
        
        public void SetButtonSelected(int id)
        {
            _backgroundPanels[id].RemoveMoneyImage();
            _backgroundPanels[id].SetButtonText("V", Color.green);
        }

        public void SetMoney(int money)
        {
            _moneyText.text = Convert.ToString(money);
        }
        
        public void RemoveMoney(int removed, int final)
        {
            if (_moneyCoroutine != null)
            {
                StopCoroutine(_moneyCoroutine);
            }

            _moneyCoroutine = StartCoroutine(SetMoneyCoroutine(removed, final));
        }

        private IEnumerator SetMoneyCoroutine(int removed, int final)
        {
            var a = final + removed;
            _moneyText.text = Convert.ToString(a);
            DOTween.To(()=> a, x=> a = x, final, 2);

            while (a != final)
            {
                yield return new WaitForSeconds(0.1f);
                _moneyText.text = Convert.ToString(a);
            }
        }
        
        public void OnClose()
        {
            CloseEvent?.Invoke();
        }
    }
}