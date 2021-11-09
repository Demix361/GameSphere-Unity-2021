using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameMechanics
{
    public class RandomClipPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;
        
        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlaySoundCoroutine());
        }

        private IEnumerator PlaySoundCoroutine()
        {
            _audioSource.clip = clips[Random.Range(0, clips.Length)];
            _audioSource.Play();

            yield return new WaitForSeconds(_audioSource.clip.length);
            
            Destroy(gameObject);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}
