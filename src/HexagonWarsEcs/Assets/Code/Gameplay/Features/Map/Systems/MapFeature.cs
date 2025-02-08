using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Map.Systems
{
    public class MapFeature : Feature
    {
        public MapFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<MapCreatorSystem>());
            Add(systemFactory.Create<SetPopulationSystem>());
        }
    }
}