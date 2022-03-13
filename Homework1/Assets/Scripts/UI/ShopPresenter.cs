using System;
using System.Collections.Generic;
using GameMechanics;

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
            _shopWindow.NoSkinsLeftEvent += OnNoSkinsLeft;
            
            OnOpenSkins();
        }

        private void OnNoSkinsLeft()
        {
            _shopWindow.ShowNoSkinsPanel(true);
        }

        private void OnOpenSkins()
        {
            _shopWindow.ShowSkinWindow();
            _shopWindow.DestroyBgPanels();
            
            SpawnSkinCards();

            var c = 0;
            foreach (var skin in _playerModel.SkinInfos)
            {
                var defaultSkin = false;
                foreach (var amogusInfo in _playerModel.AmogusInfos)
                {
                    if (amogusInfo.defaultSkin == skin)
                    {
                        defaultSkin = true;
                        break;
                    }
                }
                if (!_playerModel.GetSkinStatus(skin.id) && !defaultSkin)
                {
                    c += 1;
                }
            }
            
            _shopWindow.SetMainSkinPanel((c + 1) / 2);
            _shopWindow.SetMoney(_playerModel.Money);

            if (c == 0)
            {
                _shopWindow.ShowNoSkinsPanel(true);
            }
        }

        private void OnOpenBackgrounds()
        {
            _shopWindow.ShowBackgroundWindow();
            _shopWindow.DestroySkins();
            _shopWindow.ShowNoSkinsPanel(false);
            
            SpawnBgPanels();
            SetBgButtons();
            
            _shopWindow.SetMainBgPanel(_playerModel.BackgroundInfos.Length);
            _shopWindow.SetMoney(_playerModel.Money);
        }

        private void SpawnSkinCards()
        {
            IComparer<SkinInfo> skinComparer = new PlayerModel.ReverseSkinPriceComparer();
            Array.Sort(_playerModel.SkinInfos, skinComparer);
            
            foreach (var skinInfo in _playerModel.SkinInfos)
            {
                var defaultSkin = false;
                foreach (var amogusInfo in _playerModel.AmogusInfos)
                {
                    if (amogusInfo.defaultSkin == skinInfo)
                    {
                        defaultSkin = true;
                        break;
                    }
                }
                
                if (!_playerModel.GetSkinStatus(skinInfo.id) && !defaultSkin)
                {
                    _shopWindow.SpawnSkinCard(skinInfo.id, Convert.ToString(skinInfo.price), skinInfo.skin, _playerModel.GetBorderColor(skinInfo.rarity));
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

        private void OnSkinButton(int skinId)
        {
            var price = _playerModel.GetSkin(skinId).price;
            if (_playerModel.Money - price >= 0)
            {
                _playerModel.BuySkin(skinId);
                    
                _shopWindow.RemoveMoney(price, _playerModel.Money);
                _shopWindow.BuySkin(skinId);
                _shopWindow.RemoveSkin(skinId);
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
            _shopWindow.ShowNoSkinsPanel(false);

            _shopWindow.OpenSkinsEvent -= OnOpenSkins;
            _shopWindow.OpenBackgroundsEvent -= OnOpenBackgrounds;
            _shopWindow.CloseEvent -= OnExit;
            _shopWindow.BgButtonEvent -= OnBgButton;
            _shopWindow.SkinButtonEvent -= OnSkinButton;
            _shopWindow.NoSkinsLeftEvent -= OnNoSkinsLeft;
        }
    }
}