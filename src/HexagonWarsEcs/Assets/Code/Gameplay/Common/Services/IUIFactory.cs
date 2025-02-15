using Code.Gameplay.Features.Building.View;
using Code.Gameplay.Features.Production.View.UI;
using Code.Infrastructure.View;

namespace Code.Gameplay.Common.Services
{
    public interface IUIFactory
    {
        public void ShowInfoPanel(EntityBehaviour entityBehaviour);
        public void HideInfoPanel(EntityBehaviour entityBehaviour);
        public BuildingInfoPanel BuildingInfoPanel { get; }
        public ProductionInfoPanel ProductionInfoPanel { get; }
    }
}