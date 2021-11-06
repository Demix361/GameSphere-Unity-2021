using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class MainMenuBackground : MonoBehaviour
    {
        [SerializeField] private ModelManager _modelManager;
        [SerializeField] private GameObject amogusPrefab;

        private float halfHeight;
        private float halfWidth;
        private Coroutine _spawnCoroutine;

        private void Start()
        {
            halfHeight = Camera.main.orthographicSize * 1.6f;
            halfWidth = halfHeight * Camera.main.aspect;

            _modelManager.MainMenuModel.StartSpawnEvent += StartSpawn;
            _modelManager.MainMenuModel.StopSpawnEvent += StopSpawn;
        }

        private void StartSpawn()
        {
            if (_spawnCoroutine == null)
            {
                _spawnCoroutine = StartCoroutine(SpawnCoroutine());
            }
        }

        private void StopSpawn()
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
            
            var amoguses = FindObjectsOfType<MainMenuAmogus>();
            foreach (var a in amoguses)
            {
                Destroy(a.gameObject);
            }
        }
        
        private IEnumerator SpawnCoroutine()
        {
            var newH = Camera.main.orthographicSize * 0.8f;
            var newW = Camera.main.orthographicSize * Camera.main.aspect;
            
            while (true)
            {
                var angle = Random.Range(0f, 2f * (float) Math.PI);
                float x = (float) (Math.Cos(angle) * halfWidth);
                float y = (float) (Math.Sin(angle) * halfWidth);
                
                var destination = new Vector2(Random.Range(-newW, newW) - x, Random.Range(-newH, newH) - y).normalized * _modelManager.MainMenuModel.Force;
                
                var newAmogus = Instantiate(amogusPrefab, new Vector2(x, y), Quaternion.identity);
                newAmogus.GetComponent<Rigidbody2D>().AddForce(destination);
                newAmogus.GetComponent<Rigidbody2D>().AddTorque((Random.Range(-0.5f, 0.5f)) * _modelManager.MainMenuModel.Torque);

                yield return new WaitForSeconds(_modelManager.MainMenuModel.SpawnInterval);
            }
        }
    }
}