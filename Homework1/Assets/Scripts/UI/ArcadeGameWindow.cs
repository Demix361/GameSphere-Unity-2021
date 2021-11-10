using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ArcadeGameWindow : MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Text timeText;

        public void SetPoints(string text)
        {
            pointsText.text = text;
        }

        public void SetTime(string text)
        {
            timeText.text = text;
        }
    }
}