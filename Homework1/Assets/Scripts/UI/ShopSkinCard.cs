using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopSkinCard : MonoBehaviour
    {
        [SerializeField] private Image skinImage;
        [SerializeField] private Text buttonText;
        [SerializeField] private GameObject buyPS;
        [SerializeField] private Transform particleSystemSpawnPoint;
        [SerializeField] private Image borderImage;

        public int Id;
        
        public void RunParticleSystem()
        {
            Instantiate(buyPS, particleSystemSpawnPoint.position, Quaternion.identity);
        }

        public void Set(string text, Sprite sprite, Color borderColor)
        {
            buttonText.text = text;
            skinImage.sprite = sprite;
            borderImage.color = borderColor;
        }
    }
}