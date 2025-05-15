using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Production.Systems
{
    public class CitizenCreationSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public CitizenCreationSystem()
        {
            GameContext game = Contexts.sharedInstance.game;
            
            _entities = game.GetGroup(GameMatcher.City);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.city.CitizenOrdered == 0)
                {
                    entity.city.CurrentCooldown = entity.city.Cooldown;
                    continue;
                }

                if (entity.city.CurrentCooldown > 0) 
                    entity.city.CurrentCooldown -= Time.deltaTime;

                if (entity.city.CurrentCooldown <= 0)
                {
                    entity.city.CurrentCooldown = entity.city.Cooldown;
                    entity.city.CitizenOrdered--;
                    entity.ReplaceCitizensAmount(entity.citizensAmount.Value + 1);
                }
            }
        }
    }
}