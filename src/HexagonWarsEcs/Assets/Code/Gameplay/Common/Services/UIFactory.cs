using Code.Gameplay.Features.Building.View;
using Code.Gameplay.Features.Logistics.View.UI;
using Code.Gameplay.Features.Production.View.UI;
using Code.Infrastructure.View;
using Zenject;

namespace Code.Gameplay.Common.Services
{
    public class UIFactory : IUIFactory
    {
        private const string HEX_RESOURCES_INFO_PANEL_PATH = "Hexagons/UI/HexagonInfoPanel";
        private const string SUPPLY_ROUTE_INFO_PANEL_PATH = "Hexagons/UI/SupplyRouteInfoPanel";

        private BuildingInfoPanel _buildingInfoPanel;
        private ProductionInfoPanel _productionInfoPanel;
        private SupplyRoutInfoPanel _supplyRoutInfoPanel;
        private DiContainer _diContainer;

        public BuildingInfoPanel BuildingInfoPanel => _buildingInfoPanel;
        public ProductionInfoPanel ProductionInfoPanel => _productionInfoPanel;

        public UIFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void ShowInfoPanel(EntityBehaviour entityBehaviour)
        {
            if (entityBehaviour.Entity.isChildHexagon)
            {
                if (_buildingInfoPanel == null)
                {
                    _buildingInfoPanel = _diContainer.InstantiatePrefabResourceForComponent<BuildingInfoPanel>(HEX_RESOURCES_INFO_PANEL_PATH);
                    _productionInfoPanel = _buildingInfoPanel.GetComponent<ProductionInfoPanel>();
                }
                else
                    _buildingInfoPanel.gameObject.SetActive(true);

                _buildingInfoPanel.Setup(entityBehaviour);
                _productionInfoPanel.Setup(entityBehaviour);
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
                if (_buildingInfoPanel != null) 
                    _buildingInfoPanel.gameObject.SetActive(false);
            }

            if (entityBehaviour.Entity.isSupplyRoute)
            {
                if (_supplyRoutInfoPanel != null) 
                    _supplyRoutInfoPanel.gameObject.SetActive(false);
            }
        }
    }
}