using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class PaintingInFractionColorSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;

        public PaintingInFractionColorSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.ChildHexagon);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.isPlayerHexagon) 
                    entity.renderer.Value.material.SetFloat("_FillAmount", 0.4f);
                
                if (entity.isEnemyHexagon) 
                    entity.renderer.Value.material.SetFloat("_FillAmount", -0.4f);
            }
        }
    }
}