using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationButtonsVisibilityReactiveSystem : ReactiveSystem<GameEntity>
    {
        public MigrationButtonsVisibilityReactiveSystem(IContext<GameEntity> context) : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.AnyOf(
                    GameMatcher.ManAmount
                    ));

        protected override bool Filter(GameEntity entity) =>
            entity.isPlayerHexagon;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.manAmount.Value == 0)
                {
                    entity.migrationHandler.Value.ManButtonActive(false);
                    entity.isManToggleEnabling= false;
                }
                
                if (entity.manAmount.Value > 0)
                    entity.migrationHandler.Value.ManButtonActive(true);
            }
        }
    }
}