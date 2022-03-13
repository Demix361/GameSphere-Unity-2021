using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameMechanics;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkinWindow : MonoBehaviour
    {
        public event Action CloseEvent;
        public event Action CloseSelectPanelEvent;
        public event Action<string> ShowSkinEvent;
        public event Action<string, int> SelectSkinEvent;

        [SerializeField] private GameObject _showCardPanel;
        [SerializeField] private GameObject _showCardPrefab;
        [SerializeField] private RectTransform _showCardsParent;

        [SerializeField] private GameObject _selectPanel;
        [SerializeField] private GameObject _selectCardPrefab;
        [SerializeField] private RectTransform _selectCardsParent;
        [SerializeField] private Image _currentSkinImage;
        [SerializeField] private Image _currentSkinBorderImage;

        private List<ShowSkinCard> _showCards = new List<ShowSkinCard>();
        private List<SelectSkinCard> _selectCards = new List<SelectSkinCard>();

        public void OnShowSkins(string id)
        {
            ShowSkinEvent?.Invoke(id);
        }
        
        public void OnSelectSkin(string colorId, int skinId)
        {
            SelectSkinEvent?.Invoke(colorId, skinId);
        }

        public void OnClose()
        {
            CloseEvent?.Invoke();
        }

        public void OnCloseSelectPanel()
        {
            CloseSelectPanelEvent?.Invoke();
        }

        public void ShowShowPanel()
        {
            _showCardPanel.SetActive(true);
            _selectPanel.SetActive(false);
        }

        public void ShowSelectPanel()
        {
            _showCardPanel.SetActive(false);
            _selectPanel.SetActive(true);
        }
        
        public void SetShowPanel(int amount)
        {
            _showCardsParent.sizeDelta = new Vector2(300 * amount + 40 * (amount - 1), _showCardsParent.sizeDelta.y);
            _showCardsParent.anchoredPosition =
                new Vector2(_showCardsParent.sizeDelta.x / 2, _showCardsParent.anchoredPosition.y);
        }

        public void SetSelectPanel(int amount)
        {
            _selectCardsParent.sizeDelta = new Vector2(375 * amount + 30 * (amount - 1) + 60, _selectCardsParent.sizeDelta.y);
            _selectCardsParent.anchoredPosition =
                new Vector2(_selectCardsParent.sizeDelta.x / 2, _selectCardsParent.anchoredPosition.y);
        }

        public void SpawnShowCard(string id, Sprite defaultSprite, Sprite skinSprite, Color borderColor)
        {
            var card = Instantiate(_showCardPrefab, _showCardsParent);
            card.GetComponent<ShowSkinCard>().Set(skinSprite, defaultSprite, borderColor);
            card.GetComponent<Button>().onClick.AddListener(delegate { OnShowSkins(id); });
            _showCards.Add(card.GetComponent<ShowSkinCard>());
        }

        public void SpawnSelectCard(string colorId, int skinId, Sprite skinSprite, Color borderColor, bool selected)
        {
            var card = Instantiate(_selectCardPrefab, _selectCardsParent);
            card.GetComponent<SelectSkinCard>().Set(skinId, skinSprite, borderColor, selected);
            card.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnSelectSkin(colorId, skinId); });
            _selectCards.Add(card.GetComponent<SelectSkinCard>());
        }

        public void ChangeSelectedSkin(int oldId, int newId)
        {
            foreach (var card in _selectCards)
            {
                if (card.Id == oldId)
                {
                    card.SetSelected(false);
                }

                if (card.Id == newId)
                {
                    card.SetSelected(true);
                }
            }
        }

        public void SetCurrentSkin(Sprite skinSprite, Color borderColor)
        {
            _currentSkinImage.sprite = skinSprite;
            _currentSkinBorderImage.color = borderColor;
        }

        public void DestroySelectCards()
        {
            foreach (var card in _selectCards)
            {
                Destroy(card.gameObject);
            }
            
            _selectCards.Clear();
        }

        public void DestroyShowCards()
        {
            foreach (var card in _showCards)
            {
                Destroy(card.gameObject);
            }
            
            _showCards.Clear();
        }
    }
}