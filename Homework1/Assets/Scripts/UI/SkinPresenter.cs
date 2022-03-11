using System;
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
            
            foreach (var info in _playerModel.AmogusInfos)
            {
                _skinWindow.SpawnShowCard(info.colorName, info.crewmateSprite, _playerModel.GetSelectedSkinSprite(info.colorName));
            }
        }

        private void SpawnSelectCards(string colorId)
        {
            var count = 0;
            
            foreach (var info in _playerModel.AmogusInfos)
            {
                if (info.colorName == colorId)
                {
                    _skinWindow.SpawnSelectCard(colorId, -1, info.crewmateSprite, _playerModel.GetSelectedSkin(colorId) == -1);
                    count += 1;
                    break;
                }
            }

            foreach (var skin in _playerModel.SkinInfos)
            {
                var selectedInColor = _playerModel.GetColorBySkin(skin.id);
                var bought = _playerModel.GetSkinStatus(skin.id);
                
                if (bought && (selectedInColor == null || selectedInColor == colorId))
                {
                    _skinWindow.SpawnSelectCard(colorId, skin.id, skin.skin, selectedInColor == colorId);
                    count += 1;
                }
            }
            
            _skinWindow.SetSelectPanel(count);
        }

        private void OnShowSkin(string colorId)
        {
            _skinWindow.ShowSelectPanel();
            _skinWindow.DestroyShowCards();
            _skinWindow.SetCurrentSkin(_playerModel.GetSelectedSkinSprite(colorId));
            
            SpawnSelectCards(colorId);
        }
        
        private void OnSelectSkin(string colorId, int skinId)
        {
            Debug.Log(123);
            _skinWindow.ChangeSelectedSkin(_playerModel.GetSelectedSkin(colorId), skinId);
            _playerModel.SetSelectedSkin(colorId, skinId);
            _skinWindow.SetCurrentSkin(_playerModel.GetSelectedSkinSprite(colorId));
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