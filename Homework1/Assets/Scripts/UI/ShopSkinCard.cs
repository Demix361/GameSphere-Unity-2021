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

        public int Id;
        
        public void RunParticleSystem()
        {
            Instantiate(buyPS, particleSystemSpawnPoint.position, Quaternion.identity);
        }

        public void SetPrice(string text)
        {
            buttonText.text = text;
        }

        public void SetSkinImage(Sprite sprite)
        {
            skinImage.sprite = sprite;
        }
    }
}