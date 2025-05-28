using System.Linq;
using Code.Gameplay.Features.Warriors.Services;
using Code.Gameplay.Features.Warriors.View;
using Entitas;
using UnityEngine;

namespace Code.Gameplay.Features.Warriors.Systems
{
    public class SoldiersAnimationStopShootingSystem : IExecuteSystem
    {
        private readonly ISoldiersModelFactory _soldiersModelFactory;
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public SoldiersAnimationStopShootingSystem(ISoldiersModelFactory soldiersModelFactory) 
        {
            _soldiersModelFactory = soldiersModelFactory;
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.Battlefield);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonId);
                GameEntity attackerEntity = _game.GetEntityWithId(entity.battlefield.AttackerHexagonId);

                if (defenderEntity.warriorsAmount.Value <= 0)
                    StopShootingForHexagonsInBattlefield(entity);
                
                if (attackerEntity.warriorsAmount.Value <= 0)
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
            if (!_soldiersModelFactory.HexWithSoldiers.TryGetValue(hexId, out SoldierAnimationController soldierModel))
                return;
            
            soldierModel.transform.localRotation = Quaternion.Euler(90, 0, 0);
            
            if (soldierModel.Animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
                soldierModel.StartIdle();
        }
    }
}