using Code.Gameplay.Features.Building.DataStructure;
using Code.Gameplay.Features.Production;
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
                    if (buildProgressContainer.currentProgress >= buildProgressContainer.fullProgress)
                        continue;
                    
                    if (buildProgressContainer.buildersAmount == 0)
                        continue;
                    
                    buildProgressContainer.currentProgress += Time.deltaTime * buildProgressContainer.buildersAmount;
                    
                    if (buildProgressContainer.currentProgress >= buildProgressContainer.fullProgress)
                    {
                        FinishBuildingProcess(buildProgressContainer, entity);
                        entity.ReplaceBuildingProgress(entity.buildingProgress.Value);
                    }
                    
                    entity.ReplaceBuildingProgress(entity.buildingProgress.Value);
                }
            }
        }
        private void FinishBuildingProcess(BuildProgressContainer buildProgressContainer, GameEntity entity)
        {
            AddBuilding(buildProgressContainer, entity);
            entity.ReplaceCitizensAmount(buildProgressContainer.buildersAmount + entity.citizensAmount.Value);
            buildProgressContainer.buildersAmount = 0;
        }
        
        private void AddBuilding(BuildProgressContainer buildProgressContainer, GameEntity entity)
        {
            switch (buildProgressContainer.buildingType)
            {
                case BuildingsType.City:
                    entity.AddCity(0, _commonStaticData.CitizenCreationTime, _commonStaticData.CitizenCreationTime);
                    break;
                
                case BuildingsType.FoodFarm:
                    entity.AddFoodFarm(0);
                    break;
                
                case BuildingsType.Barracks:
                    entity.AddBarracks(0, _commonStaticData.WarriorTrainingTime, _commonStaticData.WarriorTrainingTime);
                    break;
                
                case BuildingsType.Mine:
                    entity.AddMine(0, OreType.Iron);
                    break;
            }
        }
    }
}