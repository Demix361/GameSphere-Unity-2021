using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BackgroundPanel : MonoBehaviour
    {
        [SerializeField] private Image previewImage;
        [SerializeField] private Text buttonText;
        [SerializeField] private Text nameText;
        [SerializeField] private GameObject buyPS;
        [SerializeField] private GameObject buttonBuyPanel;
        [SerializeField] private GameObject buttonSelectPanel;
        [SerializeField] private Image checkImage;
        [SerializeField] private Color moneyColor;
        [SerializeField] private Transform particleSystemSpawnPoint;

        public void RunParticleSystem()
        {
            Instantiate(buyPS, particleSystemSpawnPoint);
        }

        public void SetSelectButton(bool state)
        {
            buttonBuyPanel.SetActive(false);
            buttonSelectPanel.SetActive(true);
            
            if (state)
            {
                checkImage.color = Color.green;
            }
            else
            {
                checkImage.color = Color.white;
            }
        }

        public void SetBuyButton(string text)
        {
            buttonBuyPanel.SetActive(true);
            buttonSelectPanel.SetActive(false);
            
            buttonText.text = text;
            buttonText.color = moneyColor;
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