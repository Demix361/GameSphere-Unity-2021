using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatsPanel : MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Text missedText;

        public void ChangePointsText(int value)
        {
            pointsText.text = "Points: " + Convert.ToString(value);
        }
        
        public void ChangeMissedText(int value)
        {
            missedText.text = "Missed: " + Convert.ToString(value);
        }
    }
}
