using System;

namespace Code.Gameplay.Features.Building.DataStructure
{
    [Serializable]
    public class BuildProgressContainer
    {
        public float fullProgress;
        public float currentProgress;
        public int buildersAmount;
        public BuildingsType buildingType;
        public bool ready;
    }
}