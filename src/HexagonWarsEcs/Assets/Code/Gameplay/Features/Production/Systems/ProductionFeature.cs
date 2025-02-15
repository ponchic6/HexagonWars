using Code.Infrastructure.Systems;

namespace Code.Gameplay.Features.Production.Systems
{
    public class ProductionFeature : Feature
    {
        public ProductionFeature(ISystemFactory systemFactory)
        {
            Add(systemFactory.Create<ProductionUiViewUpdateSystem>());
            Add(systemFactory.Create<FoodProductionSystem>());
        }
    }
}