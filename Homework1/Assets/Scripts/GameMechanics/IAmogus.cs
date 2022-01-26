using UnityEngine;

namespace GameMechanics
{
    public interface IAmogus
    {
        public AmogusType Type { get; }
        public AmogusInfo Info { get; }
        
        
        public enum AmogusType
        {
            Crewmate,
            Bonus,
            Impostor,
            SuperCrewmate,
            Rage
        }

        public void SetAmogus(float scaleSpeed, int minSortingOrder, GameController gameController);
        public void Clicked();
        public void Clicked(Vector3 pos);
        public void SafeDestroy();
    }
}