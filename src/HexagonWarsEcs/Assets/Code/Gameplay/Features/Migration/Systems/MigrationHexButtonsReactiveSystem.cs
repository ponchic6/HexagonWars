using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationHexButtonsReactiveSystem : ReactiveSystem<GameEntity>
    {
        public MigrationHexButtonsReactiveSystem(IContext<GameEntity> context) : base(context)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.AnyOf(
                    GameMatcher.CitizensAmount,
                    GameMatcher.WarriorsAmount
                    ));

        protected override bool Filter(GameEntity entity) =>
            entity.isPlayerHexagon;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (entity.citizensAmount.Value == 0)
                    entity.migrationHandler.Value.CitizenButtonActive(false);
                
                if (entity.citizensAmount.Value > 0)
                    entity.migrationHandler.Value.CitizenButtonActive(true);
                
                if (entity.warriorsAmount.Value == 0)
                    entity.migrationHandler.Value.WarriorButtonActive(false);

                if (entity.warriorsAmount.Value > 0)
                    entity.migrationHandler.Value.WarriorButtonActive(true);

            }
        }
    }
}