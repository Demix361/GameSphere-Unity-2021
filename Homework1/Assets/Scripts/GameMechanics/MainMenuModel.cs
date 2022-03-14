using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class MainMenuModel
    {
        public float SpawnInterval = 2;
        public float Force = 200;
        public float Torque = 20;
        
        public event Action StartSpawnEvent;
        public event Action StopSpawnEvent;
        private PlayerModel _playerModel;

        public MainMenuModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public Sprite GetAmogusSprite()
        {
            var amogusInfo = _playerModel.AmogusInfos[Random.Range(0, _playerModel.AmogusInfos.Length)];
            var skinInfo = _playerModel.GetSkinInfoByColor(amogusInfo.colorName);

            return skinInfo.skin;
        }
        
        public void StartSpawn()
        {
            StartSpawnEvent?.Invoke();
        }

        public void StopSpawn()
        {
            StopSpawnEvent?.Invoke();
        }
    }
}