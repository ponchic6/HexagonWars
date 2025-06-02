using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationIdleOnPlaceReactiveSystem : ReactiveSystem<GameEntity>
    {
        private GameContext _game;

        public ManAnimationIdleOnPlaceReactiveSystem(IContext<GameEntity> context) : base(context)
        {
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.MigrationComplexityWay);

        protected override bool Filter(GameEntity entity) =>
            entity.migrationComplexityWay.Value.Count > 0 &&
            entity.migrationComplexityWay.Value[0] <= 0;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                GameEntity currentHex = _game.GetEntityWithId(entity.wayIdPoints.Value[0]);
                GameEntity nextHex = _game.GetEntityWithId(entity.wayIdPoints.Value[1]);
                
                if (nextHex.isEnemyHexagon)
                    continue;
                
                if (!currentHex.hasManAnimation)
                    continue;
                
                ManAnimationController animationController = currentHex.transform.Value.GetComponentInChildren<ManAnimationController>();
                animationController.StartIdle();
                animationController.transform.localRotation = Quaternion.Euler(90, 0, 0);
                currentHex.ReplaceManAnimation(ManAnimationType.Idle);
            }
        }
    }
}