using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    [SerializeField] private GameObject ballPrefab;
    
    private float _count = 0;
    private Camera _cam;
    private float _height;
    private float _width;
    private System.Random _rand;

    private void Start()
    {
        _cam = Camera.main;
        
        _height = _cam.orthographicSize * 2;
        _width = _height * Screen.width / Screen.height;

        _rand = new System.Random();
    }

    private void Update()
    {
        _count += Time.deltaTime;

        if (_count >= spawnInterval)
        {
            _count = 0;

            var pos = new Vector2(((float)_rand.NextDouble() - 0.5f) * _width, ((float)_rand.NextDouble() - 0.5f) * _height);
            
            var ball = Instantiate(ballPrefab, pos, Quaternion.identity);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
            var a = Physics2D.OverlapPointAll(pos);

            foreach (var collider in a)
            {
                if (collider.GetComponent<Ball>())
                {
                    Destroy(collider.gameObject);
                }
            }
        }
        
    }
    
    
}
