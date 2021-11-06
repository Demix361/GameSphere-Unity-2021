using System;
using UnityEngine;

namespace GameMechanics
{
    public class MainMenuModel
    {
        public float SpawnInterval = 2;
        public float Force = 200;
        public float Torque = 20;
        
        public event Action StartSpawnEvent;
        public event Action StopSpawnEvent;

        public void StartSpawn()
        {
            StartSpawnEvent?.Invoke();
            Debug.Log("invoke");
        }

        public void StopSpawn()
        {
            StopSpawnEvent?.Invoke();
        }
    }
}