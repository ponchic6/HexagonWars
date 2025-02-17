using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleStopSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private GameContext _game;

        public BattleStopSystem()
        {
            _game = Contexts.sharedInstance.game;
            
            _entities = _game.GetGroup(GameMatcher.Battlefield);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonContainer.hexagonId);
                    
                    entity.battlefield.AttackerHexagonContainers.
                        ForEach(x => _game.GetEntityWithId(x.hexagonId).warriorsAmount.Value += x.warriorsCount); 
                    defenderEntity.warriorsAmount.Value += entity.battlefield.DefenderHexagonContainer.warriorsCount;
                    
                    entity.isDestructed = true;
                }
            }
        }
    }
}