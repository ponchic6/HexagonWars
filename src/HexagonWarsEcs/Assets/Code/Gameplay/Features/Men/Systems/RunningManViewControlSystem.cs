using Code.Gameplay.Features.Citizens.Services;
using Entitas;

namespace Code.Gameplay.Features.Men.Systems
{
    public class RunningManViewControlSystem : IExecuteSystem
    {
        private readonly IManModelFactory _manModelFactory;
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public RunningManViewControlSystem(IManModelFactory manModelFactory)
        {
            _manModelFactory = manModelFactory;
            _game = Contexts.sharedInstance.game;
            _entities = _game.GetGroup(GameMatcher.MigrationComplexityWay);
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.migrationComplexityWay.Value.Count == 0)
                    continue;
                
                if (entity.migrationComplexityWay.Value[0] > 0)
                    continue;
                
                GameEntity currentHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);
                GameEntity nextHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);
                
                if (nextHex.isEnemyHexagon)
                    continue;
                
                if (currentHex.manAmount.Value <= 0)
                    continue;
                
                _manModelFactory.CreateAndMoveCitizenModel(currentHex, nextHex);
            }
        }
    }
}