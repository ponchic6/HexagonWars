using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationStartShootingReactiveSystem : ReactiveSystem<GameEntity>
    {
        private readonly GameContext _game;

        public ManAnimationStartShootingReactiveSystem(IContext<GameEntity> context) : base(context)
        {
            _game = Contexts.sharedInstance.game;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
            context.CreateCollector(GameMatcher.Battlefield.Added());

        protected override bool Filter(GameEntity entity)
        {
            int attackerId = entity.battlefield.AttackerHexagonId;
            GameEntity attackerEntity = _game.GetEntityWithId(attackerId);

            return attackerEntity.hasManAnimation;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                int defenderId = entity.battlefield.DefenderHexagonId;
                int attackerId = entity.battlefield.AttackerHexagonId;
                GameEntity defenderEntity = _game.GetEntityWithId(defenderId);
                GameEntity attackerEntity = _game.GetEntityWithId(attackerId);

                if (attackerEntity.manAnimation.Value != ManAnimationType.Shooting) 
                    TryStartShootAlongDirection(attackerEntity, defenderEntity);
                
                if (defenderEntity.manAnimation.Value != ManAnimationType.Shooting) 
                    TryStartShootAlongDirection(defenderEntity, attackerEntity);
            }
        }
        
        private void TryStartShootAlongDirection(GameEntity currentHex, GameEntity nextHex)
        {
            ManAnimationController animationController = currentHex.transform.Value.GetComponentInChildren<ManAnimationController>();
            animationController.StartShooting();
            
            currentHex.ReplaceManAnimation(ManAnimationType.Shooting);
            
            Vector3 finishPoint = nextHex.transform.Value.position;
            Vector3 direction = finishPoint - currentHex.transform.Value.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 euler = lookRotation.eulerAngles;
            Vector3 currentRotation = animationController.transform.rotation.eulerAngles;
            Vector3 newRotation = new Vector3(currentRotation.x, euler.y, currentRotation.z);
            animationController.transform.rotation = Quaternion.Euler(newRotation);
        }

    }
}