using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationIdleOnPlaceSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public ManAnimationIdleOnPlaceSystem()
        {
            _game = Contexts.sharedInstance.game;
            _entities = _game.GetGroup(GameMatcher.AllOf(GameMatcher.MigrationComplexityWay, GameMatcher.ManMigrationAmount));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                GameEntity currentHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);

                if (entity.migrationComplexityWay.Value.Count > 0 && _game.GetEntityWithId(entity.wayIdPoints.Value[1]).isEnemyHexagon)
                    continue;
                
                if (entity.migrationComplexityWay.Value.Count > 0 && entity.migrationComplexityWay.Value[0] < 0)
                {
                    ManAnimationController animationController = currentHex.transform.Value.GetComponentInChildren<ManAnimationController>();
                    animationController.StartIdle();
                    animationController.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    currentHex.ReplaceManAnimation(ManAnimationType.Idle);
                }
            }
        }
    }
}