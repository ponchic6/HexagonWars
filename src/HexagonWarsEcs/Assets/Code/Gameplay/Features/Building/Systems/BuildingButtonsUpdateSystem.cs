﻿using System.Collections.Generic;
using Code.Gameplay.Common.Services;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Gameplay.Features.Building.View;
using Entitas;

namespace Code.Gameplay.Features.Building.Systems
{
    public class BuildingButtonsUpdateSystem : IExecuteSystem
    {
        private readonly IUIFactory _uiFactory;
        private readonly IGroup<GameEntity> _entities;

        public BuildingButtonsUpdateSystem(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.BuildingProgress);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                BuildingInfoPanel infoPanel = _uiFactory.BuildingInfoPanel;
                
                if (infoPanel == null || !infoPanel.gameObject.activeSelf)
                    continue;

                if (infoPanel.EntityBehaviour.Entity.id.Value != entity.id.Value)
                    continue;

                infoPanel.UpdateBuildersStates(entity);
            }
        }
    }
}