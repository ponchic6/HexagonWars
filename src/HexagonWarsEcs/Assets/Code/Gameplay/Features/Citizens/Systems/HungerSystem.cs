using Code.Infrastructure.StaticData;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class HungerSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;

        public HungerSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.FoodAmount, GameMatcher.CitizensAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.foodAmount.Value == 0)
                {
                    if (!entity.hasCurrentHungerDeathCooldown && !entity.hasMaxHungerDeathCooldown)
                    {
                        entity.AddCurrentHungerDeathCooldown(0f);
                        entity.AddMaxHungerDeathCooldown(_commonStaticData.HungerCooldown);
                    }
                
                    if (entity.currentHungerDeathCooldown.Value == 0) 
                        entity.ReplaceCurrentHungerDeathCooldown(entity.maxHungerDeathCooldown.Value);
                    
                    if (entity.currentHungerDeathCooldown.Value > 0) 
                        entity.ReplaceCurrentHungerDeathCooldown(entity.currentHungerDeathCooldown.Value - Time.deltaTime);
                    
                    if (entity.currentHungerDeathCooldown.Value < 0)
                        entity.ReplaceCurrentHungerDeathCooldown(0);
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