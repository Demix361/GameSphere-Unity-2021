using System.Collections;
using UnityEngine;

namespace UI
{
    public class NotificationWindow : MonoBehaviour
    {
        [SerializeField] private GameObject textPrefab;

        public void ShowNotification(string text, Color color, Vector2 pos)
        {
            var not = Instantiate(textPrefab, gameObject.transform);

            StartCoroutine(LifeCycle(not));
        }

        private IEnumerator LifeCycle(GameObject notification)
        {
            yield return new WaitForSeconds(1f);
            
            Destroy(notification);
        }
    }
}