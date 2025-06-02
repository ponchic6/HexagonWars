using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationEnemyButtonsVisibilityReactiveSystem : ReactiveSystem<GameEntity>
    {
        public MigrationEnemyButtonsVisibilityReactiveSystem(IContext<GameEntity> context) : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.EnemyHexagon.AddedOrRemoved());

        protected override bool Filter(GameEntity entity) =>
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.isEnemyHexagon)
                {
                    entity.migrationHandler.Value.ManButtonActive(false);
                }
                else
                {
                    if (entity.manAmount.Value == 0)
                        entity.migrationHandler.Value.ManButtonActive(false);
                
                    if (entity.manAmount.Value > 0)
                        entity.migrationHandler.Value.ManButtonActive(true);
                }
            }
        }
    }
}