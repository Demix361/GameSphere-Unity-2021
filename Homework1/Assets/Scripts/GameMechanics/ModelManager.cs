using UnityEngine;

namespace GameMechanics
{
    public class ModelManager : MonoBehaviour
    {
        private PlayerModel _playerModel;
        private ClassicGameModel _classicGameModel;
        private ArcadeGameModel _arcadeGameModel;
        private MainMenuModel _mainMenuModel;
        
        public PlayerModel PlayerModel => _playerModel;
        public ClassicGameModel ClassicGameModel => _classicGameModel;
        public ArcadeGameModel ArcadeGameModel => _arcadeGameModel;
        public MainMenuModel MainMenuModel => _mainMenuModel;

        private void Awake()
        {
            _playerModel = new PlayerModel();
            _classicGameModel = new ClassicGameModel(_playerModel);
            _arcadeGameModel = new ArcadeGameModel(_playerModel);
            _mainMenuModel = new MainMenuModel(_playerModel);
        }
    }
}