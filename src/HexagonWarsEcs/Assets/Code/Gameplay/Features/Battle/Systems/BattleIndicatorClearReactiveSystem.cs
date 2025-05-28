using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleIndicatorClearReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _game;

        public BattleIndicatorClearReactiveSystem(IContext<GameEntity> context) : base(context)
        {
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.BattleIndicator);

        protected override bool Filter(GameEntity entity) =>
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                int battleId = entity.battleIndicator.BattleId;
                GameEntity battleEntity = _game.GetEntityWithId(battleId);

                if (battleEntity != null && !battleEntity.isDestructed)
                    continue;
                
                entity.isDestructed = true;

            }
        }
    }
}