using System.Collections.Generic;
using Entitas;

namespace Code.Gameplay.Features.Building.Systems
{
    public class BuildingProgressCleanupSystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private List<GameEntity> _buffer = new(128);

        public BuildingProgressCleanupSystem()
        {
            GameContext game = Contexts.sharedInstance.game;

            _entities = game.GetGroup(GameMatcher.BuildingProgress);
        }
        
        public void Cleanup()
        {
            foreach (GameEntity entity in _entities.GetEntities(_buffer))
            {
                if (entity.hasBuildingProgress && entity.buildingProgress.Value.Count == 0)
                {
                    entity.RemoveBuildingProgress();
                }
            }
        }
    }
}