using System.Collections.Generic;
using Code.Gameplay.Features.Battle.Services;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattlefieldFromMigrationReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly IBattleFieldFactory _battleFieldFactory;
        private readonly GameContext _game;

        public BattlefieldFromMigrationReactiveSystem(IContext<GameEntity> context, IBattleFieldFactory battleFieldFactory) : base(context)
        {
            _battleFieldFactory = battleFieldFactory;

            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.MigrationComplexityWay.AddedOrRemoved());

        protected override bool Filter(GameEntity entity)
        {
            if (!entity.hasWayIdPoints)
                return false;
            
            if (entity.wayIdPoints.Value.Count < 2)
                return false;
            
            GameEntity nextHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);
            return entity.hasWarriorsMigrationAmount && nextHex != null && nextHex.isEnemyHexagon;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                IGroup<GameEntity> battlefield = _game.GetGroup(GameMatcher.Battlefield);

                foreach (GameEntity battleEntity in battlefield)
                {
                    if (battleEntity.battlefield.AttackerHexagonId == entity.wayIdPoints.Value[0])
                        return;
                    
                    if (battleEntity.battlefield.DefenderHexagonId == entity.wayIdPoints.Value[0])
                        return;
                }

                GameEntity defenderHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);
                GameEntity attackerHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);

                _battleFieldFactory.CreateBattlefield(attackerHex, defenderHex);
            }
        }
    }
}