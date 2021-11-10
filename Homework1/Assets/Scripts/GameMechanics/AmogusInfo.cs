using UnityEngine;

namespace GameMechanics
{
    [CreateAssetMenu]
    public class AmogusInfo : ScriptableObject
    {
        [SerializeField] public string colorName;
        [SerializeField] public Color color;
        [SerializeField] public Sprite crewmateSprite;
        [SerializeField] public Sprite impostorSprite;
        [SerializeField] public Sprite miniSprite;
        [SerializeField] public RuntimeAnimatorController alienKillAnimator;
        [SerializeField] public Sprite miniBoneSprite;
    }
}