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
        public event Action OpenSkinsEvent;
        public event Action OpenBackgroundsEvent;
        public event Action<int> BgButtonEvent;
        public event Action<int> SkinButtonEvent;
        
        [SerializeField] private Text _moneyText;
        [SerializeField] private GameObject _notEnoughMoneyWindow;
        [SerializeField] private GameObject _confirmWindow;
        
        [SerializeField] private GameObject _skinWindow;
        [SerializeField] private GameObject _skinPanelPrefab;
        [SerializeField] private RectTransform _skinPanelsParent;
        [SerializeField] private Button _skinPanelButton;
        
        [SerializeField] private GameObject _bgWindow;
        [SerializeField] private GameObject _bgPanelPrefab;
        [SerializeField] private RectTransform _bgPanelsParent;
        [SerializeField] private Button _bgPanelButton;
        
        private List<BackgroundPanel> _backgroundPanels = new List<BackgroundPanel>();
        private List<SkinPanel> _skinPanels = new List<SkinPanel>();
        private Coroutine _moneyCoroutine;

        public void OpenSkins()
        {
            OpenSkinsEvent?.Invoke();
        }
        
        public void OpenBackgrounds()
        {
            OpenBackgroundsEvent?.Invoke();
        }

        public void OnSkinPress(int id)
        {
            SkinButtonEvent?.Invoke(id);
        }
        
        public void OnBgPress(int id)
        {
            BgButtonEvent?.Invoke(id);
        }
        
        public void ShowSkinWindow()
        {
            _skinWindow.SetActive(true);
            _bgWindow.SetActive(false);
            
            _skinPanelButton.GetComponent<Image>().color = Color.green;
            _bgPanelButton.GetComponent<Image>().color = Color.white;
        }
        
        public void ShowBackgroundWindow()
        {
            _skinWindow.SetActive(false);
            _bgWindow.SetActive(true);
            
            _skinPanelButton.GetComponent<Image>().color = Color.white;
            _bgPanelButton.GetComponent<Image>().color = Color.green;
        }
        
        // Устанавливает ширину панели, на которой располагаются карточки скинов
        public void SetMainSkinPanel(int amount)
        {
            _skinPanelsParent.sizeDelta = new Vector2(300 * amount + 40 * (amount - 1), _skinPanelsParent.sizeDelta.y);
            _skinPanelsParent.anchoredPosition =
                new Vector2(_skinPanelsParent.sizeDelta.x / 2, _skinPanelsParent.anchoredPosition.y);
        }
        
        public void SetMainBgPanel(int amount)
        {
            _bgPanelsParent.sizeDelta = new Vector2(650 * amount + 60 * (amount - 1), _bgPanelsParent.sizeDelta.y);
            _bgPanelsParent.anchoredPosition =
                new Vector2(_bgPanelsParent.sizeDelta.x / 2, _bgPanelsParent.anchoredPosition.y);
        }

        public void BuySkin(int id)
        {
            foreach (var panel in _skinPanels)
            {
                if (panel.Id == id)
                {
                    panel.RunParticleSystem();
                    break;
                }
            }
        }
        
        public void BuyBg(int id)
        {
            _backgroundPanels[id].RunParticleSystem();
        }
        
        public void SpawnSkinPanel(SkinInfo skinInfo)
        {
            var panel = Instantiate(_skinPanelPrefab, _skinPanelsParent);
            panel.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnSkinPress(skinInfo.id); });
            var skinPanelComponent = panel.GetComponent<SkinPanel>();
            _skinPanels.Add(skinPanelComponent);

            skinPanelComponent.Id = skinInfo.id;
            skinPanelComponent.SetPrice(Convert.ToString(skinInfo.price));
            skinPanelComponent.SetSkinImage(skinInfo.skin);
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

        public void RemoveSkin(int id)
        {
            foreach (var panel in _skinPanels)
            {
                if (panel.Id == id)
                {
                    _skinPanels.Remove(panel);
                    Destroy(panel.gameObject);
                    break;
                }
            }
        }

        public void DestroySkins()
        {
            foreach (var panel in _skinPanels)
            {
                Destroy(panel.gameObject);
            }
            
            _skinPanels.Clear();
        }

        public void DestroyBgPanels()
        {
            foreach (var panel in _backgroundPanels)
            {
                Destroy(panel.gameObject);
            }

            _backgroundPanels.Clear();
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