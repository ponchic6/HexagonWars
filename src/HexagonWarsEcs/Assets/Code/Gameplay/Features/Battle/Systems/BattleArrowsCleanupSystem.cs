using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleArrowsCleanupSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public BattleArrowsCleanupSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.BattleArrow);
        }
        
        public void Cleanup()
        {
            foreach (GameEntity entity in _entities)
            {
                if (_game.GetEntityWithId(entity.battleArrow.BattlefieldId).isDestructed) 
                    entity.isDestructed = true;
            }
        }
    }
}