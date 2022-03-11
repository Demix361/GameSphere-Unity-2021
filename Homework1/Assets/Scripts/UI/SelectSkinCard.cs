using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelectSkinCard : MonoBehaviour
    {
        [SerializeField] private Image skinImage;
        [SerializeField] private Image buttonImage;

        public int Id;

        public void Set(int id, Sprite skinSprite, bool selected)
        {
            skinImage.sprite = skinSprite;
            Id = id;
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