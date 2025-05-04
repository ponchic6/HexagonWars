using Code.Gameplay.Common.Services;
using Code.Gameplay.Features.Production.View.UI;
using Entitas;

namespace Code.Gameplay.Features.Production.Systems
{
    public class ProductionUiViewUpdateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly IUIFactory _uiFactory;

        public ProductionUiViewUpdateSystem(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.BuildingProgress);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                ProductionInfoPanel productionInfoPanel = _uiFactory.ProductionInfoPanel;
                
                if (productionInfoPanel == null || !productionInfoPanel.gameObject.activeSelf)
                    continue;

                if (productionInfoPanel.HexEntityBehaviour.Entity.id.Value != entity.id.Value)
                    continue;

                productionInfoPanel.UpdateProductionUi();
            }
        }
    }
}