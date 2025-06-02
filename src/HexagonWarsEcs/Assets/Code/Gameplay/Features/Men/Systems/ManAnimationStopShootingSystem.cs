using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Men.Systems
{
    public class ManAnimationStopShootingSystem : IExecuteSystem
    {
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public ManAnimationStopShootingSystem() 
        {
            _game = Contexts.sharedInstance.game;
            _entities = _game.GetGroup(GameMatcher.Battlefield);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
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