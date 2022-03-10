﻿using UnityEngine;

namespace GameMechanics
{
    [CreateAssetMenu]
    public class SkinInfo : ScriptableObject
    {
        [SerializeField] public int price;
        [SerializeField] public int id;
        [SerializeField] public Sprite skin;
    }
}