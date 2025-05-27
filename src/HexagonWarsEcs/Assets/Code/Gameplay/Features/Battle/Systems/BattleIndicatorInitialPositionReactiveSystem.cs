using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleIndicatorInitialPositionReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _game;

        public BattleIndicatorInitialPositionReactiveSystem(IContext<GameEntity> context) : base(context) =>
            _game = Contexts.sharedInstance.game;

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.BattleIndicatorController.Added());

        protected override bool Filter(GameEntity entity) =>
            true;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                GameEntity fromHex = _game.GetEntityWithId(entity.battleIndicator.FromHexId);
                GameEntity toHex = _game.GetEntityWithId(entity.battleIndicator.ToHexId);

                entity
                    .battleIndicatorController
                    .Controller
                    .SetPosition(fromHex.transform.Value, toHex.transform.Value);
                
                entity
                    .battleIndicatorController
                    .Controller
                    .SetDirection(fromHex.transform.Value, toHex.transform.Value);
            }
        }
    }
}