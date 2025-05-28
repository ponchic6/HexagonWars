using Code.Gameplay.Features.Warriors.Services;
using Code.Gameplay.Features.Warriors.View;
using Entitas;

namespace Code.Gameplay.Features.Warriors.Systems
{
    public class SoldiersAnimationStartShootingSystem : IExecuteSystem
    {
        private readonly ISoldiersModelFactory _soldiersModelFactory;
        private readonly GameContext _game;
        private readonly IGroup<GameEntity> _entities;

        public SoldiersAnimationStartShootingSystem(ISoldiersModelFactory soldiersModelFactory) 
        {
            _soldiersModelFactory = soldiersModelFactory;
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
                GameEntity defenderEntity = _game.GetEntityWithId(defenderId);
                GameEntity attackerEntity = _game.GetEntityWithId(entity.battlefield.AttackerHexagonsId[0]);

                StartShootingForHexagon(defenderEntity, attackerEntity);

                foreach (int attackerId in entity.battlefield.AttackerHexagonsId)
                {
                    defenderEntity = _game.GetEntityWithId(defenderId);
                    attackerEntity = _game.GetEntityWithId(attackerId);
                    StartShootingForHexagon(attackerEntity, defenderEntity);
                }
            }
        }

        private void StartShootingForHexagon(GameEntity currentHex, GameEntity nextHex)
        {
            if (!_soldiersModelFactory.HexWithSoldiers.TryGetValue(currentHex.id.Value, out SoldierAnimationController soldierModel))
                return;
            
            if (soldierModel.Animator.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
                return;
            
            _soldiersModelFactory.TryStartShootAlongDirection(currentHex, nextHex);
        }
    }
}