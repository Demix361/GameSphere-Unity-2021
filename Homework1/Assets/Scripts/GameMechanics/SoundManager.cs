using UnityEngine;
using UnityEngine.Audio;

namespace GameMechanics
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private ModelManager _modelManager;

        private void Start()
        {
            ChangeMusicVolume(_modelManager.PlayerModel.MusicVolume);
            ChangeEffectsVolume(_modelManager.PlayerModel.EffectsVolume);
            
            _modelManager.PlayerModel.ChangeMusicVolume += ChangeMusicVolume;
            _modelManager.PlayerModel.ChangeEffectsVolume += ChangeEffectsVolume;
        }

        private void ChangeMusicVolume(float value)
        {
            _audioMixer.SetFloat("volMusic", value);
        }
        
        private void ChangeEffectsVolume(float value)
        {
            _audioMixer.SetFloat("volEffects", value);
        }
    }
}