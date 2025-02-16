using Code.Gameplay.Features.Battle.DataStructures;
using Code.Gameplay.Features.PopUp.Services;
using Code.Infrastructure.StaticData;
using Entitas;

namespace Code.Gameplay.Features.Battle.Systems
{
    public class WarriorLossesPopUpSystem : IExecuteSystem
    {
        private readonly IPopUpFactory _popUpFactory;
        private readonly CommonStaticData _commonStaticData;
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public WarriorLossesPopUpSystem(IPopUpFactory popUpFactory, CommonStaticData commonStaticData)
        {
            _popUpFactory = popUpFactory;
            _commonStaticData = commonStaticData;
            
            _game = Contexts.sharedInstance.game;

            _entities = _game.GetGroup(GameMatcher.AllOf(GameMatcher.CurrentBattleCooldown, GameMatcher.BattleCooldown));
        }
        
        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.currentBattleCooldown.Value == 0)
                {
                    GameEntity defenderEntity = _game.GetEntityWithId(entity.battlefield.DefenderHexagonContainer.hexagonId);
                    _popUpFactory.CreatePopUp(_commonStaticData.ManSprite, entity.battlefield.DefenderHexagonContainer.warriorsCount, defenderEntity.transform.Value);

                    foreach (WarriorsContainer warriorsContainer in entity.battlefield.AttackerHexagonContainers)
                    {
                        GameEntity attackerEntity = _game.GetEntityWithId(warriorsContainer.hexagonId);
                        _popUpFactory.CreatePopUp(_commonStaticData.ManSprite, warriorsContainer.warriorsCount, attackerEntity.transform.Value);
                    }
                }
            }
        }
    }
}