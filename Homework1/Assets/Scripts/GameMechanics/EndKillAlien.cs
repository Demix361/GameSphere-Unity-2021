using UnityEngine;

namespace GameMechanics
{
    public class EndKillAlien : MonoBehaviour
    {
        [SerializeField] public Animator ImpostorAnimator;
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        public void SetBackgroundWidth(float width)
        {
            backgroundSpriteRenderer.size = new Vector2(width, backgroundSpriteRenderer.size.y);
        }
    }
}
