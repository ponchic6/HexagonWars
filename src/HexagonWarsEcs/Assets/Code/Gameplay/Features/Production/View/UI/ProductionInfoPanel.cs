using System;
using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Features.Building;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.View;
using UnityEngine;

namespace Code.Gameplay.Features.Production.View.UI
{
    public class ProductionInfoPanel : MonoBehaviour
    {
        [SerializeField] private ProductionHandler _productionHandlerPrefab;
        [SerializeField] private RectTransform _content;
        
        private Dictionary<BuildingsType, ProductionHandler> _prodictionHandlers = new();
        private EntityBehaviour _entityBehaviour;

        public EntityBehaviour EntityBehaviour => _entityBehaviour;

        public void Setup(EntityBehaviour entityBehaviour)
        {
            foreach (var kvp in _prodictionHandlers)
            {
                Destroy(kvp.Value.gameObject);
            }
            
            _prodictionHandlers.Clear();
            _entityBehaviour = entityBehaviour;
        }

        public void UpdateProductionUi(GameEntity entity)
        {
            foreach (BuildProgressContainer build in entity.buildingProgress.Value.Where(x => x.ready))
            {
                if (!_prodictionHandlers.ContainsKey(build.buildingType)) 
                    CreateProductionHandler(entity, build);
            }
        }

        private void CreateProductionHandler(GameEntity entity, BuildProgressContainer build)
        {
            var productionHandler = Instantiate(_productionHandlerPrefab, _content);
            productionHandler.Setup(entity, build);
            _prodictionHandlers.Add(build.buildingType, productionHandler);
            
            productionHandler.OnSliderValueChanged += x => UpdateWorkersAmount(x, build.buildingType);
        }

        private void UpdateWorkersAmount(float sliderValue, BuildingsType buildingType)
        {
            int totalAmount = 0;
            
            switch (buildingType)
            {
                case BuildingsType.FoodFarm:
                    totalAmount = _entityBehaviour.Entity.foodFarm.Workers +
                                  _entityBehaviour.Entity.citizensAmount.Value;
                    
                    _entityBehaviour.Entity.foodFarm.Workers = (int)Math.Round(totalAmount * sliderValue);
                    _entityBehaviour.Entity.ReplaceCitizensAmount(totalAmount - _entityBehaviour.Entity.foodFarm.Workers);
                    break;
            }
        }
    }
}