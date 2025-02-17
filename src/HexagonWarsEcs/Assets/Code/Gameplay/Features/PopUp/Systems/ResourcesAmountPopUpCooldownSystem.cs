using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.PopUp.Systems
{
    public class ResourcesAmountPopUpCooldownSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public ResourcesAmountPopUpCooldownSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AmountPopUpCooldown);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.amountPopUpCooldown.RemainingTime == 0)
                    entity.amountPopUpCooldown.RemainingTime = entity.amountPopUpCooldown.Cooldown;
                
                if (entity.amountPopUpCooldown.RemainingTime > 0) 
                    entity.amountPopUpCooldown.RemainingTime -= Time.deltaTime;

                if (entity.amountPopUpCooldown.RemainingTime < 0)
                    entity.amountPopUpCooldown.RemainingTime = 0;
            }
        }
    }
}