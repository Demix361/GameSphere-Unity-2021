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
            Impostor,
            Super,
            Rage,
            Frozen,
            Metal
        }

        public void SetAmogus(int minSortingOrder, GameController gameController);
        public bool Clicked();
        public void Clicked(Vector3 pos);
        public void SafeDestroy();
    }
}