using Code.Gameplay.Features.Building.View;
using Code.Infrastructure.View;

namespace Code.Gameplay.Common.Services
{
    public interface IUIFactory
    {
        public void ShowInfoPanel(EntityBehaviour entityBehaviour);
        public void HideInfoPanel(EntityBehaviour entityBehaviour);
        public HexagonInfoPanel HexagonInfoPanel { get; }
    }
}