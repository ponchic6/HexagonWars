using Code.Gameplay.Features.Logistics.View;

namespace Code.Gameplay.Features.Logistics.Services
{
    public interface ISupplyRouteFactory
    {
        void StartCreateRoute(LogisticNode logisticNode);
        bool TryAjustLogicNode(LogisticNode logisticNode);
        bool TryFinishOfCreatingRoute(LogisticNode logisticNode);
    }
}