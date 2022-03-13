using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectSkinCard : MonoBehaviour
    {
        [SerializeField] private Image skinImage;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Image borderImage;

        public int Id;

        public void Set(int id, Sprite skinSprite, Color borderColor, bool selected)
        {
            skinImage.sprite = skinSprite;
            Id = id;
            borderImage.color = borderColor;
            SetSelected(selected);
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                buttonImage.color = Color.green;
            }
            else
            {
                buttonImage.color = Color.white;
            }
        }
    }
}