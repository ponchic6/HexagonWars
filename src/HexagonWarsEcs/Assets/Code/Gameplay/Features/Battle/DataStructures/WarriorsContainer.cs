using System;

namespace Code.Gameplay.Features.Battle.DataStructures
{
    [Serializable]
    public class WarriorsContainer
    {
        public int warriorsCount;
        public int hexagonId;

        public WarriorsContainer(int warriorsCount, int hexagonId)
        {
            this.warriorsCount = warriorsCount;
            this.hexagonId = hexagonId;
        }
    }
}