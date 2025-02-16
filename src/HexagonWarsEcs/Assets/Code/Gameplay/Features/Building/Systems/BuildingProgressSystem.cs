using System.Collections.Generic;
using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Building.Systems
{
    public class BuildingProgressSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(128);

        public BuildingProgressSystem(CommonStaticData commonStaticData)
        {
            GameContext game = Contexts.sharedInstance.game;
            _commonStaticData = commonStaticData;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.BuildingProgress, GameMatcher.CitizensAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                foreach (BuildProgressContainer buildProgressContainer in entity.buildingProgress.Value)
                {
                    if (buildProgressContainer.currentProgress < buildProgressContainer.fullProgress)
                        buildProgressContainer.currentProgress += Time.deltaTime * buildProgressContainer.buildersAmount;
                    else if (!buildProgressContainer.ready)
                        FinishBuildingProcess(buildProgressContainer, entity);
                }
            }
        }

        private void FinishBuildingProcess(BuildProgressContainer buildProgressContainer, GameEntity entity)
        {
            AddBuilding(buildProgressContainer, entity);
            entity.ReplaceCitizensAmount(buildProgressContainer.buildersAmount + entity.citizensAmount.Value);
            buildProgressContainer.buildersAmount = 0;
            buildProgressContainer.ready = true;
        }
        
        private void AddBuilding(BuildProgressContainer buildProgressContainer, GameEntity entity)
        {
            switch (buildProgressContainer.buildingType)
            {
                case BuildingsType.LivingArea:
                    entity.isLivingArea = true;
                    break;
                
                case BuildingsType.FoodFarm:
                    entity.AddFoodFarm(0);
                    break;
                
                case BuildingsType.Barracks:
                    entity.AddBarracks(0, _commonStaticData.WarriorTrainingTime, _commonStaticData.WarriorTrainingTime);
                    break;
            }
        }
    }
}