using System.Linq;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class FoodDecreasingByBuildersSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public FoodDecreasingByBuildersSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.BuildingProgress);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.foodAmount.Value <= 0)
                    entity.ReplaceFoodAmount(0);
                else
                {
                    int total = entity.buildingProgress.Value.Where(x => !x.ready).Sum(x => x.buildersAmount);
                    entity.ReplaceFoodAmount(entity.foodAmount.Value - total * Time.deltaTime * 0.1f);
                }
            }
        }
    }
}