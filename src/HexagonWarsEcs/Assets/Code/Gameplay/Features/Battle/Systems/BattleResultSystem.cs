using System.Linq;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleResultSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public BattleResultSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.Battlefield);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonId);
                GameEntity attackerEntity = _game.GetEntityWithId(entity.battlefield.AttackerHexagonId);

                if (defenderEntity.isPlayerHexagon && attackerEntity.isPlayerHexagon ||
                    defenderEntity.isEnemyHexagon && attackerEntity.isEnemyHexagon)
                {
                    entity.isDestructed = true;
                    continue;
                }

                if (defenderEntity.warriorsAmount.Value <= 0)
                {
                    if (defenderEntity.isEnemyHexagon)
                    {
                        defenderEntity.isEnemyHexagon = false;
                        defenderEntity.isPlayerHexagon = true;
                    }
                    else if (defenderEntity.isPlayerHexagon)
                    {
                        defenderEntity.isEnemyHexagon = true;
                        defenderEntity.isPlayerHexagon = false;
                    }

                    
                    //defenderEntity.ReplaceWarriorsAmount(defenderEntity.warriorsAmount.Value + attackerEntity.warriorsAmount.Value);
                    //attackerEntity.ReplaceWarriorsAmount(0);
                    entity.isDestructed = true;
                }

                if (attackerEntity.warriorsAmount.Value <= 0)
                    entity.isDestructed = true;
            }
        }
    }
}