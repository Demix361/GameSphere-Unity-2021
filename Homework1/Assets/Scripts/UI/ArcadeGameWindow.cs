using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Localization.Components;

namespace UI
{
    public class ArcadeGameWindow : MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Text timeText;
        [SerializeField] private GameObject defaultNotificationPrefab;
        [SerializeField] private GameObject maxComboNotificationPrefab;
        [SerializeField] private GameObject comboOfNotificationPrefab;
        [SerializeField] private GameObject comboInRowNotificationPrefab;
        
        public event Action PauseEvent;
        
        public void SetPoints(string text)
        {
            pointsText.text = text;
        }

        public void SetTime(string text)
        {
            timeText.text = text;
        }

        public void ShowDefaultNotification(string text, Color color, Vector2 pos)
        {
            var not = Instantiate(defaultNotificationPrefab, gameObject.transform);
            var textComponent = not.GetComponent<Text>();
            textComponent.text = text;
            textComponent.color = color;
            not.transform.position = pos;

            StartCoroutine(LifeCycle(not));
        }

        public void ShowMaxComboNotification(Color color, Vector2 pos, string val1)
        {
            var not = Instantiate(maxComboNotificationPrefab, gameObject.transform);
            var textComponent = not.GetComponent<Text>();
            textComponent.color = color;
            not.transform.position = pos;

            var trValues = not.GetComponent<NotificationTranslationValues>();
            trValues.tr1 = val1;
            
            var localizeStringEvent = not.GetComponent<LocalizeStringEvent>();
            localizeStringEvent.enabled = true;

            StartCoroutine(LifeCycle(not));
        }
        
        public void ShowComboOfNotification(Color color, Vector2 pos, string val1, string val2)
        {
            var not = Instantiate(comboOfNotificationPrefab, gameObject.transform);
            var textComponent = not.GetComponent<Text>();
            textComponent.color = color;
            not.transform.position = pos;

            var trValues = not.GetComponent<NotificationTranslationValues>();
            trValues.tr1 = val1;
            trValues.tr2 = val2;

            var localizeStringEvent = not.GetComponent<LocalizeStringEvent>();
            localizeStringEvent.enabled = true;

            StartCoroutine(LifeCycle(not));
        }
        
        public void ShowComboInRowNotification(Color color, Vector2 pos, string val1, string val2, float delay)
        {
            StartCoroutine(ShowComboInRowNotificationCoroutine(color, pos, val1, val2, delay));
        }

        public void PauseGame()
        {
            PauseEvent?.Invoke();
        }

        private IEnumerator ShowComboInRowNotificationCoroutine(Color color, Vector2 pos, string val1, string val2,
            float delay)
        {
            yield return new WaitForSeconds(delay);
            
            var not = Instantiate(comboInRowNotificationPrefab, gameObject.transform);
            var textComponent = not.GetComponent<Text>();
            textComponent.color = color;
            not.transform.position = pos;

            var trValues = not.GetComponent<NotificationTranslationValues>();
            trValues.tr1 = val1;
            trValues.tr2 = val2;
            
            var localizeStringEvent = not.GetComponent<LocalizeStringEvent>();
            localizeStringEvent.enabled = true;

            StartCoroutine(LifeCycle(not));
        }

        private IEnumerator LifeCycle(GameObject notification)
        {
            yield return new WaitForSeconds(1f);
            
            Destroy(notification);
        }
    }
}