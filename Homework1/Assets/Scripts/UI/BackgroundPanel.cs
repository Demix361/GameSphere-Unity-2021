using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BackgroundPanel : MonoBehaviour
    {
        [SerializeField] private Image previewImage;
        [SerializeField] private Image moneyImage;
        [SerializeField] private Text buttonText;
        [SerializeField] private Text nameText;

        public void RemoveMoneyImage()
        {
            if (moneyImage)
            {
                Destroy(moneyImage.gameObject);
                buttonText.rectTransform.anchorMin = new Vector2(0, 0);
                buttonText.rectTransform.anchorMax = new Vector2(1, 1);
                buttonText.rectTransform.localPosition = Vector3.zero;
                buttonText.rectTransform.anchoredPosition = Vector2.zero;
                buttonText.rectTransform.sizeDelta = Vector2.zero;
                
            }
        }
        
        public void SetButtonText(string text)
        {
            buttonText.text = text;
        }
        
        public void SetButtonText(string text, Color color)
        {
            buttonText.text = text;
            buttonText.color = color;
        }

        public void SetNameText(string text)
        {
            nameText.text = text;
        }

        public void SetPreviewImage(Sprite sprite)
        {
            previewImage.sprite = sprite;
        }
    }
}