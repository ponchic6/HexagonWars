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

                if (defenderEntity.warriorsAmount.Value <= 0)
                {
                    defenderEntity.isEnemyHexagon = false;
                    defenderEntity.isPlayerHexagon = true;
                    
                    entity.battlefield.AttackerHexagonsId.ForEach(x =>
                    {
                        GameEntity attackerEntity = _game.GetEntityWithId(x);
                        defenderEntity.warriorsAmount.Value += attackerEntity.warriorsAmount.Value;
                        attackerEntity.warriorsAmount.Value = 0;
                    });
                    entity.isDestructed = true;
                }

                if (entity.battlefield.AttackerHexagonsId.All(x => _game.GetEntityWithId(x).warriorsAmount.Value <= 0))
                    entity.isDestructed = true;
            }
        }
    }
}