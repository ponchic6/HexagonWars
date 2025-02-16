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
                if (!_prodictionHandlers.TryGetValue(build.buildingType, out ProductionHandler handler)) 
                    CreateProductionHandler(entity, build);
                else
                    UpdateProductionHandlerState(build.buildingType, handler, entity);
            }
        }

        private void CreateProductionHandler(GameEntity entity, BuildProgressContainer build)
        {
            var productionHandler = Instantiate(_productionHandlerPrefab, _content);
            productionHandler.Setup(entity, build);
            _prodictionHandlers.Add(build.buildingType, productionHandler);

            switch (build.buildingType)
            {
                case BuildingsType.FoodFarm:
                    productionHandler.OnSliderValueChanged += x => OnSliderValueChanged(x, build.buildingType);
                    break;
                
                case BuildingsType.Barracks:
                    productionHandler.OnInputFieldSubmit += x => OnInputFieldSubmit(x, build.buildingType);
                    break;
            }
        }

        private void UpdateProductionHandlerState(BuildingsType buildingsType, ProductionHandler productionHandler, GameEntity entity)
        {
            switch (buildingsType)
            {
                case BuildingsType.Barracks:
                    if (!productionHandler.InputField.isFocused) 
                        productionHandler.InputField.text = entity.barracks.WarriorsOrdered.ToString();
                    break;
            }
        }

        private void OnInputFieldSubmit(int input, BuildingsType buildBuildingType)
        {
            switch (buildBuildingType)
            {
                case BuildingsType.Barracks:
                    _entityBehaviour.Entity.barracks.WarriorsOrdered = input;
                    break;
            }
        }

        private void OnSliderValueChanged(float sliderValue, BuildingsType buildingType)
        {
            switch (buildingType)
            {
                case BuildingsType.FoodFarm:
                    var totalAmount = _entityBehaviour.Entity.foodFarm.Workers + _entityBehaviour.Entity.citizensAmount.Value;

                    _entityBehaviour.Entity.foodFarm.Workers = (int)Math.Round(totalAmount * sliderValue);
                    _entityBehaviour.Entity.ReplaceCitizensAmount(totalAmount - _entityBehaviour.Entity.foodFarm.Workers);
                    break;
            }
        }
    }
}