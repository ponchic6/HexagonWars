using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationTogglesCountPeopleReactiveSystem : ReactiveSystem<GameEntity>
    {
        public MigrationTogglesCountPeopleReactiveSystem(IContext<GameEntity> context) : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.CitizensAmount.Added(), GameMatcher.WarriorsAmount.Added());

        protected override bool Filter(GameEntity entity) =>
            entity.hasMigrationHandler;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                entity.migrationHandler.Value.UpdateCitizenAndWarriorsCountsView();
            }
        }
    }
}