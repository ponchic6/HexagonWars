using System;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationTrailClearSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public MigrationTrailClearSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.MigrationArrow);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                GameEntity migrationEntity = _game.GetEntityWithId(entity.migrationArrow.MigrationId);

                if (migrationEntity.isDestructed) 
                    entity.isDestructed = true;
            }
        }
    }
}