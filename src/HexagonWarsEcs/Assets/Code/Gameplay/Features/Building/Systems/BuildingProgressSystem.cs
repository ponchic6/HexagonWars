using Code.Gameplay.Features.Building.DataStructure;
using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Building.Systems
{
    public class BuildingProgressSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;

        public BuildingProgressSystem(CommonStaticData commonStaticData)
        {
            GameContext game = Contexts.sharedInstance.game;
            _commonStaticData = commonStaticData;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.BuildingProgress, GameMatcher.CitizensAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                foreach (BuildProgressContainer buildProgressContainer in entity.buildingProgress.Value)
                {
                    if (buildProgressContainer.currentProgress < buildProgressContainer.fullProgress)
                    {
                        if (buildProgressContainer.buildersAmount != 0)
                        {
                            buildProgressContainer.currentProgress += Time.deltaTime * buildProgressContainer.buildersAmount;
                            entity.ReplaceBuildingProgress(entity.buildingProgress.Value);
                        }
                    }
                    else if (!buildProgressContainer.ready)
                    {
                        FinishBuildingProcess(buildProgressContainer, entity);
                        entity.ReplaceBuildingProgress(entity.buildingProgress.Value);
                    }
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