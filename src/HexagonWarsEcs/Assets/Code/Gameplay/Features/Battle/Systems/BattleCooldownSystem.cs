using System.Linq;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleCooldownSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public BattleCooldownSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.AllOf(GameMatcher.CurrentBattleCooldown, GameMatcher.BattleCooldown));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.currentBattleCooldown.Value == 0) 
                    entity.ReplaceCurrentBattleCooldown(entity.battleCooldown.Value);
                
                if (entity.currentBattleCooldown.Value > 0) 
                    entity.ReplaceCurrentBattleCooldown(entity.currentBattleCooldown.Value - Time.deltaTime);

                if (entity.currentBattleCooldown.Value < 0) 
                    entity.ReplaceCurrentBattleCooldown(0);
            }
        }
    }
}