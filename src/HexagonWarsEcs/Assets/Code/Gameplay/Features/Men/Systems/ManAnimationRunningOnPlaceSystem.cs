using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationRunningOnPlaceSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public ManAnimationRunningOnPlaceSystem()
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
                
                if (currentHex.manAnimation.Value != ManAnimationType.Run && entity.migrationComplexityWay.Value.Count > 0)
                {
                    GameEntity nextHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);
                    Vector3 finishPoint = nextHex.transform.Value.TransformPoint(new Vector3(0, 0, 0.3f));
                    ManAnimationController animationController = currentHex.transform.Value.GetComponentInChildren<ManAnimationController>();
                    animationController.transform.LookAt(finishPoint);
                    animationController.StartRun();
                    currentHex.ReplaceManAnimation(ManAnimationType.Run);
                }
            }
        }
    }
}