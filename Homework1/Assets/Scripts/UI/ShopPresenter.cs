using System;
using GameMechanics;
using UnityEngine;

namespace UI
{
    public class ShopPresenter
    {
        private ShopWindow _shopWindow;
        private event Action _onExit;
        private PlayerModel _playerModel;

        public ShopPresenter(PlayerModel playerModel, ShopWindow shopWindow, Action onExit)
        {
            _shopWindow = shopWindow;
            _onExit = onExit;
            _playerModel = playerModel;
        }

        public void OnOpen()
        {
            _shopWindow.CloseEvent += OnExit;
            _shopWindow.OpenSkinsEvent += OnOpenSkins;
            _shopWindow.OpenBackgroundsEvent += OnOpenBackgrounds;
            _shopWindow.BgButtonEvent += OnBgButton;
            _shopWindow.SkinButtonEvent += OnSkinButton;
            
            OnOpenSkins();
        }

        private void OnOpenSkins()
        {
            _shopWindow.ShowSkinWindow();
            _shopWindow.DestroyBgPanels();
            
            SpawnSkinPanels();

            var c = 0;
            foreach (var skin in _playerModel.SkinInfos)
            {
                if (!_playerModel.GetSkinStatus(skin.id))
                {
                    c += 1;
                }
            }
            
            _shopWindow.SetMainSkinPanel((c + 1) / 2);
            _shopWindow.SetMoney(_playerModel.Money);
        }

        private void OnOpenBackgrounds()
        {
            _shopWindow.ShowBackgroundWindow();
            _shopWindow.DestroySkins();
            
            SpawnBgPanels();
            SetBgButtons();
            
            _shopWindow.SetMainBgPanel(_playerModel.BackgroundInfos.Length);
            _shopWindow.SetMoney(_playerModel.Money);
        }

        private void SpawnSkinPanels()
        {
            foreach (var panel in _playerModel.SkinInfos)
            {
                if (!_playerModel.GetSkinStatus(panel.id))
                {
                    _shopWindow.SpawnSkinPanel(panel);
                }
            }
        }

        private void SpawnBgPanels()
        {
            foreach (var panel in _playerModel.BackgroundInfos)
            {
                _shopWindow.SpawnBgPanel(panel);
            }
        }

        private void SetBgButtons()
        {
            var choosen = _playerModel.Background;
            
            for (int i = 0; i < _playerModel.BackgroundInfos.Length; i++)
            {
                if (i == choosen)
                {
                    _shopWindow.SetButtonSelected(i);
                }
                else
                {
                    if (_playerModel.GetBgStatus(i) == PlayerModel.BackgroundStatus.Locked)
                    {
                        _shopWindow.SetButtonLocked(i, _playerModel.BackgroundInfos[i].price);
                    }
                    else if (_playerModel.GetBgStatus(i) == PlayerModel.BackgroundStatus.Unlocked)
                    {
                        _shopWindow.SetButtonUnlocked(i);
                    }
                }
            }
        }

        private void OnSkinButton(int id)
        {
            if (_playerModel.Money - _playerModel.SkinInfos[id].price >= 0)
            {
                _playerModel.BuySkin(id);
                    
                _shopWindow.RemoveMoney(_playerModel.SkinInfos[id].price, _playerModel.Money);
                _shopWindow.BuySkin(id);
                _shopWindow.RemoveSkin(id);
            }
            else
            {
                _shopWindow.ShowNotEnoughMoneyWindow(true);
            }
        }
        
        private void OnBgButton(int id)
        {
            if (id == _playerModel.Background)
            {
                return;
            }

            if (_playerModel.GetBgStatus(id) == PlayerModel.BackgroundStatus.Unlocked)
            {
                _shopWindow.SetButtonUnlocked(_playerModel.Background);
                _shopWindow.SetButtonSelected(id);
                _playerModel.Background = id;
                return;
            }

            if (_playerModel.GetBgStatus(id) == PlayerModel.BackgroundStatus.Locked)
            {
                if (_playerModel.Money - _playerModel.BackgroundInfos[id].price >= 0)
                {
                    _shopWindow.SetButtonUnlocked(_playerModel.Background);
                    _shopWindow.SetButtonSelected(id);
                    _playerModel.BuyBackground(id);
                    
                    _shopWindow.RemoveMoney(_playerModel.BackgroundInfos[id].price, _playerModel.Money);
                    _shopWindow.BuyBg(id);
                }
                else
                {
                    _shopWindow.ShowNotEnoughMoneyWindow(true);
                }
            }
        }

        private void OnExit()
        {
            _onExit?.Invoke();
        }

        public void OnClose()
        {
            _shopWindow.DestroySkins();
            _shopWindow.DestroyBgPanels();

            _shopWindow.OpenSkinsEvent -= OnOpenSkins;
            _shopWindow.OpenBackgroundsEvent -= OnOpenBackgrounds;
            _shopWindow.CloseEvent -= OnExit;
            _shopWindow.BgButtonEvent -= OnBgButton;
            _shopWindow.SkinButtonEvent -= OnSkinButton;
        }
    }
}