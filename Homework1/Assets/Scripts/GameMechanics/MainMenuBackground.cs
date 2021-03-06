using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class MainMenuBackground : MonoBehaviour
    {
        [SerializeField] private ModelManager modelManager;
        [SerializeField] private GameObject amogusPrefab;
        [SerializeField] private AudioSource _menuSong;

        private float halfHeight;
        private float halfWidth;
        private Coroutine _spawnCoroutine;

        private void Start()
        {
            halfHeight = Camera.main.orthographicSize * 1.6f;
            halfWidth = halfHeight * Camera.main.aspect;

            modelManager.MainMenuModel.StartSpawnEvent += StartSpawn;
            modelManager.MainMenuModel.StopSpawnEvent += StopSpawn;
        }

        private void StartSpawn()
        {
            if (_spawnCoroutine == null)
            {
                _menuSong.Play();
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
                _menuSong.Stop();
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
                
                var destination = new Vector2(Random.Range(-newW, newW) - x, Random.Range(-newH, newH) - y).normalized * modelManager.MainMenuModel.Force;
                
                var newAmogus = Instantiate(amogusPrefab, new Vector2(x, y), Quaternion.identity);
                newAmogus.GetComponent<Rigidbody2D>().AddForce(destination);
                newAmogus.GetComponent<Rigidbody2D>().AddTorque((Random.Range(-0.5f, 0.5f)) * modelManager.MainMenuModel.Torque);

                newAmogus.GetComponent<MainMenuAmogus>().SetSprite(modelManager.MainMenuModel.GetAmogusSprite());

                yield return new WaitForSeconds(modelManager.MainMenuModel.SpawnInterval);
            }
        }
    }
}