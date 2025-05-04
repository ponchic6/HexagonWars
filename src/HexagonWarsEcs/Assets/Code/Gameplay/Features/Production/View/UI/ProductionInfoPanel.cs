using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Features.Building;
using Code.Gameplay.Features.Building.View;
using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Production.View.UI
{
    public class ProductionInfoPanel : MonoBehaviour
    {
        [SerializeField] private BuildingInfoPanel _buildingInfoPanel;
        [SerializeField] private RectTransform _content;
        
        private Dictionary<BuildingsType, ProductionHandler> _prodictionHandlers = new();
        private EntityBehaviour _hexEntityBehaviour;

        public EntityBehaviour HexEntityBehaviour => _hexEntityBehaviour;

        public void Setup(EntityBehaviour entityBehaviour)
        {
            _hexEntityBehaviour = entityBehaviour;
            _prodictionHandlers.Clear();

            foreach (BuildingButton buildingButton in _buildingInfoPanel.BuildingButtons.Values.ToList())
            {
                ProductionHandler productionHandler =
                    buildingButton.GetComponentInChildren<ProductionHandler>(true);
                _prodictionHandlers
                    .Add(buildingButton.BuildingProgressContainer.buildingType,
                        productionHandler);
            }
        }

        public void UpdateProductionUi()
        {
            foreach (ProductionHandler productionHandler in _prodictionHandlers.Values)
                productionHandler.UpdateProductionUi();
        }
    }
}