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
            Impostor
        }

        public void SetAmogus(float scaleSpeed, int minSortingOrder, GameController gameController);
        public void Clicked();
        public void SafeDestroy();
    }
}