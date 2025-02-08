using Entitas;

namespace Code.Gameplay.Features.Migration.Systems
{
    public class MigrationArrowsPositionSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _arrows;

        public MigrationArrowsPositionSystem()
        {
            _game = Contexts.sharedInstance.game;
            
            _arrows = _game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MigrationArrow,
                GameMatcher.LineRenderer,
                GameMatcher.View,
                GameMatcher.MigrationWayIdPoints));
        }
        
        public void Execute()
        {
            foreach (GameEntity arrow in _arrows)
            {
                if (arrow.migrationWayIdPoints.Value.Count <= 1) 
                    continue;
                
                var vec1 = _game.GetEntityWithId(arrow.migrationWayIdPoints.Value[0]).transform.Value.position;
                vec1.y = 0.5f;
                var vec2 = _game.GetEntityWithId(arrow.migrationWayIdPoints.Value[1]).transform.Value.position;
                vec2.y = 0.5f;
                arrow.lineRenderer.Value.SetPosition(0, vec1);
                arrow.lineRenderer.Value.SetPosition(1, vec2);
            }
        }
    }
}