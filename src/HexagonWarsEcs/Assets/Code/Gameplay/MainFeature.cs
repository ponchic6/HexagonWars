using Code.Gameplay.Features.Battle.Systems;
using Code.Gameplay.Features.Building.Systems;
using Code.Gameplay.Features.Citizens.Systems;
using Code.Gameplay.Features.Logistics.Systems;
using Code.Gameplay.Features.Map.Systems;
using Code.Gameplay.Features.Migration.Systems;
using Code.Gameplay.Features.Production.Systems;
using Code.Infrastructure.Destroy;
using Code.Infrastructure.Systems;
using Code.Infrastructure.View;

namespace Code.Gameplay
{
    public class MainFeature : Feature
    {
        public MainFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<ViewFeature>());
            Add(systemFactory.Create<MapFeature>());
            Add(systemFactory.Create<MigrationFeature>());
            Add(systemFactory.Create<CitizensFeature>());
            Add(systemFactory.Create<BuildFeature>());
            Add(systemFactory.Create<SupplyFeature>());
            Add(systemFactory.Create<BattleFeature>());
            Add(systemFactory.Create<ProductionFeature>());
            Add(systemFactory.Create<DestroyFeature>());
        }
    }
}