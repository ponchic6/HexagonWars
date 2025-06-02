using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationStopShootingReactiveSystem : ReactiveSystem<GameEntity>
    {
        private GameContext _game;

        public ManAnimationStopShootingReactiveSystem(IContext<GameEntity> context) : base(context)
        {
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.Destructed);

        protected override bool Filter(GameEntity entity) =>
            entity.hasBattlefield;

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonId);
                GameEntity attackerEntity = _game.GetEntityWithId(entity.battlefield.AttackerHexagonId);

                if (defenderEntity.manAmount.Value <= 0)
                    StopShootingForHexagonsInBattlefield(entity);
                
                if (attackerEntity.manAmount.Value <= 0)
                    StopShootingForHexagonsInBattlefield(entity);
                
                if (entity.isDestructed) 
                    StopShootingForHexagonsInBattlefield(entity);
            }
        }
        
        private void StopShootingForHexagonsInBattlefield(GameEntity battlefield)
        {
            StopShootingForHexagon(battlefield.battlefield.AttackerHexagonId);
            StopShootingForHexagon(battlefield.battlefield.DefenderHexagonId);
        }

        private void StopShootingForHexagon(int hexId)
        {
            GameEntity hexEntity = _game.GetEntityWithId(hexId);
            
            if (hexEntity.manAnimation.Value != ManAnimationType.Shooting)
                return;
            
            ManAnimationController animationController = hexEntity.transform.Value.GetComponentInChildren<ManAnimationController>();
            animationController.transform.localRotation = Quaternion.Euler(90, 0, 0);
            animationController.StartIdle();
            hexEntity.ReplaceManAnimation(ManAnimationType.Idle);
        }
    }
}