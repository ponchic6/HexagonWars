using Code.Gameplay.Features.Building.View;
using Code.Gameplay.Features.Logistics.View.UI;
using Code.Infrastructure.View;
using Zenject;

namespace Code.Gameplay.Common.Services
{
    public class UIFactory : IUIFactory
    {
        private const string HEX_RESOURCES_INFO_PANEL_PATH = "Hexagons/UI/HexagonInfoPanel";
        private const string SUPPLY_ROUTE_INFO_PANEL_PATH = "Hexagons/UI/SupplyRouteInfoPanel";

        private HexagonInfoPanel _hexagonInfoPanel;
        private SupplyRoutInfoPanel _supplyRoutInfoPanel;
        private DiContainer _diContainer;
        
        public HexagonInfoPanel HexagonInfoPanel => _hexagonInfoPanel;

        public UIFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void ShowInfoPanel(EntityBehaviour entityBehaviour)
        {
            if (entityBehaviour.Entity.isChildHexagon)
            {
                if (_hexagonInfoPanel == null)
                    _hexagonInfoPanel = _diContainer.InstantiatePrefabResourceForComponent<HexagonInfoPanel>(HEX_RESOURCES_INFO_PANEL_PATH);
                else
                    _hexagonInfoPanel.gameObject.SetActive(true);

                _hexagonInfoPanel.Setup(entityBehaviour);
                return;
            }

            if (entityBehaviour.Entity.isSupplyRoute)
            {
                if (_supplyRoutInfoPanel == null)
                    _supplyRoutInfoPanel = _diContainer.InstantiatePrefabResourceForComponent<SupplyRoutInfoPanel>(SUPPLY_ROUTE_INFO_PANEL_PATH);
                else
                    _supplyRoutInfoPanel.gameObject.SetActive(true);

                _supplyRoutInfoPanel.Setup(entityBehaviour);
                return;
            }
        }

        public void HideInfoPanel(EntityBehaviour entityBehaviour)
        {
            if (entityBehaviour.Entity.isChildHexagon)
            {
                if (_hexagonInfoPanel != null) 
                    _hexagonInfoPanel.gameObject.SetActive(false);
            }

            if (entityBehaviour.Entity.isSupplyRoute)
            {
                if (_supplyRoutInfoPanel != null) 
                    _supplyRoutInfoPanel.gameObject.SetActive(false);
            }
        }
    }
}