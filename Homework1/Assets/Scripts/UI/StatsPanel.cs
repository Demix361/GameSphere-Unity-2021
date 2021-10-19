using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsPanel : MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Image[] crossImages;
        [SerializeField] private Sprite fullCross;
        [SerializeField] private Sprite emptyCross;
        
        private int curMissed = 0;
        
        private void OnEnable()
        {
            curMissed = 0;
            foreach (var img in crossImages)
            {
                img.sprite = emptyCross;
            }
            ChangePointsText(0);
        }

        public void ChangePointsText(int value)
        {
            pointsText.text = "Points: " + Convert.ToString(value);
        }
        
        public void IncreaseMissed()
        {
            if (curMissed + 1 <= 3)
            {
                crossImages[curMissed].sprite = fullCross;
                curMissed += 1;
            }
        }
        
        public void DecreaseMissed()
        {
            if (curMissed - 1 >= 0)
            {
                curMissed -= 1;
                crossImages[curMissed].sprite = emptyCross;
            }
        }
    }
}
