using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float popTime;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float maxScale;
    
    private float _counter = 0;
    private Color _deltaColor;

    private void Start()
    {
        _deltaColor = Color.red - spriteRenderer.color;
    }

    private void Update()
    {
        _counter += Time.deltaTime;

        spriteRenderer.color += _deltaColor * Time.deltaTime / popTime;
        
        if (_counter >= popTime)
        {
            Destroy(gameObject);
        }
    }
}
