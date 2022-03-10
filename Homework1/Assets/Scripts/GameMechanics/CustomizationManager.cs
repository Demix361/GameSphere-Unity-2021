using UnityEngine;

namespace GameMechanics
{
    public class CustomizationManager : MonoBehaviour
    {
        [SerializeField] private ModelManager modelManager;
        [SerializeField] private BackgroundInfo[] backgroundInfos;
        [SerializeField] private SkinInfo[] skinInfos;

        private GameObject _background;

        private void Start()
        {
            var curBg = modelManager.PlayerModel.Background;

            modelManager.PlayerModel.BackgroundInfos = backgroundInfos;
            modelManager.PlayerModel.SkinInfos = skinInfos;

            modelManager.PlayerModel.ChangeBackground += ChangeBackground;
            
            ChangeBackground(curBg);
        }

        private void ChangeBackground(int index)
        {
            Destroy(_background);
            _background = Instantiate(backgroundInfos[index].prefab);
        }
    }
}