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
            _shopWindow.BgButtonEvent += OnBgButton;
            
            SpawnBgPanels();
            SetBgButtons();
            
            _shopWindow.SetMainPanel(_playerModel.BackgroundInfos.Length);
            _shopWindow.SetMoney(_playerModel.Money);
        }

        private void SpawnBgPanels()
        {
            for (int i = 0; i < _playerModel.BackgroundInfos.Length; i++)
            {
                _shopWindow.SpawnBgPanel(_playerModel.BackgroundInfos[i]);
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
            _shopWindow.CloseEvent -= OnExit;
            _shopWindow.BgButtonEvent -= OnBgButton;
        }
    }
}