using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Warriors.Systems
{
    public class WarriorsFeature : Feature
    {
        public WarriorsFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<IdleSoldiersViewControlReactiveSystem>());
            Add(systemFactory.Create<RunningSoldiersViewControlReactiveSystem>());
        }
    }
} 