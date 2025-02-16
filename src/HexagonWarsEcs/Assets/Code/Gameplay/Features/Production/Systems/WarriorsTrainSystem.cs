using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Production.Systems
{
    public class WarriorsTrainSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public WarriorsTrainSystem()
        {
            GameContext game = Contexts.sharedInstance.game;
            
            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.CitizensAmount, GameMatcher.Barracks));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.barracks.WarriorsOrdered == 0)
                {
                    entity.barracks.CurrentCooldown = entity.barracks.Cooldown;
                    continue;
                }

                if (entity.barracks.CurrentCooldown > 0) 
                    entity.barracks.CurrentCooldown -= Time.deltaTime;

                if (entity.barracks.CurrentCooldown <= 0)
                {
                    entity.barracks.CurrentCooldown = entity.barracks.Cooldown;
                    entity.barracks.WarriorsOrdered--;
                    entity.citizensAmount.Value--;
                    entity.warriorsAmount.Value++;
                }
            }
        }
    }
}