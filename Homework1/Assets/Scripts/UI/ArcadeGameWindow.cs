using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI
{
    public class ArcadeGameWindow : MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Text timeText;
        [SerializeField] private GameObject notificationTextPrefab;

        public void SetPoints(string text)
        {
            pointsText.text = text;
        }

        public void SetTime(string text)
        {
            timeText.text = text;
        }
        
        public void ShowNotification(string text, Color color, Vector2 pos)
        {
            var not = Instantiate(notificationTextPrefab, gameObject.transform);
            var textComponent = not.GetComponent<Text>();
            textComponent.text = text;
            textComponent.color = color;
            not.transform.position = pos;

            StartCoroutine(LifeCycle(not));
        }

        private IEnumerator LifeCycle(GameObject notification)
        {
            yield return new WaitForSeconds(1f);
            
            Destroy(notification);
        }
    }
}