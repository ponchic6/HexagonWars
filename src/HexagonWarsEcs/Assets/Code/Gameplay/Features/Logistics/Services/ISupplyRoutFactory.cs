using Code.Gameplay.Features.Logistics.View;

namespace Code.Gameplay.Features.Logistics.Services
{
    public interface ISupplyRoutFactory
    {
        void StartCreateRoute(LogisticNode logisticNode);
        bool TryAjustLogicNode(LogisticNode logisticNode);
        bool TryFinishOfCreatingRoute();
    }
}