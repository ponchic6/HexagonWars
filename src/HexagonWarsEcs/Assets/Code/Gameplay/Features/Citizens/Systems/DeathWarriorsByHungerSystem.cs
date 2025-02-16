using Code.Gameplay.Features.Battle.DataStructures;
using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Citizens.Systems
{
    public class DeathWarriorsByHungerSystem : IExecuteSystem
    {
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _hexagons;
        private readonly IGroup<GameEntity> _battlefields;
        private readonly GameContext _game;

        public DeathWarriorsByHungerSystem(CommonStaticData commonStaticData)
        {
            _commonStaticData = commonStaticData;
            _game = Contexts.sharedInstance.game;

            _hexagons = _game.GetGroup(GameMatcher.AllOf(GameMatcher.WarriorsAmount, GameMatcher.CurrentHungerDeathCooldown));
            _battlefields = _game.GetGroup(GameMatcher.Battlefield);
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _battlefields)
            {
                TryKillByHungerAttackers(entity);
                TryKillByHungerDefender(entity);
            }
            
            foreach (GameEntity entity in _hexagons)
            {
                if (entity.currentHungerDeathCooldown.Value == 0)
                    entity.ReplaceWarriorsAmount((int)(entity.warriorsAmount.Value * _commonStaticData.CoefficientWarriorsDeathByHungerInAct));
            }
        }

        private void TryKillByHungerDefender(GameEntity entity)
        {
            WarriorsContainer defenderHexagonContainer = entity.battlefield.DefenderHexagonContainer;

            if (entity.hasCurrentHungerDeathCooldown && entity.currentHungerDeathCooldown.Value == 0)
                defenderHexagonContainer.warriorsCount = (int)(defenderHexagonContainer.warriorsCount * _commonStaticData.CoefficientWarriorsDeathByHungerInAct);
        }

        private void TryKillByHungerAttackers(GameEntity entity)
        {
            foreach (WarriorsContainer warriorsContainer in entity.battlefield.AttackerHexagonContainers)
            {
                if (entity.hasCurrentHungerDeathCooldown && entity.currentHungerDeathCooldown.Value == 0)
                    warriorsContainer.warriorsCount = (int)(warriorsContainer.warriorsCount * _commonStaticData.CoefficientWarriorsDeathByHungerInAct);
            }
        }
    }
}