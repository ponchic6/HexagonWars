using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class CitizensFeature : Feature
    {
        public CitizensFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<FoodDecreasingByCitizensSystem>());
            Add(systemFactory.Create<FoodDecreasingByBuildersSystem>());
            Add(systemFactory.Create<DeathBuildersByHungerSystem>());
            Add(systemFactory.Create<DeathCitizensByHungerSystem>());
            Add(systemFactory.Create<HungerSystem>());
        }
    }
}