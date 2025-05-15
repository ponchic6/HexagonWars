using System.Collections.Generic;
using Code.Gameplay.Features.Battle.Services;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattlefieldFromMigrationReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IBattleFieldFactory _battleFieldFactory;
        private GameContext _game;

        public BattlefieldFromMigrationReactiveSystem(IContext<GameEntity> context, IBattleFieldFactory battleFieldFactory) : base(context)
        {
            _battleFieldFactory = battleFieldFactory;

            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.Destructed);

        protected override bool Filter(GameEntity entity) =>
            entity.hasWarriorsMigrationAmount && entity.hasHexagonForAttack;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                if (_game.GetEntityWithId(entity.hexagonForAttack.Value).isPlayerHexagon)
                    continue;
                
                _battleFieldFactory.CreateBattlefieldFromWarriorsMigration(entity);
            }
        }
    }
}