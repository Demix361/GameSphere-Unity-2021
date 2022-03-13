using System;
using System.Collections.Generic;
using GameMechanics;
using UnityEngine;

namespace UI
{
    public class SkinPresenter
    {
        private SkinWindow _skinWindow;
        private event Action _onExit;
        private PlayerModel _playerModel;

        public SkinPresenter(PlayerModel playerModel, SkinWindow skinWindow, Action onExit)
        {
            _skinWindow = skinWindow;
            _onExit = onExit;
            _playerModel = playerModel;
        }

        public void OnOpen()
        {
            _skinWindow.CloseEvent += OnExit;
            _skinWindow.ShowSkinEvent += OnShowSkin;
            _skinWindow.SelectSkinEvent += OnSelectSkin;
            _skinWindow.CloseSelectPanelEvent += OnExitSelectPanel;
            
            _skinWindow.ShowShowPanel();
            SpawnShowCards();
        }
        
        private void SpawnShowCards()
        {
            _skinWindow.SetShowPanel((_playerModel.AmogusInfos.Length + 1) / 2);
            
            foreach (var amogusInfo in _playerModel.AmogusInfos)
            {
                var skinInfo = _playerModel.GetSkinInfoByColor(amogusInfo.colorName);
                _skinWindow.SpawnShowCard(amogusInfo.colorName, amogusInfo.crewmateSprite,
                    skinInfo.skin,
                    _playerModel.GetBorderColor(skinInfo.rarity));
            }
        }

        private void SpawnSelectCards(string colorId)
        {
            var skinList = new List<SkinInfo>();

            skinList.Add(_playerModel.GetAmogusInfoByColor(colorId).defaultSkin);
            foreach (var skin in _playerModel.SkinInfos)
            {
                var selectedInColor = _playerModel.GetColorBySkin(skin.id);
                var bought = _playerModel.GetSkinStatus(skin.id);
                
                if (bought && (selectedInColor == null || selectedInColor == colorId))
                {
                    skinList.Add(skin);
                }
            }
            
            IComparer<SkinInfo> skinComparer = new PlayerModel.ReverseSkinRarityComparer();
            skinList.Sort(skinComparer);

            foreach (var skin in skinList)
            {
                var selectedInColor = _playerModel.GetColorBySkin(skin.id);
                _skinWindow.SpawnSelectCard(colorId, skin.id, skin.skin, _playerModel.GetBorderColor(skin.rarity), selectedInColor == colorId);
            }

            _skinWindow.SetSelectPanel(skinList.Count);
        }

        private void OnShowSkin(string colorId)
        {
            var currentSkin = _playerModel.GetSkinInfoByColor(colorId);
            
            _skinWindow.ShowSelectPanel();
            _skinWindow.DestroyShowCards();
            _skinWindow.SetCurrentSkin(currentSkin.skin, _playerModel.GetBorderColor(currentSkin.rarity));
            
            SpawnSelectCards(colorId);
        }
        
        private void OnSelectSkin(string colorId, int skinId)
        {
            _skinWindow.ChangeSelectedSkin(_playerModel.GetSkinIdByColor(colorId), skinId);
            _playerModel.SetSelectedSkin(colorId, skinId);
            
            var currentSkin = _playerModel.GetSkinInfoByColor(colorId);
            _skinWindow.SetCurrentSkin(currentSkin.skin, _playerModel.GetBorderColor(currentSkin.rarity));
        }

        private void OnExitSelectPanel()
        {
            _skinWindow.DestroySelectCards();
            _skinWindow.ShowShowPanel();
            SpawnShowCards();
        }

        private void OnExit()
        {
            _onExit?.Invoke();
        }

        public void OnClose()
        {
            _skinWindow.DestroyShowCards();

            _skinWindow.CloseEvent -= OnExit;
            _skinWindow.ShowSkinEvent -= OnShowSkin;
            _skinWindow.SelectSkinEvent -= OnSelectSkin;
            _skinWindow.CloseSelectPanelEvent -= OnExitSelectPanel;
        }
    }
}