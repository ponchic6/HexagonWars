using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Logistics.Systems
{
    public class SupplyFeature : Feature
    {
        public SupplyFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<SupplyAvailabilityBuildSystem>());
            Add(systemFactory.Create<SupplyProceedSystem>());
            Add(systemFactory.Create<SupplyRoutsAddUiReactiveSystem>());
            Add(systemFactory.Create<SupplyRoutsRemoveUiReactiveSystem>());
            Add(systemFactory.Create<SupplyRoutHighlightReactiveSystem>());
            Add(systemFactory.Create<ChangeResourceAmountAtStartHexUiReactiveSystem>());
        }
    }
}