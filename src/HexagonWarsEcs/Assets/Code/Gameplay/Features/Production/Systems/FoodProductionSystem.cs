using System.Collections.Generic;
using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Production.Systems
{
    public class FoodProductionSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(128);

        public FoodProductionSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.FoodAmount, GameMatcher.FoodFarm));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer)) 
                entity.ReplaceFoodAmount(entity.foodAmount.Value + entity.foodFarm.Workers * Time.deltaTime * _commonStaticData.FoodPerformancePerSecond);
        }
    }
}