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
        [SerializeField] private Text _moneyText;
        [SerializeField] private GameObject _notEnoughMoneyWindow;
        [SerializeField] private GameObject _confirmWindow;
        [SerializeField] private GameObject _bgPanelPrefab;
        [SerializeField] private RectTransform _bgPanelsParent;

        private Coroutine _moneyCoroutine;

        public void SetMainPanel(int amount)
        {
            _bgPanelsParent.sizeDelta = new Vector2(650 * amount + 60 * (amount - 1), _bgPanelsParent.sizeDelta.y);
            _bgPanelsParent.anchoredPosition =
                new Vector2(_bgPanelsParent.sizeDelta.x / 2, _bgPanelsParent.anchoredPosition.y);
        }
        
        public void OnBgPress(int id)
        {
            BgButtonEvent?.Invoke(id);
        }

        public void Buy(int id)
        {
            _backgroundPanels[id].RunParticleSystem();
        }

        public void SpawnBgPanel(BackgroundInfo bgInfo)
        {
            var panel = Instantiate(_bgPanelPrefab, _bgPanelsParent);
            panel.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnBgPress(bgInfo.id); });
            var bgPanel = panel.GetComponent<BackgroundPanel>();
            _backgroundPanels.Add(bgPanel);
            
            bgPanel.SetNameText(Convert.ToString(bgInfo.id + 1));
            bgPanel.SetPreviewImage(bgInfo.preview);
        }

        public void DestroyBgPanels()
        {
            foreach (var panel in _backgroundPanels)
            {
                Destroy(panel.gameObject);
            }

            _backgroundPanels = new List<BackgroundPanel>();
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
            _backgroundPanels[id].SetBuyButton(Convert.ToString(price));
        }

        public void SetButtonUnlocked(int id)
        {
            _backgroundPanels[id].SetSelectButton(false);
        }
        
        public void SetButtonSelected(int id)
        {
            _backgroundPanels[id].SetSelectButton(true);
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