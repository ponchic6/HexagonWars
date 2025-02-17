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
                GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonContainer.hexagonId);

                if (entity.battlefield.DefenderHexagonContainer.warriorsCount <= 0)
                {
                    defenderEntity.isEnemyHexagon = false;
                    defenderEntity.isPlayerHexagon = true;
                    entity.battlefield.AttackerHexagonContainers.ForEach(x => defenderEntity.warriorsAmount.Value += x.warriorsCount);
                    entity.isDestructed = true;
                }
                
                if (entity.battlefield.AttackerHexagonContainers.All(x => x.warriorsCount <= 0))
                {
                    defenderEntity.warriorsAmount.Value += entity.battlefield.DefenderHexagonContainer.warriorsCount;
                    entity.isDestructed = true;
                }
            }
        }
    }
}