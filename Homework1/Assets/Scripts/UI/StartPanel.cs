using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartPanel : MonoBehaviour
    {
        [SerializeField] private Toggle scaleToggle;
        [SerializeField] private Toggle colorToggle;
        [SerializeField] private Toggle rotationToggle;
        [SerializeField] private InputField lengthInputField;

        [SerializeField] private GameMechanics.GameController gameController;
        
        public void OnClickStartGame()
        {
            var length = Convert.ToInt32(lengthInputField.text);
            var scale = scaleToggle.isOn;
            var color = colorToggle.isOn;
            var rotation = rotationToggle.isOn;
            
            gameController.InitializeGameController(length, color, scale, rotation);
            gameController.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
