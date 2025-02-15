using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class HungerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public HungerSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.FoodAmount, GameMatcher.CitizensAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.foodAmount.Value == 0 && entity.citizensAmount.Value > 0)
                {
                    if (!entity.hasCurrentHungerDeathCooldown && !entity.hasMaxHungerDeathCooldown)
                    {
                        entity.AddCurrentHungerDeathCooldown(0f);
                        entity.AddMaxHungerDeathCooldown(3f);
                    }
                
                    if (entity.currentHungerDeathCooldown.Value >= entity.maxHungerDeathCooldown.Value)
                    {
                        entity.ReplaceCurrentHungerDeathCooldown(0f);
                    }
                    
                    entity.ReplaceCurrentHungerDeathCooldown(entity.currentHungerDeathCooldown.Value + Time.deltaTime);
                }
                else
                {
                    if (entity.hasCurrentHungerDeathCooldown && entity.hasMaxHungerDeathCooldown)
                    {
                        entity.RemoveCurrentHungerDeathCooldown();
                        entity.RemoveMaxHungerDeathCooldown();
                    }
                }
            }
        }
    }
}