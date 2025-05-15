using Code.Gameplay.Features.Migration.Systems;
using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class CitizensFeature : Feature
    {
        public CitizensFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<DeathByHungerSystem>());
            Add(systemFactory.Create<HungerSystem>());
            
            Add(systemFactory.Create<IdleCitizensViewControlReactiveSystem>());
            Add(systemFactory.Create<RunningCitizensViewControlReactiveSystem>());
        }
    }
}