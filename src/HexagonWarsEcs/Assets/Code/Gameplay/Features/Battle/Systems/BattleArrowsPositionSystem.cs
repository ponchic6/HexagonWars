using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class BattleArrowsPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public BattleArrowsPositionSystem()
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(GameMatcher.BattleArrow, GameMatcher.LineRenderer, GameMatcher.WayIdPoints));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                var vec1 = _game.GetEntityWithId(entity.wayIdPoints.Value[0]).transform.Value.position;
                vec1.y = 0.5f;
                var vec2 = _game.GetEntityWithId(entity.wayIdPoints.Value[1]).transform.Value.position;
                vec2.y = 0.5f;
                entity.lineRenderer.Value.SetPosition(0, vec1);
                entity.lineRenderer.Value.SetPosition(1, vec2);
            }
        }
    }
}