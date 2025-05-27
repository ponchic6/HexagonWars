using System.Collections.Generic;
using Code.Gameplay.Common;
using Code.Gameplay.Features.Migration.Services;
using Code.Infrastructure.View;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationTrailCreatingReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IMigrationFactory _migrationFactory;
        private readonly GameContext _game;

        public MigrationTrailCreatingReactiveSystem(IContext<GameEntity> context, IMigrationFactory migrationFactory) : base(context)
        {
            _migrationFactory = migrationFactory;
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.CitizenToggleEnabling.Added(), GameMatcher.SoldiersToggleEnabling.Added());

        protected override bool Filter(GameEntity entity) =>
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            DestructAllMigrationTrailsView();
            
            foreach (GameEntity entity in entities)
            {
                EntityBehaviour entityBehaviour = entity.view.Value;
                _migrationFactory.CreateMigrationViewTrail(entityBehaviour, entity.isCitizenToggleEnabling ? ManMigrationType.Citizens : ManMigrationType.Warriors);
            }
        }
        
        private void DestructAllMigrationTrailsView()
        {
            foreach (GameEntity trailEntity in _game.GetGroup(GameMatcher.MigrationArrow))
                trailEntity.isDestructed = true;
        }
    }
}