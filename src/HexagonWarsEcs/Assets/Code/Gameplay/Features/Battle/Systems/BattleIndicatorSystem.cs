using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleIndicatorSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public BattleIndicatorSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.BattleIndicator);
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (!entity.hasBattleIndicatorController)
                    continue;
                
                GameEntity fromHex = _game.GetEntityWithId(entity.battleIndicator.FromHexId);
                GameEntity toHex = _game.GetEntityWithId(entity.battleIndicator.ToHexId);

                int totalWarriors = fromHex.manAmount.Value + toHex.manAmount.Value;
                entity.battleIndicator.WinIndicator = fromHex.manAmount.Value / (float)totalWarriors;
                entity.ReplaceBattleIndicator(
                    entity.battleIndicator.FromHexId,
                    entity.battleIndicator.ToHexId,
                    entity.battleIndicator.WinIndicator,
                    entity.battleIndicator.BattleId);

                entity
                    .battleIndicatorController
                    .Controller
                    .SetBattleStatus(entity);
            }
        }
    }
}
