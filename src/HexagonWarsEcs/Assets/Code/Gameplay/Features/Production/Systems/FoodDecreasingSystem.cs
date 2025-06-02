using System;
using System.Linq;
using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Production.Systems
{
    public class FoodDecreasingSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;

        public FoodDecreasingSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AnyOf(
                GameMatcher.ManAmount,
                GameMatcher.BuildingProgress,
                GameMatcher.FoodFarm,
                GameMatcher.Mine));
        }
    
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (!entity.hasFoodAmount)
                    continue;
                
                float foodConsumption = CalculateFoodConsumption(entity);
                float newFoodAmount = Math.Max(0, entity.foodAmount.Value - foodConsumption);
                entity.ReplaceFoodAmount(newFoodAmount);
            }
        }
    
        private float CalculateFoodConsumption(GameEntity entity)
        {
            float consumption = 0f;
            float deltaTime = Time.deltaTime;
        
            if (entity.hasManAmount)
                consumption += entity.manAmount.Value * deltaTime * _commonStaticData.FoodPerSecondByCitizens;
            
            if (entity.hasFoodFarm)
                consumption += entity.foodFarm.Workers * deltaTime * _commonStaticData.FoodPerSecondByCitizens;
            
            if (entity.hasMine)
                consumption += entity.mine.Miners * deltaTime * _commonStaticData.FoodPerSecondByCitizens;
            
            if (entity.hasBuildingProgress)
            {
                int totalBuilders = entity.buildingProgress.Value.Where(x => !x.Ready).Sum(x => x.buildersAmount);
                
                consumption += totalBuilders * deltaTime * _commonStaticData.FoodPerSecondByBuilders;
            }
        
            return consumption;
        }
    }}