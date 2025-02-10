using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyFeature : Feature
    {
        public SupplyFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<SupplyProceedSystem>());
        }
    }
}