using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowSkinCard : MonoBehaviour
    {
        [SerializeField] private Image skinImage;
        [SerializeField] private Image defaultImage;
        [SerializeField] private Image cardSpriteRenderer;

        public int Id;

        public void Set(Sprite skinSprite, Sprite defaultSprite, Color borderColor)
        {
            skinImage.sprite = skinSprite;
            defaultImage.sprite = defaultSprite;
            cardSpriteRenderer.color = borderColor;
        }
    }
}