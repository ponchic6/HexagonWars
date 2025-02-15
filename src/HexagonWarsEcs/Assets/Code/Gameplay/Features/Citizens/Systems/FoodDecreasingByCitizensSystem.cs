using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class FoodDecreasingByCitizensSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public FoodDecreasingByCitizensSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.CitizensAmount, GameMatcher.FoodAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.foodAmount.Value <= 0)
                    entity.ReplaceFoodAmount(0);
                else
                    entity.ReplaceFoodAmount(entity.foodAmount.Value - entity.citizensAmount.Value * Time.deltaTime * 0.1f);
            }
        }
    }
}