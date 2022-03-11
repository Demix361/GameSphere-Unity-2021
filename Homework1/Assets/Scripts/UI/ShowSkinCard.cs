using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowSkinCard : MonoBehaviour
    {
        [SerializeField] private Image skinImage;
        [SerializeField] private Image defaultImage;

        public int Id;

        public void Set(Sprite skinSprite, Sprite defaultSprite)
        {
            skinImage.sprite = skinSprite;
            defaultImage.sprite = defaultSprite;
        }
    }
}