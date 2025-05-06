using System.Collections.Generic;
using Code.Gameplay.Common.Services;
using Code.Gameplay.Features.Building.View;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Building.Systems
{
    public class BuildingButtonUpdateReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIFactory _uiFactory;

        public BuildingButtonUpdateReactiveSystem(IContext<GameEntity> context, IUIFactory uiFactory) : base(context)
        {
            _uiFactory = uiFactory;
        }
        
        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.BuildingProgress);

        protected override bool Filter(GameEntity entity) =>
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                BuildingInfoPanel infoPanel = _uiFactory.BuildingInfoPanel;
                
                if (infoPanel == null || !infoPanel.gameObject.activeSelf)
                    continue;

                if (infoPanel.HexEntityBehaviour.Entity.id.Value != entity.id.Value)
                    continue;

                infoPanel.UpdateBuildingProgress();
            }
        }
    }
}