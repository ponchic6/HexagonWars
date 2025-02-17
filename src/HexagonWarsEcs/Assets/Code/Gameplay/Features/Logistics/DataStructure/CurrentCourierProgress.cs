using System;

namespace Code.Gameplay.Features.Logistics.DataStructure
{
    [Serializable]
    public class CurrentCourierProgress
    {
        public float currentProgress;
        public LogisticResources logisticResources;

        public CurrentCourierProgress()
        {
        }
        
        public CurrentCourierProgress(LogisticResources logisticResources)
        {
            this.logisticResources = logisticResources;
        }
    }
}