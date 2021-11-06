using System;
using UnityEngine;

namespace GameMechanics
{
    public class ModelManager : MonoBehaviour
    {
        private static PlayerModel _playerModel = new PlayerModel();
        private ClassicGameModel _classicGameModel = new ClassicGameModel(_playerModel);
        private ArcadeGameModel _arcadeGameModel = new ArcadeGameModel(_playerModel);
        private MainMenuModel _mainMenuModel = new MainMenuModel();
        
        public PlayerModel PlayerModel => _playerModel;
        public ClassicGameModel ClassicGameModel => _classicGameModel;
        public ArcadeGameModel ArcadeGameModel => _arcadeGameModel;
        public MainMenuModel MainMenuModel => _mainMenuModel;
    }
}