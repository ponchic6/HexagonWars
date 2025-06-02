using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationStartShootingSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public ManAnimationStartShootingSystem() 
        {
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.Battlefield);
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.isDestructed)
                    continue;
                
                int defenderId = entity.battlefield.DefenderHexagonId;
                int attackerId = entity.battlefield.AttackerHexagonId;
                GameEntity defenderEntity = _game.GetEntityWithId(defenderId);
                GameEntity attackerEntity = _game.GetEntityWithId(attackerId);

                StartShootingForHexagon(defenderEntity, attackerEntity);
                StartShootingForHexagon(attackerEntity, defenderEntity);
            }
        }

        private void StartShootingForHexagon(GameEntity currentHex, GameEntity nextHex)
        {
            if (currentHex.hasManAnimation && currentHex.manAnimation.Value == ManAnimationType.Shooting)
                return;
            
            TryStartShootAlongDirection(currentHex, nextHex);
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