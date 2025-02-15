using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class DeathCitizensByHungerSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public DeathCitizensByHungerSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.CitizensAmount, GameMatcher.CurrentHungerDeathCooldown));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.foodAmount.Value == 0 && entity.currentHungerDeathCooldown.Value >= entity.maxHungerDeathCooldown.Value)
                {
                    entity.ReplaceCitizensAmount((int)(entity.citizensAmount.Value * 0.9f));
                }
            }
        }
    }
}