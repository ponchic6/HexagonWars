using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationRunningOnPlaceReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _game;

        public ManAnimationRunningOnPlaceReactiveSystem(IContext<GameEntity> context) : base(context)
        {
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.MigrationComplexityWay);

        protected override bool Filter(GameEntity entity)
        {
            return entity.migrationComplexityWay.Value.Count > 0 &&
                   entity.migrationComplexityWay.Value[0] > 0 &&
                   !_game.GetEntityWithId(entity.wayIdPoints.Value[1]).isEnemyHexagon;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                GameEntity currentHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);
                GameEntity nextHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);
                
                if (!currentHex.hasManAnimation || currentHex.manAnimation.Value == ManAnimationType.Run)
                    continue;
                    
                Vector3 finishPoint = nextHex.transform.Value.TransformPoint(new Vector3(0, 0, 0.3f));
                    
                ManAnimationController animationController = currentHex.transform.Value.GetComponentInChildren<ManAnimationController>();
                animationController.transform.LookAt(finishPoint);
                animationController.StartRun();
                    
                currentHex.ReplaceManAnimation(ManAnimationType.Run);
            }
        }
    }
}