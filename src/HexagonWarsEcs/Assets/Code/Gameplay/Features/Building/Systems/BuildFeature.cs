using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Building.Systems
{
    public class BuildFeature : Feature
    {
        public BuildFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<BuildingButtonsUpdateSystem>());
            Add(systemFactory.Create<BuildingProgressSystem>());
            Add(systemFactory.Create<BuildingProgressCleanupSystem>());
        }
    }
}