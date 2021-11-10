using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ClassicGameWindow : MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Image[] crossImages;
        [SerializeField] private Sprite fullCross;
        [SerializeField] private Sprite emptyCross;

        public void SetPoints(string points)
        {
            pointsText.text = points;
        }

        public void SetCrosses(int failed)
        {
            for (var i = 0; i < crossImages.Length; i++)
            {
                if (i < failed)
                {
                    crossImages[i].GetComponent<Image>().sprite = fullCross;
                }
                else
                {
                    crossImages[i].GetComponent<Image>().sprite = emptyCross;
                }
            }
        }
    }
}
